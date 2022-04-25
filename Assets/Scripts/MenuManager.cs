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

    [Header("Properties")]
    public byte playerToPlay;

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

    private void ShowInputUsername()
    {
        inputUsernamePanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void ConfirmInputUsername()
    {
        if (inputUsernameField.text.Length < 6) return;

        PhotonNetwork.NickName = inputUsernameField.text;
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
        inputUsernamePanel.SetActive(false);
        selectionSkinPanel.SetActive(false);
        menuPanel.SetActive(true);
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
        cancelButton.interactable = true;

        if (PhotonNetwork.IsMasterClient) matchmakingCoroutine = StartCoroutine(WaitForOpponent());
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

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(gameScene);
    }

    public void CancelMatchmaking()
    {
        if (!PhotonNetwork.InRoom) return;

        StopCoroutine(matchmakingCoroutine);
        
        menuPanel.SetActive(true);
        roomPanel.SetActive(false);

        PhotonNetwork.LeaveRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Debug.Log(cause);
    }
}
