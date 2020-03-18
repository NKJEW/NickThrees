using System.Collections;
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
    public string GetDisplay() { return baseData.display; }
    public string GetKey() { return string.Format("{0}-{1}", (int)GetSuit(), GetValue()); }
}

public class CardManager : MonoBehaviour {
    public static CardManager instance;

    public List<CardValue> cardValues;
    public GameObject cardPrefab;
    public List<Sprite> suitSprites;

    List<CardData> deck;
    Dictionary<string, CardData> keyToCard;

    void Awake() {
        instance = this;

        GenerateDeck();
    }

    void GenerateDeck() {
        deck = new List<CardData>();
        keyToCard = new Dictionary<string, CardData>();

        for (int i = 0; i < 4; i += 1) {
            CardData.Suit suit = (CardData.Suit)i;
            foreach (CardValue card in cardValues) {
                CardData c = new CardData(card, suit);
                deck.Add(c);
                keyToCard.Add(c.GetKey(), c);
            }
        }
    }

    public List<CardData> DeckCopy() {
        return new List<CardData>(deck);
    }

    public string StringFromCardList(List<CardData> list) {
        string str = "";
        for (int i = 0; i < list.Count; i += 1) {
            str += list[i].GetKey();
            if (i < list.Count - 1) {
                str += ",";
            }
        }
        return str;
    }

    public List<CardData> CardListFromString(string str) {
        string[] arr = str.Split(',');
        List<CardData> list = new List<CardData>();
        foreach (string key in arr) {
            CardData newCard = PullDeckCard(key);
            list.Add(newCard);
        }
        return list;
    }

    public CardData PullDeckCard(string key) {
        CardData newCard = keyToCard[key];
        DrawPile.instance.DrawCard(newCard);
        return newCard;
    }

    public Card CreateCard(CardData newData) {
        GameObject newCardObj = Instantiate(cardPrefab, transform.position, Quaternion.identity);
        Card newCard = newCardObj.GetComponent<Card>();
        newCard.Init(newData);
        return newCard;
    }

    public Sprite GetSpriteForSuit(CardData.Suit suit) {
        return suitSprites[(int)suit];
    }
}
