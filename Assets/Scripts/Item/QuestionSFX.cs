using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionSFX : MonoBehaviour
{
    public static QuestionSFX Instance;

    [SerializeField] private AudioClip correctSfx;
    [SerializeField] private AudioClip falseSfx;
    [SerializeField] private AudioSource source;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayCorrectSFX()
    {
        source.PlayOneShot(correctSfx);
    }

    public void PlayFalseSFX()
    {
        source.PlayOneShot(falseSfx);
    }
}
