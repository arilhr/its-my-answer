// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CountdownTimer.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Utilities,
// </copyright>
// <summary>
// This is a basic CountdownTimer. In order to start the timer, the MasterClient can add a certain entry to the Custom Room Properties,
// which contains the property's name 'StartTime' and the actual start time describing the moment, the timer has been started.
// To have a synchronized timer, the best practice is to use PhotonNetwork.Time.
// In order to subscribe to the CountdownTimerHasExpired event you can call CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
// from Unity's OnEnable function for example. For unsubscribing simply call CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;.
// You can do this from Unity's OnDisable function for example.
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>This is a basic, network-synced CountdownTimer based on properties.</summary>
/// <remarks>
/// In order to start the timer, the MasterClient can call SetStartTime() to set the timestamp for the start.
/// The property 'StartTime' then contains the server timestamp when the timer has been started.
/// 
/// In order to subscribe to the CountdownTimerHasExpired event you can call CountdownTimer.OnCountdownTimerHasExpired
/// += OnCountdownTimerIsExpired;
/// from Unity's OnEnable function for example. For unsubscribing simply call CountdownTimer.OnCountdownTimerHasExpired
/// -= OnCountdownTimerIsExpired;.
/// 
/// You can do this from Unity's OnEnable and OnDisable functions.
/// </remarks>
public class CustomCountdownTimer : MonoBehaviourPunCallbacks
{
    public string CountdownStartTimeKey = "StartTime";

    [Header("Countdown time in seconds")]
    public float Countdown = 5.0f;

    private bool isTimerRunning;

    private int startTime;

    public Action OnCountdownTimerHasExpired;


    public void Start()
    {
        // Reference to text
    }

    public override void OnEnable()
    {
        Debug.Log("OnEnable CountdownTimer");
        base.OnEnable();

        // the starttime may already be in the props. look it up.
        Initialize();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Debug.Log("OnDisable CountdownTimer");
    }


    public void Update()
    {
        if (!this.isTimerRunning) return;

        float countdown = TimeRemaining();

        if (countdown > 0.0f) return;

        OnTimerEnds();
    }


    private void OnTimerRuns()
    {
        this.isTimerRunning = true;
    }

    private void OnTimerEnds()
    {
        OnCountdownTimerHasExpired?.Invoke();

        this.isTimerRunning = false;
    }


    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Debug.Log($"CountdownTimer.OnRoomPropertiesUpdate {propertiesThatChanged.ToStringFull()}");
        Initialize();
    }


    private void Initialize()
    {
        int propStartTime;
        if (TryGetStartTime(out propStartTime))
        {
            this.startTime = propStartTime;
            Debug.Log("Initialize sets StartTime " + this.startTime + " server time now: " + PhotonNetwork.ServerTimestamp + " remain: " + TimeRemaining());

            this.isTimerRunning = TimeRemaining() > 0;

            if (this.isTimerRunning)
                OnTimerRuns();
            else
                OnTimerEnds();
        }
    }


    private float TimeRemaining()
    {
        int timer = PhotonNetwork.ServerTimestamp - this.startTime;
        return this.Countdown - timer / 1000f;
    }

    public string GetTimeRemaining()
    {
        float countdown = TimeRemaining();
        return string.Format(countdown.ToString("n0"));
    }


    public bool TryGetStartTime(out int startTimestamp)
    {
        startTimestamp = PhotonNetwork.ServerTimestamp;

        object startTimeFromProps;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(CountdownStartTimeKey, out startTimeFromProps))
        {
            startTimestamp = (int)startTimeFromProps;
            return true;
        }

        return false;
    }


    public void SetStartTime()
    {
        int startTime = 0;
        bool wasSet = TryGetStartTime(out startTime);

        Hashtable props = new Hashtable
        {
            {CountdownStartTimeKey, (int)PhotonNetwork.ServerTimestamp}
        };

        PhotonNetwork.CurrentRoom.SetCustomProperties(props);

        Debug.Log($"Set Custom Props for {CountdownStartTimeKey}: " + props.ToStringFull() + " wasSet: " + wasSet);
    }
}