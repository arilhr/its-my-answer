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
            PlayerController player = other.GetComponent<PlayerController>();

            if (question.pv.IsMine && player.currentAnswer != -1)
            {
                question.CheckAnswer(player);
            }
        }
    }
}
