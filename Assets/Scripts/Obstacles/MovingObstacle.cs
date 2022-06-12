using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [Header("Properties")]
    public float speed = 1f;
    public float timeToMove = 1f;
    public List<Vector3> targetPosList;
    public float targetPosOffset = 0.1f;

    [Header("References")]
    public Rigidbody rb;

    protected int currentTargetIndex = 0;
    protected Vector3 currentTargetPos;

    protected bool isMove = false;

    protected virtual void Start()
    {
        StartMove();
    }

    protected virtual void FixedUpdate()
    {
        if (isMove)
        {
            Move();
            CheckArrivedToTarget(() => StartCoroutine(OnWaitNextMove()));
        }
    }

    public void StartMove()
    {
        if (targetPosList.Count == 0)
        {
            Debug.LogError("No target pos list found!");
            return;
        }

        currentTargetPos = targetPosList[currentTargetIndex];
        isMove = true;
    }

    protected IEnumerator OnWaitNextMove()
    {
        yield return new WaitForSeconds(timeToMove);

        if (currentTargetIndex == targetPosList.Count - 1)
        {
            currentTargetIndex = 0;
        }
        else
        {
            currentTargetIndex++;
        }
        
        StartMove();
    }

    protected void CheckArrivedToTarget(Action OnArrived = null)
    {
        if (Vector3.Distance(rb.transform.position, currentTargetPos) < targetPosOffset)
        {
            isMove = false;
            OnArrived?.Invoke();
        }
    }

    protected virtual void Move()
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody is null");
            return;
        }

        rb.MovePosition(Vector3.MoveTowards(rb.transform.position, currentTargetPos, speed));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MovingObstacle))]
public class MovingObstacleGUI : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Start Move"))
        {
            MovingObstacle moveObject = (MovingObstacle)target;
            moveObject.StartMove();
        }
    }
}

#endif