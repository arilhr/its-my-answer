using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Properties")]
    public float speed = 1f;
    public float timeToMove = 1f;
    public List<Vector3> targetPosList;
    public float targetPosOffset = 0.1f;
    public bool startOnAwake = false;

    protected int currentTargetIndex = 0;
    protected Vector3 currentTargetPos;

    protected bool isMove = false;

    protected virtual void Start()
    {
        if (startOnAwake)
        {
            StartMove();
        }
        
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
        if (Vector3.Distance(transform.position, currentTargetPos) < targetPosOffset)
        {
            isMove = false;
            OnArrived?.Invoke();
        }
    }

    protected virtual void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTargetPos, speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent = transform;
        }
    }
}
