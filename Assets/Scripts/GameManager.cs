using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public GameObject preGameCover;

    List<CardData> allCards;
    PlayerDeck[] allPlayers;

    void Awake() {
        instance = this;
        preGameCover.SetActive(true);
    }

    public void LoadGame(int numDecks) {
        FindObjectOfType<PlayerDeck>().photonView.RPC("RpcStartGame", RpcTarget.All, numDecks); //SUPER HACKY
    }

    public void OnDeckInitialized(int numDecks) {
        preGameCover.SetActive(false);

        allCards = new List<CardData>();
        for (int i = 0; i < numDecks; i += 1) {
            allCards.AddRange(CardManager.instance.DeckCopy());
        }

        DrawPile.instance.Init(allCards);

        if (PhotonNetwork.IsMasterClient) {
            allPlayers = FindObjectsOfType<PlayerDeck>();
            PlayerDeck me = null;
            for (int i = 0; i < allPlayers.Length; i++) {
                List<CardData> hidden = DrawThree();
                List<CardData> visible = DrawThree();
                List<CardData> hand = DrawThree();

                string data = CardManager.instance.StringFromCardList(hand) + "/"
                + CardManager.instance.StringFromCardList(visible) + "/"
                + CardManager.instance.StringFromCardList(hidden);

                allPlayers[i].MasterInit(i, hand, visible, hidden);
                allPlayers[i].photonView.RPC("RpcInit", RpcTarget.Others, i, data);

                if (allPlayers[i].photonView.IsMine) {
                    me = allPlayers[i];
                }
            }

            me.photonView.RPC("RpcStartTurn", RpcTarget.All);
        }
    }

    List<CardData> DrawThree() {
        List<CardData> list = new List<CardData>();
        for (int i = 0; i < 3; i++) {
            list.Add(DrawPile.instance.DrawCard());
        }

        return list;
    }
}
