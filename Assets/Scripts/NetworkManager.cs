using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks {

    public GameObject preGameMenu;

    void Start() {
        if (PhotonNetwork.IsMasterClient) {
            preGameMenu.SetActive(true);
        }
    }

    public void OnStartGame() {
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene(0);
    }

    public void Leave() {
        PhotonNetwork.LeaveRoom();
    }
}
