using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAnswer : MonoBehaviour
{
    public List<TMP_Text> answerText;

    public void SetAnswerPicked(int answer)
    {
        SetAnswerTextUI(answer.ToString());
        gameObject.SetActive(true);
    }

    public void SetAnswerDropped()
    {
        SetAnswerTextUI("");
        gameObject.SetActive(false);
    }

    public void SetAnswerTextUI(string answer)
    {
        foreach (TMP_Text t in answerText)
        {
            t.text = answer;
        }
    }
}
