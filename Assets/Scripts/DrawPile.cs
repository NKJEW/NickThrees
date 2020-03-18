using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawPile : MonoBehaviour {
    public static DrawPile instance;
    public GameObject cardVisual;
    public Text cardCount;

    List<CardData> pile;

    void Awake() {
        instance = this;
    }

    public void Init(List<CardData> cards) {
        pile = cards;
    }

    public bool IsEmpty() {
        return pile.Count == 0;
    }

    public CardData DrawCard(bool shouldRemove = true) {
        CardData randomCard = pile[Random.Range(0, pile.Count)];
        UpdateText();
        return DrawCard(randomCard);
    }

    public CardData DrawCard(CardData card) {
        pile.Remove(card);
        UpdateText();
        return card;
    }

    void UpdateText() {
        cardVisual.SetActive(!IsEmpty());
        cardCount.text = pile.Count.ToString();
    }
}
