using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonDestroy<CameraManager>
{
    public Camera mainCamera;
    public CinemachineFreeLook cinemachineCamera;

    public void SetObjectFollow(Transform objToFollow)
    {
        cinemachineCamera.Follow = objToFollow;
        cinemachineCamera.LookAt = objToFollow;
    }
}
