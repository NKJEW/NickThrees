using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {
    public Text topText;
    public Text bottomText;
    public Transform layoutParent;

    public enum Location {
        Hand,
        Visible,
        Hidden
    }
    public Location loc;
    public CardData data;
    public bool interactable;
    Collider2D col;

    Vector3 offset;
    Vector3 originalPos;
    bool isClicked;

    void Awake() {
        col = GetComponent<Collider2D>();
    }

    public void Init(CardData data) {
        this.data = data;

        topText.text = data.GetDisplay();
        bottomText.text = data.GetDisplay();
        Color textColor = (((int)data.GetSuit()) % 2 == 0) ? Color.black : Color.red;
        topText.color = textColor;
        bottomText.color = textColor;

        Transform layout = layoutParent.Find(data.GetDisplay());
        layout.gameObject.SetActive(true);
        SpriteRenderer[] srends = layout.GetComponentsInChildren<SpriteRenderer>();
        Sprite sprite = CardManager.instance.GetSpriteForSuit(data.GetSuit());
        foreach (SpriteRenderer srend in srends) {
            if (!srend.gameObject.name.Contains("Face")) {
                srend.sprite = sprite;
            }
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            originalPos = transform.position;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (interactable && Physics2D.Raycast(mousePos, Vector2.zero).collider == col) {
                isClicked = true;
                offset = transform.position - mousePos;
            }
        }

        if (Input.GetMouseButtonUp(0) && isClicked) {
            isClicked = false;
            if (transform.position.y > -2) {
                PlayerDeck.local.PlayCard(this);
            } else {
                ResetPos();
            }
        }
    }

    public void ResetPos() {
        transform.position = originalPos;
    }

    public void Flip(bool shouldHide) {
        transform.rotation = Quaternion.Euler(0, shouldHide ? 180 : 0, 0);
    }

    void LateUpdate() {
        if (isClicked) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos + offset;
        }
    }
}


