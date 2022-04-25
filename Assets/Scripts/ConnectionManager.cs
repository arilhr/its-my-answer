using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;

public class ConnectionManager : SingletonPunCallback<ConnectionManager>
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        ConnectToServer();
    }

    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log($"<color=green>On connect master</color>");

        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Debug.Log($"<color=red>Disconnected from server: {cause}</color>");

        ConnectToServer();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ConnectionManager))]
public class ConnectionManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Disconnect"))
        {
            PhotonNetwork.Disconnect();
        }
    }
}
#endif