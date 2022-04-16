using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AccountManager : Singleton<AccountManager>
{
    // Load Data Key
    public const string UNAME_KEY = "Username";

    public string username;

    private void Start()
    {
        LoadData();
    }

    private void LoadData()
    {
        // load username
        username = PlayerPrefs.GetString(UNAME_KEY);
        if (username == string.Empty)
        {
            SetUsername($"Player {Random.Range(0, 1000)}");
        }
        else
        {
            PhotonNetwork.NickName = username;
        }
    }

    public void SetUsername(string _uname)
    {
        username = _uname;
        PlayerPrefs.SetString(UNAME_KEY, username);
        PhotonNetwork.NickName = username;
    }
}
