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

    public void CheckAnswer(PlayerController answerer)
    {
        Debug.Log($"Check Answer from {answerer.photonView.Owner.NickName}");

        if (answer == answerer.currentAnswer)
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
        Debug.Log($"{answerer.photonView.Owner.NickName} is correct");

        SetQuestion(QuestionGenerator.Instance.GetNextQuestion());
        answerer.photonView.Owner.AddScore(1);

        pv.RPC("RpcPlayerCorrectAnswer", answerer.photonView.Owner);
    }

    [PunRPC]
    private void RpcPlayerCorrectAnswer()
    {
        PlayerController player = PlayerController.GetLocalPlayer();
        AnswerItem answerPicked = player.currentItemPicked;

        player.DropItem();
        answerPicked.SetItemActive(false);

        // play correct audio
        QuestionSFX.Instance.PlayCorrectSFX();
    }

    private void FalseAnswer(PlayerController answerer)
    {
        Debug.Log($"{answerer.photonView.Owner.NickName} is wrong");

        pv.RPC("RpcPlayerFalseAnswer", answerer.photonView.Owner);
    }

    [PunRPC]
    private void RpcPlayerFalseAnswer()
    {
        Debug.Log($"You wrong!");

        PlayerController player = PlayerController.GetLocalPlayer();
        AnswerItem answerItem = player.currentItemPicked;
        player.DropItem();
        answerItem?.RandomizePosition();

        // play false audio
        QuestionSFX.Instance.PlayFalseSFX();
    }
}
