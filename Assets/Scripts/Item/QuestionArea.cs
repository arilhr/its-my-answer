using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionArea : MonoBehaviour
{
    public QuestionDataItem question;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (question.pv.IsMine)
            {
                PlayerController player = other.GetComponent<PlayerController>();

                if (player.currentItemPicked == null) return;

                question.CheckAnswer(player, player.currentAnswer);
            }
        }
    }
}
