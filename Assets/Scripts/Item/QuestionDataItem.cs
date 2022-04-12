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
        SetQuestion(QuestionGenerator.Instance.GetNextQuestion());
        answerer.photonView.Owner.AddScore(1);

        pv.RPC("RpcCorrectAnswer", RpcTarget.All);
    }

    [PunRPC]
    private void RpcCorrectAnswer()
    {
        Debug.Log($"Correct");
    }

    private void FalseAnswer(PlayerController answerer)
    {
        pv.RPC("RpcFalseAnswer", RpcTarget.All);
    }

    [PunRPC]
    private void RpcFalseAnswer()
    {
        Debug.Log($"False");
    }
}
