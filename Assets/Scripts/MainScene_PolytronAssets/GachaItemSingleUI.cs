using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaItemSingleUI : MonoBehaviour
{
    private PolytronGameUI mainGameUI;
    private Button storageItemButton;

    [SerializeField] private Image storageItemOutline;

    private ItemSO storageItem;
    public void SetStorageItem(ItemSO newStorageItem) => this.storageItem = newStorageItem;

    private void Awake()
    {
        mainGameUI = GetComponentInParent<PolytronGameUI>();
        storageItemButton = GetComponent<Button>();
    }

    private void Start()
    {
        storageItemButton.onClick.AddListener(() => mainGameUI.RemoveStorageItem(storageItem));
    }

    public void SetupStorageItem()
    {
        storageItemOutline.sprite = storageItem.itemOutlineSprite;
    }
}
