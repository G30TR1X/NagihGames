using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageItemSingleUI : MonoBehaviour
{
    private GameUI mainGameUI;
    private Button storageItemButton;

    [SerializeField] private Outline storageItemOutline;
    [SerializeField] private Image storageItemImage;

    private ItemSO storageItem;
    public void SetStorageItem(ItemSO newStorageItem) => this.storageItem = newStorageItem;

    private void Awake()
    {
        mainGameUI = GetComponentInParent<GameUI>();
        storageItemButton = GetComponent<Button>();
    }

    private void Start()
    {
        storageItemButton.onClick.AddListener(() => mainGameUI.IncreasePlayerCoins(storageItem.GetItemValue()));
    }

    public void SetupStorageItem()
    {
        storageItemImage.sprite = storageItem.itemSprite;
        storageItemOutline.effectColor = storageItem.GetOutlineColor();
    }
}
