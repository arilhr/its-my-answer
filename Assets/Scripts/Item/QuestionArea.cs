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

            Debug.Log($"Collide with player {player.currentAnswer}");

            if (question.pv.IsMine)
            {
                question.CheckAnswer(player, player.currentAnswer);
            }

        }
    }
}
