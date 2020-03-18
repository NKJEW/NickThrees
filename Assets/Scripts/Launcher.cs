using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks {
    public Button playButton;
    public Text nameText;
    public Text logText;

    string gameVersion = "v0.1";
    bool shouldJoin;

    void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;
        logText.text = "";
    }

    void Update() {
        playButton.interactable = !string.IsNullOrEmpty(nameText.text.Trim());
    }

    public void PlayButton() {
        logText.text = "connecting...";
        PhotonNetwork.NickName = nameText.text;
        shouldJoin = true;
        Connect();
    }

    public override void OnConnectedToMaster() {
        if (shouldJoin) {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    void Connect() {
        if (shouldJoin) {
            if (PhotonNetwork.IsConnected) {
                PhotonNetwork.JoinRandomRoom();
            } else {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
    }

    public override void OnJoinedRoom() {
        shouldJoin = false;
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        PhotonNetwork.CreateRoom(null, new RoomOptions());
    }
}
