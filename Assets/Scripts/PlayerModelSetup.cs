using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelSetup : MonoBehaviour
{
    public PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = PlayerController.GetPlayerObject(pv.Owner);

        if (player != null)
            transform.SetParent(player.transform);

        transform.localPosition = Vector3.zero;
    }
}
