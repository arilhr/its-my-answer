using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using Photon.Realtime;

public class QuestionDataItem : MonoBehaviourPunCallbacks
{
    public TMP_Text questionText;
    public PhotonView pv;

    public string question;
    public int answer;

    public void SetQuestion(Question data)
    {
        if (pv.IsMine)
            pv.RPC("RpcSetQuestion", RpcTarget.All, data.question, data.answer);
    }


    [PunRPC]
    private void RpcSetQuestion(string question, int answer)
    {
        this.question = question;
        this.answer = answer;

        // set UI
        questionText.text = question;
    }

    public void CheckAnswer(PlayerController answerer, int playerAnswer)
    {
        Debug.Log($"Check Answer");

        if (answer == playerAnswer)
        {
            CorrectAnswer(answerer);
        }
        else
        {
            FalseAnswer(answerer);
        }
    }

    private void CorrectAnswer(PlayerController answerer)
    {
        Debug.Log($"Local Correct");

        if (pv.IsMine)
        {
            SetQuestion(QuestionGenerator.Instance.GetNextQuestion());
            answerer.photonView.Owner.AddScore(1);

            pv.RPC("RpcCorrectAnswer", RpcTarget.All);
        }

        if (answerer.photonView.Owner == pv.Owner)
        {
            AnswerItem answerPicked = answerer.currentItemPicked;
            answerer.DropItem();
            answerPicked.SetItemActive(false);
        }
        else
        {
            pv.RPC("RpcPlayerCorrectAnswer", RpcTarget.Others);
        }
    }

    [PunRPC]
    private void RpcCorrectAnswer()
    {
        Debug.Log($"Correct");
    }

    [PunRPC]
    private void RpcPlayerCorrectAnswer()
    {
        Debug.Log($"Player Correct");

        PlayerController player = PlayerController.GetLocalPlayer();
        AnswerItem answerPicked = player.currentItemPicked;

        player.DropItem();
        answerPicked.SetItemActive(false);
    }

    private void FalseAnswer(PlayerController answerer)
    {
        Debug.Log($"Local False");

        if (pv.IsMine)
        {
            pv.RPC("RpcFalseAnswer", RpcTarget.All);
        }

        if (answerer.photonView.Owner == pv.Owner)
        {
            AnswerItem answerItem = answerer.currentItemPicked;
            answerer.DropItem();
            answerItem?.RandomizePosition();
        }
        else
        {
            pv.RPC("RpcPlayerFalseAnswer", RpcTarget.Others);
        }
    }

    [PunRPC]
    private void RpcFalseAnswer()
    {
        Debug.Log($"False");
    }

    [PunRPC]
    private void RpcPlayerFalseAnswer()
    {
        Debug.Log($"Player False Answer");

        PlayerController player = PlayerController.GetLocalPlayer();
        AnswerItem answerItem = player.currentItemPicked;
        player.DropItem();
        answerItem?.RandomizePosition();
    }
}
