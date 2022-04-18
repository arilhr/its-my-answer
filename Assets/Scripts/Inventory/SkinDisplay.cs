using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinDisplay : MonoBehaviour
{
    public Transform skinDisplayParent;
    public GameObject currentShowedSkin;
    
    private void Start()
    {
        if (InventoryManager.Instance != null) InventoryManager.Instance.onChangedSkin += UpdateDisplay;

        UpdateDisplay();
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null) InventoryManager.Instance.onChangedSkin -= UpdateDisplay;
    }

    private void UpdateDisplay()
    {
        if (InventoryManager.Instance == null) return;
        
        if (currentShowedSkin != null)
        {
            Destroy(currentShowedSkin);
        }

        currentShowedSkin = Instantiate(InventoryManager.Instance.GetUsedSkin().modelDisplay, skinDisplayParent);

        Debug.Log($"Update skin display");
    }
}
