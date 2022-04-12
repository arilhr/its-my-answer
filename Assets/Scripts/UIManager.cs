using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [Serializable]
    public struct PlayerUI
    {
        public TMP_Text usernameText;
        public TMP_Text scoreText;
    }

    [Header("Game UI")]
    public GameObject gamePanel;
    public TMP_Text timerUI;
    public List<PlayerUI> playersUI;

    [Header("Game End UI")]
    public GameObject gameEndPanel;
    public TMP_Text winnerText;
    public Button backButton;
}
