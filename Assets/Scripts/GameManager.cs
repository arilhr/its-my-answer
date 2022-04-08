using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Player Spawner")]
    public GameObject playerPrefabs;
    public List<Transform> spawnPoints;

    [Header("Game Components")]
    public GameObject startGate;

    [Header("UI")]
    public TMP_Text timerUI;

    [Header("Timer")]
    public CustomCountdownTimer startTimer;

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
        int startTimestamp;
        bool timerIsStarting = startTimer.TryGetStartTime(out startTimestamp);

        if (timerIsStarting)
        {
            timerUI.text = startTimer.GetTimeRemaining();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (CheckAllPlayerLoaded())
        {
            startTimer.SetStartTime();
        }
    }

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

        timerUI.gameObject.SetActive(false);

        startGate.SetActive(false);
    }
}
