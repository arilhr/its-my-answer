using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinItemUI : MonoBehaviour
{
    public int indexSkin;
    public TMP_Text skinName;
    public TMP_Text skinPrice;
    public Button selectButton;

    private void Start()
    {
        selectButton.onClick.AddListener(() => SelectSkin());
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (InventoryManager.Instance == null) return;

        SkinInventoryData data = InventoryManager.Instance.skins[indexSkin];

        skinName.text = data.skinData.skinName;
        skinPrice.text = data.skinData.price.ToString();

        if (!data.owned) skinPrice.text = data.skinData.price.ToString();
        else skinPrice.text = "Owned";
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
            Debug.Log($"You not owned this skin!, Buy it first!");
        }
    }
}
