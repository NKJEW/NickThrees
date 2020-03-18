using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPile : MonoBehaviour {
    List<CardData> cards;

    public CardData TopCard() {
        return cards[cards.Count - 1];
    }

    public void BlowUp() {
        if (TopCard().GetValue() == 10) {
            cards.Clear();
        }
    }

    bool IsReversed() {
        return TopCard().GetValue() == 7;
    }

    public bool IsValidMove(CardData card) {
        return card.GetValue() >= TopCard().GetValue();
    }

    public void AddCard(CardData card) {
        cards.Add(card);
    }

    //probably other stuffs too
}
