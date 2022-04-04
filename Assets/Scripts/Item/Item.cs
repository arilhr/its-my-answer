using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Item : MonoBehaviour, IPickable
{
    private PlayerController currentCarrier;
    private Transform currentPos;
    private Rigidbody rb;

    private bool isPicked = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (currentPos != null)
        {
            transform.position = currentPos.position;
        }
    }

    public void Picked(PlayerController player)
    {
        GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);

        currentCarrier = player;
        currentPos = player.pickedItemPos;

        GetComponent<PhotonView>().RPC("OnChangeKinematic", RpcTarget.All, true);
    }

    
    public void Dropped()
    {
        currentPos = null;
        currentCarrier = null;

        GetComponent<PhotonView>().RPC("OnChangeKinematic", RpcTarget.All, false);
    }

    [PunRPC]
    public void OnChangeKinematic(bool isKinematic)
    {
        rb.isKinematic = isKinematic;
    }
}
