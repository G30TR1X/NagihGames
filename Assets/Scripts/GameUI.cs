using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private ItemListSO itemListSO;
    [SerializeField] private Button gachaButton;
    [SerializeField] private TextMeshProUGUI itemValueText;

    /* Storage UI Settings */
    [SerializeField] private TextMeshProUGUI storageValueText;
    [SerializeField] private Transform itemStorageContainer;
    [SerializeField] private Transform itemStorageTemplate;
    private List<ItemSO> storageQueue;

    /* Rarity Gacha Percentage Input Field */
    [SerializeField] private TMP_InputField metalInputField;
    private float metalPercentage;
    [SerializeField] private TMP_InputField bronzeInputField;
    private float bronzePercentage;
    [SerializeField] private TMP_InputField silverInputField;
    private float silverPercentage;
    [SerializeField] private TMP_InputField goldInputField;
    private float goldPercentage;
    [SerializeField] private TMP_InputField diamondInputField;
    private float diamondPercentage;
    private float maxPercentage = 100f;


    public int playerCoins { get; private set; } = 200;
    
    private int itemCost = 30;

    private void Start()
    {
        string itemValue = "Item Value:\n";
        foreach (ItemSO item in itemListSO.itemList)
            itemValue += $"{item.itemRarity} = {item.GetItemValue()} Coins\n";
        itemValueText.text = itemValue;

        gachaButton.onClick.AddListener(() =>
        {
            if (playerCoins < itemCost)
                return;

            playerCoins -= itemCost;

            ItemSO randomItem = GetItemGacha();
            storageQueue.Add(randomItem);
            UpdateStorageVisual();
        });

        metalInputField.onValueChanged.AddListener((currPercentage) =>
        {
            if (!float.TryParse(currPercentage, out metalPercentage))
            {
                metalInputField.text = "0";
                metalPercentage = 0;
            }

            if (MaxPercentageOver())
                metalPercentage = maxPercentage - PercentageSum;
        });

        bronzeInputField.onValueChanged.AddListener((currPercentage) =>
        {
            if (!float.TryParse(currPercentage, out bronzePercentage))
            {
                bronzeInputField.text = "0";
                bronzePercentage = 0;
            }

            if (MaxPercentageOver())
                bronzePercentage = maxPercentage - PercentageSum;
        });

        silverInputField.onValueChanged.AddListener((currPercentage) =>
        {
            if (!float.TryParse(currPercentage, out silverPercentage))
            {
                silverInputField.text = "0";
                silverPercentage = 0;
            }

            if (MaxPercentageOver())
                silverPercentage = maxPercentage - PercentageSum;
        });

        goldInputField.onValueChanged.AddListener((currPercentage) =>
        {
            if (!float.TryParse(currPercentage, out goldPercentage))
            {
                goldInputField.text = "0";
                goldPercentage = 0;
            }

            if (MaxPercentageOver())
                goldPercentage = maxPercentage - PercentageSum;
        });

        diamondInputField.onValueChanged.AddListener((currPercentage) =>
        {
            if (!float.TryParse(currPercentage, out diamondPercentage))
            {
                diamondInputField.text = "0";
                diamondPercentage = 0;
            }

            if (MaxPercentageOver())
                diamondPercentage = maxPercentage - PercentageSum;
        });
    }

    private float PercentageSum() => (metalPercentage + bronzePercentage + silverPercentage + goldPercentage + diamondPercentage);
    private bool MaxPercentageOver() => PercentageSum() > maxPercentage

    private ItemSO GetItemGacha()
    {
        int rng = Random.Range(0, int.MaxValue);
        rng %= (int) maxPercentage;

        int firstTier = metalPercentage, secondTier = firstTier + bronzePercentage, thirdTier = secondTier + silverPercentage, fourthTier = thirdTier + goldPercentage;

        ItemSO.Rarity chosenRarity;
        if (rng >= 0 && rng <= firstTier)
            chosenRarity = ItemSO.Rarity.Metal;
        if (rng > firstTier && rng <= secondTier)
            chosenRarity = ItemSO.Rarity.Bronze;
        if (rng > secondTier && rng <= thirdTier)
            chosenRarity = ItemSO.Rarity.Silver;
        if (rng > thirdTier && rng <= fourthTier)
            chosenRarity = ItemSO.Rarity.Gold;
        if (rng > fourthTier && rng <= maxPercentage)
            chosenRarity = ItemSO.Rarity.Diamond;

        ItemSO randomItem = itemListSO.itemList[chosenRarity];
        return randomItem;
    }

    private void UpdateStorageVisual()
    {
        foreach (Transform child in itemStorageContainer)
            Destroy(child.gameObject);

        int storageValue = 0;
        foreach (ItemSO item in storageQueue)
        {
            Transform newStorageItem = Instantiate(itemStorageTemplate, itemStorageContainer);
            StorageItemSingleUI storageItemScript = newStorageItem.GetComponent<StorageItemSingleUI>();
            storageItemScript.SetStorageItem(item);
            storageItemScript.SetupStorageItem();
        }

        storageValueText.text = $"Storage Value: {storageValue}";
    }

    public void IncreasePlayerCoins(int amount) => playerCoins += amount;
}
