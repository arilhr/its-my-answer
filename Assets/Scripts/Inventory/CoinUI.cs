using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    public TMP_Text coinText;

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        CurrencyManager.Instance.OnCoinsChanged -= UpdateCoinUI;
    }

    private void Init()
    {
        CurrencyManager.Instance.OnCoinsChanged += UpdateCoinUI;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        coinText.text = $"{CurrencyManager.Instance.currencies.coins}";
    }
}
