using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks {
    public Text sliderText;
    public Slider slider;
    public GameObject preGameMenu;

    void Start() {
        if (PhotonNetwork.IsMasterClient) {
            preGameMenu.SetActive(true);
        }

        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    public void OnStartGame() {
        preGameMenu.SetActive(false);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        GameManager.instance.LoadGame(Mathf.RoundToInt(slider.value));
    }

    public void OnSliderUpdated() {
        sliderText.text = slider.value.ToString();
    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene(0);
    }

    public void Leave() {
        PhotonNetwork.LeaveRoom();
    }
}
