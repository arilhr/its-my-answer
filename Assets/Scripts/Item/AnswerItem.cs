using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class AnswerItem : MonoBehaviour, IPickable
{
    public TMP_Text answerText;
    public PhotonView pv;

    private PlayerController currentCarrier;
    private Transform currentPos;
    private Rigidbody rb;

    public int answer { private set; get; }
    public bool isPicked { private set; get; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        isPicked = false;
    }

    private void Update()
    {
        if (!pv.IsMine) return;

        if (currentPos != null)
        {
            transform.position = currentPos.position;
        }
    }

    public void SetItemAnswer(int answer)
    {
        this.answer = answer;
        answerText.text = answer.ToString();

        pv.RPC("RpcSetItemAnswer", RpcTarget.All, answer);
    }

    [PunRPC]
    private void RpcSetItemAnswer(int answer)
    {
        this.answer = answer;
        answerText.text = answer.ToString();
    }

    public void Picked(PlayerController player)
    {
        pv.TransferOwnership(PhotonNetwork.LocalPlayer);

        currentCarrier = player;
        currentPos = player.itemPickedPos;

        pv.RPC("OnPicked", RpcTarget.All, true);
    }

    
    public void Dropped()
    {
        currentPos = null;
        currentCarrier = null;

        pv.RPC("OnPicked", RpcTarget.All, false);
    }

    [PunRPC]
    public void OnPicked(bool cond)
    {
        rb.isKinematic = cond;
        isPicked = cond;
    }

    public void RandomizePosition()
    {
        transform.position = QuestionGenerator.Instance.GetRandomSpawnAnswerPos();
    }
}
