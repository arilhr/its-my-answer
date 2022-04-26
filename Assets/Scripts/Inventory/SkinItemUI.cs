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

        SkinInventoryData data = InventoryManager.Instance.skins[indexSkin];

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

        SkinInventoryData data = InventoryManager.Instance.skins[indexSkin];

        if (data.owned)
        {
            InventoryManager.Instance.ChangeUsedSkin(indexSkin);
        }
        else
        {
            BuySkin();
        }
    }

    private void BuySkin()
    {
        if (InventoryManager.Instance == null) return;

        // TODO: check player has enough money

        InventoryManager.Instance.ChangeSkinInventoryData(indexSkin, true, false);

        Debug.Log($"Skin {skinName.text} bought");
    }
}
