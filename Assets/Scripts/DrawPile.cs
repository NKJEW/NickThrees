using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPile : MonoBehaviour {
    List<CardData> pile;

    public void Init(List<CardData> cards) {
        pile = cards;
    }

    public bool IsEmpty() {
        return pile.Count == 0;
    }

    public CardData DrawCard() {
        CardData newCard = pile[pile.Count - 1];
        pile.Remove(newCard);
        return newCard;
    }
}
