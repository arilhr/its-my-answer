using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public enum QuestionType
{
    ADD,
    SUBTRACT
}

public struct Question
{
    public string question;
    public int answer;
}

public class QuestionGenerator : MonoBehaviour
{
    public static QuestionGenerator Instance;

    [Header("Answer")]
    public GameObject answerObject;
    public Transform spawnPos;
    public Vector3 sizeArea;

    [Header("Question Data")]
    public List<QuestionDataItem> questionDataItems;
    public List<QuestionType> questionTypes;
    public int totalQuestion = 20;
    public int totalAnswer = 30;

    private List<Question> questionPool = new List<Question>();


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartGenerateQuestionAndAnswer();
        }
    }

    public void StartGenerateQuestionAndAnswer()
    {
        // generate question pool
        for (int i = 0; i < totalQuestion; i++)
        {
            if (questionTypes.Count == 0)
            {
                Debug.LogError($"Question types not set.");
                return;
            }

            QuestionType questionType = questionTypes[Random.Range(0, questionTypes.Count)];
            Question newQuestion = GenerateQuestionByType(questionType);
            questionPool.Add(newQuestion);
        }

        // spawn all answer
        for (int i = 0; i < totalAnswer; i++)
        {
            int answerToSpawn = 0;

            if (i > totalQuestion - 1) answerToSpawn = Random.Range(1, 10);
            else answerToSpawn = questionPool[i].answer;

            SpawnAnswer(answerToSpawn);
        }

        // set starter question
        foreach (QuestionDataItem q in questionDataItems)
        {
            q.SetQuestion(GetNextQuestion());
        }
    }

    public Question GetNextQuestion()
    {
        Question result = questionPool[0];
        questionPool.Remove(result);

        return result;
    }

    private void SpawnAnswer(int answer)
    {
        GameObject answerItem = PhotonNetwork.InstantiateRoomObject(answerObject.name, GetRandomSpawnAnswerPos(), Quaternion.identity);
        answerItem.GetComponent<AnswerItem>().SetItemAnswer(answer);
    }

    public Vector3 GetRandomSpawnAnswerPos()
    {
        float posX = Random.Range(spawnPos.position.x - sizeArea.x / 2, spawnPos.position.x + sizeArea.x / 2);
        float posY = Random.Range(spawnPos.position.y - sizeArea.y / 2, spawnPos.position.y + sizeArea.y / 2);
        float posZ = Random.Range(spawnPos.position.z - sizeArea.z / 2, spawnPos.position.z + sizeArea.z / 2);
        Vector3 randomPos = new Vector3(posX, posY, posZ);

        return randomPos;
    }

    #region Question Generator
    private Question GenerateQuestionByType(QuestionType type)
    {
        switch(type)
        {
            case QuestionType.ADD:
                return GenerateSumQuestion();
            case QuestionType.SUBTRACT:
                return GenerateSubtractQuestion();
            default:
                return new Question();
        }
    }

    private Question GenerateSumQuestion()
    {
        Question result;

        int a = Random.Range(1, 5);
        int b = Random.Range(1, 5);

        result.question = $"{a} + {b}";
        result.answer = a + b;

        return result;
    }

    private Question GenerateSubtractQuestion()
    {
        Question result;

        int a = Random.Range(1, 10);
        int b = Random.Range(0, a -1);

        result.question = $"{a} - {b}";
        result.answer = a - b;

        return result;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawCube(spawnPos.position, sizeArea);
    }
}
