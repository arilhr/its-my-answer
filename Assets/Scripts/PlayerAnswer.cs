using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAnswer : MonoBehaviour
{
    public TMP_Text answerText;

    public void SetAnswerPicked(int answer)
    {
        answerText.text = answer.ToString();
        gameObject.SetActive(true);
    }

    public void SetAnswerDropped()
    {
        answerText.text = "";
        gameObject.SetActive(false);
    }
}
