﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CardValue {
    public string display;
    public int val;
}

public struct CardData {
    public enum Suit {
        Spades = 0,
        Hearts = 1,
        Clubs = 2,
        Diamonds = 3
    }

    CardValue baseData;
    Suit suit;

    public CardData(CardValue baseData, Suit suit) {
        this.baseData = baseData;
        this.suit = suit;
    }

    public Suit GetSuit() { return suit; }
    public int GetValue() { return baseData.val; }
    public string GetKey() { return string.Format("{0}-{1}", (int)GetSuit(), GetValue()); }
}

public class CardManager : MonoBehaviour {
    public static CardManager instance;

    public List<CardValue> cardValues;

    List<CardData> deck;

    void Awake() {
        instance = this;

        GenerateDeck();
    }

    void GenerateDeck() {
        deck = new List<CardData>();

        for (int i = 0; i < 4; i += 1) {
            CardData.Suit suit = (CardData.Suit)i;
            foreach (CardValue card in cardValues) {
                deck.Add(new CardData(card, suit));
            }
        }
    }

    public List<CardData> DeckCopy() {
        return new List<CardData>(deck);
    }
}
