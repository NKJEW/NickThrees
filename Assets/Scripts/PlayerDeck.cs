using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDeck : MonoBehaviourPunCallbacks, IPunObservable
{
    //representation of your hand
	public List<CardData> hand;

    //representation of your visible cards (face up ones)
    public List<CardData> visible;

    //representation of your face down cards
    public List<CardData> hidden;

    //which cards in your hand are playable given the current center pile
    public List<CardData> playable;

    public void FindPlayableCards() {
    	playable.Clear();
    	if (hand.Count > 0) {
    		SortPlayable(hand);
    	} else if (visible.Count > 0) {
    		SortPlayable(visible);
    	} else {
    		playable.AddRange(hidden);
    	}
    }

    void SortPlayable(List<CardData> list) {
    	foreach (CardData c in list) {
			if (CenterPile.instance.IsValidMove(c)) {
				playable.Add(c);
			}
		}
    }

    public string GetPlayerDeckKey() {
    	return CardManager.instance.StringFromCardList(hand) + "/"
    		+ CardManager.instance.StringFromCardList(visible) + "/"
    		+ CardManager.instance.StringFromCardList(hidden);
    }

    public void Setup(string key) {
    	string[] arr = key.split('/');
    	hand = CardManager.instance.CardListFromString(arr[0]);
    	visible = CardManager.instance.CardListFromString(arr[1]);
    	hidden = CardManager.instance.CardListFromString(arr[2]);
    }


    public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
        
    }
}
