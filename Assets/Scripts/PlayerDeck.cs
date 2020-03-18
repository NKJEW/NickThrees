using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class PlayerDeck : MonoBehaviourPunCallbacks, IPunObservable {
    struct ListPointer {
        public List<CardData> list;
        public int index;

        public ListPointer(List<CardData> list, int index) {
            this.list = list;
            this.index = index;
        }

        public static List<ListPointer> AllPointers(List<CardData> list) {
            List<ListPointer> newList = new List<ListPointer>();
            for (int i = 0; i < list.Count; i++) {
                newList.Add(new ListPointer(list, i));
            }
            return newList;
        }

        public CardData GetCard() { return list[index]; }
    }

    //representation of your hand
	List<CardData> hand;

    //representation of your visible cards (face up ones)
    List<CardData> visible;

    //representation of your face down cards
    List<CardData> hidden;

    //which cards in your hand are playable given the current center pile
    List<ListPointer> playable;

    static Dictionary<int, PlayerDeck> players = new Dictionary<int, PlayerDeck>();
    public static PlayerDeck local;
    int globalId;
    bool isMyTurn;

    [PunRPC]
    void RpcInit(int id, string data) {
        Setup(data);
        Init(id);
    }

    public void MasterInit(int id, List<CardData> newHand, List<CardData> newVisible, List<CardData> newHidden) {
        hand = newHand;
        visible = newVisible;
        hidden = newHidden;

        Init(id);
    }

    void Init(int id) {
        globalId = id;
        players.Add(id, this);

        if (photonView.IsMine) {
            local = this;
            UpdateUI();
        }
    }

    [PunRPC]
    void RpcStartTurn() {
        isMyTurn = true;
        FindPlayableCards();

        if (photonView.IsMine) {
            if (playable.Count == 0 && !DrawPile.instance.IsEmpty()) {
                DrawPile.instance.EnableManualDraw();
            }

            UpdateUI();
        }
    }

    public void ManualDraw() {
        DrawCard();
        UpdateUI();
    }

    public void PlayCard(Card card) {
        if (!isMyTurn) {
            card.ResetPos();
            return;
        }

        int index = -1;
        if (hand.Count > 0) {
            if (card.loc == Card.Location.Hand) {
                index = hand.IndexOf(card.data);
            } else {
                card.ResetPos();
                return;
            }
        } else if (visible.Count > 0) {
            if (card.loc == Card.Location.Visible) {
                index = visible.IndexOf(card.data);
            } else {
                card.ResetPos();
                return;
            }
        } else {
            index = hidden.IndexOf(card.data);
        }

        photonView.RPC("RpcPlayCard", RpcTarget.All, index);
    }

    [PunRPC]
    void RpcPlayCard(int id) {
        CardData card;
        if (hand.Count > 0) {
            card = hand[id];
            hand.RemoveAt(id);
        } else if (visible.Count > 0) {
            card = visible[id];
            visible.RemoveAt(id);
        } else {
            card = hidden[id];
            visible.RemoveAt(id);
        }

        CenterPile.Result result = CenterPile.instance.AddCard(card);
        switch (result) {
            case CenterPile.Result.Valid:
                FillHand();
                StartNextTurn();
                break;
            case CenterPile.Result.Invalid:
                hand.AddRange(CenterPile.instance.TakePile());
                StartNextTurn();
                break;
            case CenterPile.Result.Explode:
                FindPlayableCards();
                if (photonView.IsMine) {
                    UpdateUI();
                }
                break;
        }
    }

    void FillHand() {
        if (photonView.IsMine) {
            while (hand.Count < 3 && !DrawPile.instance.IsEmpty()) {
                DrawCard();
            }
        }
    }

    void DrawCard() {
        CardData newCard = DrawPile.instance.DrawCard();
        photonView.RPC("RpcDrawCard", RpcTarget.Others, newCard.GetKey());
        hand.Add(newCard);
    }

    [PunRPC]
    void RpcDrawCard(string key) {
        CardData newCard = CardManager.instance.PullDeckCard(key);
        hand.Add(newCard);
    }

    void StartNextTurn() {
        isMyTurn = false;
        if (photonView.IsMine) {
            UpdateUI();
            GetNextPlayer().photonView.RPC("RpcStartTurn", RpcTarget.All);
        }
    }

    PlayerDeck GetNextPlayer() {
        List<int> keys = players.Keys.ToList();
        keys.Sort();

        if (globalId == keys[keys.Count - 1]) {
            return players[keys[0]];
        }
        return players[keys[keys.IndexOf(globalId) + 1]];
    }

    void FindPlayableCards() {
        playable = new List<ListPointer>();
    	if (hand.Count > 0) {
            SortPlayable(hand);
        } else if (visible.Count > 0) {
            SortPlayable(visible);
    	} else {
            playable = ListPointer.AllPointers(hidden);
    	}
    }

    void SortPlayable(List<CardData> list) {
        for (int i = 0; i < list.Count; i++) {
			if (CenterPile.instance.IsValidMove(list[i])) {
				playable.Add(new ListPointer(list, i));
			}
		}
    }

    void Setup(string key) {
    	string[] arr = key.Split('/');
    	hand = CardManager.instance.CardListFromString(arr[0]);
    	visible = CardManager.instance.CardListFromString(arr[1]);
    	hidden = CardManager.instance.CardListFromString(arr[2]);
    }

    [PunRPC]
    void RpcStartGame(int numDecks) {
        GameManager.instance.OnDeckInitialized(numDecks);
    }

    void UpdateUI() {
        ScrollingHand.instance.Init(hand, visible, hidden);
    }

    public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
        
    }
}
