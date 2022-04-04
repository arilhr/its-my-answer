using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [Header("Panel")]
    public GameObject inputUsernamePanel;
    public GameObject menuPanel;
    public GameObject roomPanel;

    [Header("Components")]
    public TMP_InputField inputUsernameField;
    public TMP_Text profileNameText;

    [Scene]
    public string gameScene;

    // Start is called before the first frame update
    void Start()
    {
        inputUsernamePanel.SetActive(true);
        menuPanel.SetActive(false);

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ConfirmInputUsername()
    {
        if (inputUsernameField.text.Length < 6) return;

        PhotonNetwork.NickName = inputUsernameField.text;
        inputUsernamePanel.SetActive(false);
        menuPanel.SetActive(true);

        profileNameText.text = PhotonNetwork.NickName;
    }

    public void Play()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log($"On connect master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Join Room");
        Debug.Log($"{PhotonNetwork.CurrentRoom.Name}");

        roomPanel.SetActive(true);
        menuPanel.SetActive(false);

        if (PhotonNetwork.IsMasterClient) StartCoroutine(WaitForOpponent());
    }

    private IEnumerator WaitForOpponent()
    {
        yield return new WaitUntil(() => PhotonNetwork.CurrentRoom.PlayerCount == 2);

        yield return new WaitForSeconds(3f);

        PhotonNetwork.LoadLevel(gameScene);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Debug.Log(cause);
    }
}
