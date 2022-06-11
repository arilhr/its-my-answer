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
    private Transform droppedPos;
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
        droppedPos = player.droppedItemPos;
        

        pv.RPC("OnPicked", RpcTarget.All, true);
    }

    
    public void Dropped()
    {
        transform.position = droppedPos.position;

        droppedPos = null;
        currentCarrier = null;

        pv.RPC("OnPicked", RpcTarget.All, false);
    }

    [PunRPC]
    public void OnPicked(bool cond)
    {
        rb.isKinematic = cond;
        isPicked = cond;
        gameObject.SetActive(!cond);
    }

    public void SetItemActive(bool cond)
    {
        pv.RPC("RpcItemActive", RpcTarget.All, cond);
    }

    [PunRPC]
    private void RpcItemActive(bool cond)
    {
        gameObject.SetActive(cond);
    }

    public void RandomizePosition()
    {
        transform.position = QuestionGenerator.Instance.GetRandomSpawnAnswerPos();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!pv.AmOwner) return;

        if (other.CompareTag("BottomBorder") && !isPicked)
        {
            RandomizePosition();
        }
    }
}
