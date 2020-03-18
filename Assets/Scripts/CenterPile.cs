using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPile : MonoBehaviour {
    List<CardData> cards;

    public CardData TopCardVal() {
        return cards[cards.Count - 1].GetValue();
    }

    public void BlowUp() {
        if (TopCardVal() == 10) {
            cards.Clear();
        }
    }

    bool IsReversed() {
        return TopCardVal() == 7;
    }

    public bool IsValidMove(CardData card) {
        int playerVal = card.GetValue();
        if (playerVal == 10 || playerVal == 2) {
            return true;
        } else if (IsReversed()) {
            return playerVal <= TopCardVal();
        } else {
            return card.GetValue() >= TopCardVal();
        }
    }

    public void AddCard(CardData card) {
        cards.Add(card);
    }

    //probably other stuffs too
}
