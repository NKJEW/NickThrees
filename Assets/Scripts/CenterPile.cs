﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPile : MonoBehaviour {
    List<CardData> cards;

    public void BlowUp() {
        //FIXME
    }

    bool IsReversed() {
        return false; //FIXME
    }

    public bool IsValid(CardData card) {
        return true; //FIXME
    }

    public void AddCard(CardData card) {
        cards.Add(card);
    }

    //probably other stuffs too
}
