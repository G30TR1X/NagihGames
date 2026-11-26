using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageItemSingleUI : MonoBehaviour
{
    private GameUI mainGameUI;
    private Button storageItemButton;

    [SerializeField] private Image storageItemOutline;
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
        storageItemButton.onClick.AddListener(() => mainGameUI.RemoveStorageItem(storageItem));
    }

    public void SetupStorageItem()
    {
        storageItemImage.sprite = storageItem.itemSprite;
        storageItemOutline.color = storageItem.GetOutlineColor();
    }
}
