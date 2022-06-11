using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip jumpAudio;

    [Header("References")]
    [SerializeField] private AudioSource audioSource;

    public void PlayJumpAudio()
    {
        audioSource.PlayOneShot(jumpAudio);
    }
}
