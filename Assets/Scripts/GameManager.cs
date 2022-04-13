using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using TMPro;
using System;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    [Scene]
    public string menuScene;

    [Header("Game Properties")]
    public int scoreToWin = 10;

    [Header("Player Spawner")]
    public GameObject playerPrefabs;
    public List<Transform> spawnPoints;

    [Header("Game Components")]
    public GameObject startGate;

    [Header("UI")]
    public UIManager uiManager;

    [Header("Timer")]
    public CustomCountdownTimer startTimer;

    private bool isGameEnd = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public override void OnEnable()
    {
        base.OnEnable();

        startTimer.OnCountdownTimerHasExpired += OnCountdownIsOver;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        startTimer.OnCountdownTimerHasExpired -= OnCountdownIsOver;
    }


    public void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerPrefabs.name, spawnPoints[0].position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefabs.name, spawnPoints[1].position, Quaternion.identity);
        }

        Hashtable props = new Hashtable
        {
            {GamePropsKey.PLAYER_LOADED, true}
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom == null) return;

        UpdateCountdownUI();
        UpdatePlayerInfoUI();
        CheckGameEnd();
    }

    #region Callbacks

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (!changedProps.ContainsKey(GamePropsKey.PLAYER_LOADED)) return;

        if (CheckAllPlayerLoaded())
        {
            startTimer.SetStartTime();
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        PhotonNetwork.LocalPlayer.SetScore(0);
        SceneManager.LoadScene(menuScene, LoadSceneMode.Single);
    }

    #endregion

    #region UI
    private void UpdateCountdownUI()
    {
        if (startTimer == null) return;

        int startTimestamp;
        bool timerIsStarting = startTimer.TryGetStartTime(out startTimestamp);

        if (timerIsStarting)
        {
            uiManager.timerUI.text = startTimer.GetTimeRemaining();
        }
    }

    private void UpdatePlayerInfoUI()
    {
        int i = 0;
        foreach (Player p in PhotonNetwork.CurrentRoom.Players.Values)
        {
            uiManager.playersUI[i].usernameText.text = p.NickName;
            uiManager.playersUI[i].scoreText.text = $"{p.GetScore()}";

            i++;
        }
    }
    #endregion

    private bool CheckAllPlayerLoaded()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object playerLoadedLevel;

            if (p.CustomProperties.TryGetValue(GamePropsKey.PLAYER_LOADED, out playerLoadedLevel))
            {
                if ((bool)playerLoadedLevel)
                {
                    continue;
                }
            }

            return false;
        }

        return true;
    }

    private void OnCountdownIsOver()
    {
        Debug.Log($"Start Game");

        uiManager.timerUI.gameObject.SetActive(false);

        startGate.SetActive(false);
    }

    public void CheckGameEnd()
    {
        if (isGameEnd) return;

        foreach (Player p in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (p.GetScore() >= scoreToWin)
            {
                GameEnd(p);
            }
        }
    }

    private void GameEnd(Player winner)
    {
        isGameEnd = true;
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject p in playerObjects)
        {
            PlayerController player = p.GetComponent<PlayerController>();

            if (player != null)
            {
                player.input.Disable();

                if (player.photonView.Owner == winner)
                {
                    CameraManager.Instance.SetObjectFollow(player.transform);
                }
            }
        }

        // set ui
        uiManager.gamePanel.SetActive(false);
        uiManager.gameEndPanel.SetActive(true);
        uiManager.winnerText.text = $"{winner.NickName} won the game!";
    }
}

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("TEST WIN P1"))
        {
            PhotonNetwork.LocalPlayer.SetScore(10);
        }
    }
}