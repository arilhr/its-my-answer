using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinItemUI : MonoBehaviour
{
    public int indexSkin;
    public TMP_Text skinName;
    public Image skinImage;
    public TMP_Text skinPrice;
    public Button selectButton;

    SkinInventoryData data;

    private void Start()
    {
        UpdateUI();

        selectButton.onClick.AddListener(() => SelectSkin());
    }

    private void OnEnable()
    {
        if (InventoryManager.Instance != null) InventoryManager.Instance.onSkinDataChanged += UpdateUI;
    }

    private void OnDisable()
    {
        InventoryManager.Instance.onSkinDataChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        if (InventoryManager.Instance == null) return;
        
        data = InventoryManager.Instance.skins[indexSkin];
        
        skinName.text = data.skinData.skinName;
        skinImage.sprite = data.skinData.modelSprite != null? data.skinData.modelSprite : null;
        

        if (!data.owned)
        {
            skinPrice.text = data.skinData.price.ToString();
        }
        else
        {
            if (data.used)
                skinPrice.text = "Used";
            else
                skinPrice.text = "Owned";
        }
    }

    private void SelectSkin()
    {
        if (InventoryManager.Instance == null) return;

        if (data.owned)
        {
            InventoryManager.Instance.ChangeUsedSkin(indexSkin);
        }
        else
        {
            SkinSelection skinSelectUI = GetComponentInParent<SkinSelection>();
            AlertPanel alert = skinSelectUI.alertPanel;

            // not enough coins
            if (CurrencyManager.Instance.currencies.coins < data.skinData.price)
            {
                alert.ShowAlert(AlertType.INFO, "Not enough coins");
                return;
            }

            // show buy skin confirmation
            alert.ShowAlert(AlertType.CONFIRMATION, $"Are you sure you want to buy skin {data.skinData.name}?", () =>
            {
                BuySkin();
            }, null);
        }
    }

    private void BuySkin()
    {
        if (InventoryManager.Instance == null) return;

        // subtract coins
        CurrencyManager.Instance.SubtractCoins(data.skinData.price);

        // change skin data that bough
        InventoryManager.Instance.ChangeSkinInventoryData(indexSkin, true, false);

        Debug.Log($"Skin {skinName.text} bought");
    }
}
