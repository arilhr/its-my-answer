using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        StartCoroutine(CreateRoom());
    }

    private void Update()
    {
        Debug.Log($"{PhotonNetwork.IsConnected}");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log($"Connecting to master..");
    }

    public IEnumerator CreateRoom()
    {
        yield return new WaitForSeconds(2f);

        PhotonNetwork.CreateRoom("Test");
    }
}
