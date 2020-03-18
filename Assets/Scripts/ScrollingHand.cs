using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingHand : HandVisual {
    public static ScrollingHand instance;

    public Transform center;

    void Awake() {
        instance = this;
    }

    public override void Init(List<CardData> cards, List<CardData> visible, List<CardData> hidden) {
        for (int i = 0; i < center.childCount; i++) {
            Destroy(center.GetChild(i).gameObject);
        }

        for(int i = 0; i < cards.Count; i++) {
            Card newCard = CardManager.instance.CreateCard(cards[i]);
            newCard.Flip(false);
            newCard.loc = Card.Location.Hand;
            newCard.interactable = true;

            newCard.gameObject.transform.position = center.transform.position + Vector3.right * (cards.Count * -0.5f + i) * 1.5f;
            newCard.transform.parent = center;
        }
    }

    void Update() {
        
    }
}
