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
    public GameObject selectionSkinPanel;

    [Header("Components")]
    public TMP_InputField inputUsernameField;
    public TMP_Text profileNameText;
    public Button cancelButton;
    public GameObject playerDisplay;

    [Header("Properties")]
    public byte playerToPlay;

    [Header("Input Username")]
    private int minUsername = 3;
    private int maxusername = 8;

    [Scene]
    public string gameScene;

    private Coroutine matchmakingCoroutine = null;

    private void Update()
    {
        UpdateUsername();
    }

    private void UpdateUsername()
    {
        if (!PhotonNetwork.IsConnected) return;

        profileNameText.text = PhotonNetwork.NickName;
    }

    public void ShowInputUsername()
    {
        inputUsernamePanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void ConfirmInputUsername()
    {
        if (inputUsernameField.text.Length < minUsername) return;
        if (inputUsernameField.text.Length > maxusername) return;

        AccountManager.Instance.SetUsername(inputUsernameField.text);
        inputUsernamePanel.SetActive(false);
        menuPanel.SetActive(true);

        profileNameText.text = PhotonNetwork.NickName;
    }

    public void ShowSelectionSkin()
    {
        menuPanel.SetActive(false);
        selectionSkinPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        // disable all panels except menu panel
        inputUsernamePanel.SetActive(false);
        selectionSkinPanel.SetActive(false);
        roomPanel.SetActive(false);
        
        menuPanel.SetActive(true);
        playerDisplay.SetActive(true);
    }

    public void Play()
    {
        if (!PhotonNetwork.IsConnectedAndReady) return;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CleanupCacheOnLeave = false;
        roomOptions.MaxPlayers = playerToPlay;

        PhotonNetwork.JoinRandomOrCreateRoom(null, 0, MatchmakingMode.FillRoom, null, null, null, roomOptions, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Join Room {PhotonNetwork.CurrentRoom.Name}");

        roomPanel.SetActive(true);
        menuPanel.SetActive(false);
        playerDisplay.SetActive(false);
        cancelButton.interactable = true;

        matchmakingCoroutine = StartCoroutine(WaitForOpponent());
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        Debug.Log($"Left Room..");
    }

    private IEnumerator WaitForOpponent()
    {
        yield return new WaitUntil(() => PhotonNetwork.CurrentRoom.PlayerCount == playerToPlay);

        cancelButton.interactable = false;

        yield return new WaitForSeconds(3f);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(gameScene);
        } 
    }

    public void CancelMatchmaking()
    {
        if (!PhotonNetwork.InRoom) return;

        StopCoroutine(matchmakingCoroutine);

        BackToMenu();

        PhotonNetwork.LeaveRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Debug.Log(cause);
    }
}
