using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSelection : MonoBehaviour
{
    public Transform skinItemParent;
    public GameObject skinItemUI;
    public AlertPanel alertPanel;

    private void Start()
    {
        SpawnAllSkinUI();
    }

    private void SpawnAllSkinUI()
    {
        if (InventoryManager.Instance == null) return;

        int i = 0;
        foreach (SkinInventoryData data in InventoryManager.Instance.skins)
        {
            GameObject skinItemObj = Instantiate(skinItemUI, skinItemParent);
            SkinItemUI skinItem = skinItemObj.GetComponent<SkinItemUI>();
            skinItem.indexSkin = i;

            i++;
        }
    }
}
