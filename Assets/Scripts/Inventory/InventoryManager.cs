using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SkinInventoryData
{
    public Skin skinData;
    public bool owned;
    public bool used;
}

public class InventoryManager : MonoBehaviour
{
    public const string SKIN_FILE_NAME = "skins";

    public static InventoryManager Instance;

    [Header("Skin")]
    public List<SkinInventoryData> skins;
    public Skin defaultSkin;

    public Action onChangedSkin;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeInventory();
        LoadSkins();
    }

    private void InitializeInventory()
    {
        // set starting skin data
        for (int i = 0; i < skins.Count; i++)
        {
            ChangeSkinInventoryData(i, false, false);
        }

        // change owned and used in default skin
        int indexDefaultSkin = skins.FindIndex(x => x.skinData == defaultSkin);
        ChangeSkinInventoryData(indexDefaultSkin, true, false);
        ChangeUsedSkin(indexDefaultSkin);
    }

    private void LoadSkins()
    {
        // load data
        List<SkinInventoryData> loadedSkins = new List<SkinInventoryData>();
        SaveSystem.ReadJsonFile(SKIN_FILE_NAME, out loadedSkins);

        // if there is no saved data, then load starting data
        if (loadedSkins.Count == 0)
        {
            Debug.Log($"No saved file.");
            return;
        }

        foreach (SkinInventoryData loadedSkin in loadedSkins)
        {
            if (skins.Exists(x => x.skinData == loadedSkin.skinData))
            {
                int indexToChange = skins.FindIndex(x => x.skinData == loadedSkin.skinData);
                ChangeSkinInventoryData(indexToChange, loadedSkin.owned, loadedSkin.used);
            }
        }
    }

    public Skin GetUsedSkin()
    {
        return skins.Find(x => x.used).skinData;
    }
    
    public void ChangeUsedSkin(Skin skinToUsed)
    {
        // unused previous skin
        if (skins.Exists(x => x.used))
        {
            int indexPrevUsed = skins.FindIndex(x => x.used);
            ChangeSkinInventoryData(indexPrevUsed, skins[indexPrevUsed].owned, false);
        }

        // search skin to used, and used it
        int indexCurrentUsed = skins.FindIndex(x => x.used);
        ChangeSkinInventoryData(indexCurrentUsed, skins[indexCurrentUsed].owned, true);

        onChangedSkin?.Invoke();

        Debug.Log($"Skin changed to {skinToUsed.skinName}");
    }

    public void ChangeUsedSkin(int indexSkin)
    {

        // unused previous skin
        if (skins.Exists(x => x.used))
        {
            int indexPrevUsed = skins.FindIndex(x => x.used);

            if (indexPrevUsed == indexSkin)
            {
                Debug.Log($"Skin {skins[indexPrevUsed].skinData.skinName} is already used.");
                return;
            }

            ChangeSkinInventoryData(indexPrevUsed, skins[indexPrevUsed].owned, false);
        }

        ChangeSkinInventoryData(indexSkin, skins[indexSkin].owned, true);

        onChangedSkin?.Invoke();

        Debug.Log($"Skin changed to {skins[indexSkin].skinData.skinName}");
    }

    private void ChangeSkinInventoryData(int indexSkin, bool owned, bool used)
    {
        SkinInventoryData updatedData = skins[indexSkin];

        updatedData.owned = owned;
        updatedData.used = used;

        skins[indexSkin] = updatedData;
    }

}

