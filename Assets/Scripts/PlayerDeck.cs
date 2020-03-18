﻿using System.Collections;
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
    	for (int i = 0; i < hand.Count; i++) {
			if (CenterPile.instance.IsValidMove(list[i])) {
				playable.Add(list[i]);
			}
		}
    }


    public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
        
    }
}
