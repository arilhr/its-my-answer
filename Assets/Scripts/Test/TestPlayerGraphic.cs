using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerGraphic : MonoBehaviour
{
    public PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = TestPlayerAnimator.GetPlayer(pv.Owner);

        if (player != null)
            transform.SetParent(player.transform);
    }
}
