using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawnPlayer : MonoBehaviour
{
    public GameObject player;

    public List<Transform> spawnPos;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(player.name, spawnPos[0].position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(player.name, spawnPos[1].position, Quaternion.identity);
        }
    }
}
