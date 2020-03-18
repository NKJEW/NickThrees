using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
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
    	} else {
    		SortPlayable(visible);
    		SortPlayable(hidden);
    	}
    }

    void SortPlayable(List<CardData> list) {
    	for (int i = 0; i < hand.Count; i++) {
			if (CenterPile.instance.IsValidMove(list[i])) {
				playable.Add(list[i]);
			}
		}
    }
    //
}
