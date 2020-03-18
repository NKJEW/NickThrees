using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour {
    public static DeckManager instance;

    List<CardData> allCards;

    public void LoadGame(int numDecks) {
        for (int i = 0; i < numDecks; i += 1) {
            allCards.AddRange(CardManager.instance.DeckCopy());
        }
    }
}
