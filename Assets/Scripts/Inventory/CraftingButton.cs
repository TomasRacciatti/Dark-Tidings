using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Items;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class CraftingButton : MonoBehaviour
{
    [SerializeField] private SO_Item soItem;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Image image;
    [SerializeField] private CraftingManager craftingManager;

    private void Awake()
    {
        textMesh.text = soItem.ItemName;
        image.sprite = soItem.Image;
    }

    public void Click()
    {
        craftingManager.SetItem(new ItemAmount(soItem,1));
    }
}
