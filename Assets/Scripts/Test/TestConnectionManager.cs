using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestConnectionManager : SingletonPunCallback<ConnectionManager>
{
    [Scene]
    public string gameScene;

    public void ConnectToServer()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void DisconnectFromServer()
    {
        PhotonNetwork.Disconnect();
    }

    public void CreateRoom()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log($"Connected to server...");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Debug.Log(cause);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(WaitToPlayGame());

        Debug.Log($"Join room...");
    }

    private IEnumerator WaitToPlayGame()
    {
        yield return new WaitUntil(() => PhotonNetwork.CurrentRoom.PlayerCount > 1);

        Debug.Log($"Play Game...");

        PhotonNetwork.LoadLevel(gameScene);
    }
}

#if UNITY_EDITOR
// Custom UI Inspector
[CustomEditor(typeof(TestConnectionManager))]
public class TEstConnectionManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TestConnectionManager cm = (TestConnectionManager)target;

        if (GUILayout.Button("Connect"))
        {
            cm.ConnectToServer();
        }

        if (GUILayout.Button("Disconnect"))
        {
            cm.DisconnectFromServer();
        }

        if (GUILayout.Button("Join/Create Random Roon"))
        {
            cm.CreateRoom();
        }
    }
}
#endif