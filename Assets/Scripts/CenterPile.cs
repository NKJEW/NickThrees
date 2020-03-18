using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CenterPile : MonoBehaviour {
    public static CenterPile instance;
    public Text cardCount;

    List<CardData> cards = new List<CardData>();
    Card curCard;

    public enum Result {
        Valid,
        Invalid,
        Explode
    }

    void Awake() {
        instance = this;
    }

    public int TopCardVal() {
        return cards[cards.Count - 1].GetValue();
    }

    bool IsReversed() {
        return TopCardVal() == 7;
    }

    public bool IsValidMove(CardData card) {
        if (cards.Count == 0) {
            return true;
        }

        int playerVal = card.GetValue();
        if (playerVal == 10 || playerVal == 2) {
            return true;
        } else if (IsReversed()) {
            return playerVal <= TopCardVal();
        } else {
            return card.GetValue() >= TopCardVal();
        }
    }

    public Result AddCard(CardData card) {
        bool wasBad = !IsValidMove(card);

        cards.Add(card);
        UpdateText();
        if (curCard != null) {
            Destroy(curCard.gameObject);
        }

        if (wasBad) {
            return Result.Invalid;
        }

        if (TopCardVal() == 10) {
            cards.Clear();
            UpdateText();
            return Result.Explode;
        }

        curCard = CardManager.instance.CreateCard(card);
        curCard.gameObject.transform.position = transform.position;
        return Result.Valid;
    }

    public List<CardData> TakePile() {
        List<CardData> copy = new List<CardData>(cards);
        cards.Clear();
        UpdateText();
        return copy;
    }

    void UpdateText() {
        cardCount.text = cards.Count.ToString();
    }

    //probably other stuffs too
}
