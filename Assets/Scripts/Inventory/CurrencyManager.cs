using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct Currencies
{
    public int coins;
}


public class CurrencyManager : MonoBehaviour
{
    private readonly string CurrencySaveFile = "currency_save.json";

    public static CurrencyManager Instance;

    public Currencies currencies;

    public Action OnCoinsChanged; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

            OnCoinsChanged += SaveCoin;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        OnCoinsChanged -= SaveCoin;
    }

    private void Start()
    {
        LoadCoin();
    }

    public void AddCoins(int amount)
    {
        currencies.coins += amount;

        OnCoinsChanged?.Invoke();
    }

    public void SubtractCoins(int amount)
    {
        if (currencies.coins == 0) return;
        
        if (currencies.coins < amount)
        {
            currencies.coins = 0;
        }
        else
        {
            currencies.coins -= amount;
        }

        OnCoinsChanged?.Invoke();
    }
    
    public void LoadCoin()
    {
        SaveSystem.ReadJsonFile(CurrencySaveFile, out currencies);

        OnCoinsChanged?.Invoke();
    }

    public void SaveCoin()
    {
        SaveSystem.SaveToJson(CurrencySaveFile, currencies);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(CurrencyManager))]
public class CurrencyManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CurrencyManager coinManager = (CurrencyManager)target;

        if (GUILayout.Button("Add Coins"))
        {
            coinManager.AddCoins(10);
        }

        if (GUILayout.Button("Subtract Coins"))
        {
            coinManager.SubtractCoins(10);
        }
    }
}

#endif