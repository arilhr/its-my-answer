using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "Inventory/New Skin")]
public class Skin : ScriptableObject
{
    public string skinName;

    [Tooltip("Model displayed in inventory")]
    public GameObject modelDisplay;

    [Tooltip("Image model")]
    public Sprite modelSprite;

    [Tooltip("Model used in game")]
    public GameObject modelInGame;

    [Tooltip("Player owned this skin?")]
    public bool isFree;

    [Tooltip("Price of skin")]
    public int price;
}
