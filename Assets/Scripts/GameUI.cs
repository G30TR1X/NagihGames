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
    private List<ItemSO> storageQueue = new();

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

        metalPercentage = float.Parse(metalInputField.text);
        bronzePercentage = float.Parse(bronzeInputField.text);
        silverPercentage = float.Parse(silverInputField.text);
        goldPercentage = float.Parse(goldInputField.text);
        diamondPercentage = float.Parse(diamondInputField.text);

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
            {
                metalPercentage = maxPercentage - (PercentageSum() - metalPercentage);
                metalInputField.text = metalPercentage.ToString();
            }
        });

        bronzeInputField.onValueChanged.AddListener((currPercentage) =>
        {
            if (!float.TryParse(currPercentage, out bronzePercentage))
            {
                bronzeInputField.text = "0";
                bronzePercentage = 0;
            }

            if (MaxPercentageOver())
            {
                bronzePercentage = maxPercentage - (PercentageSum() - bronzePercentage);
                bronzeInputField.text = bronzePercentage.ToString();
            }
        });

        silverInputField.onValueChanged.AddListener((currPercentage) =>
        {
            if (!float.TryParse(currPercentage, out silverPercentage))
            {
                silverInputField.text = "0";
                silverPercentage = 0;
            }

            if (MaxPercentageOver())
            {
                silverPercentage = maxPercentage - (PercentageSum() - silverPercentage);
                silverInputField.text = silverPercentage.ToString();
            }
        });

        goldInputField.onValueChanged.AddListener((currPercentage) =>
        {
            if (!float.TryParse(currPercentage, out goldPercentage))
            {
                goldInputField.text = "0";
                goldPercentage = 0;
            }

            if (MaxPercentageOver())
            {
                goldPercentage = maxPercentage - (PercentageSum() - goldPercentage);
                goldInputField.text = goldPercentage.ToString();
            }
        });

        diamondInputField.onValueChanged.AddListener((currPercentage) =>
        {
            if (!float.TryParse(currPercentage, out diamondPercentage))
            {
                diamondInputField.text = "0";
                diamondPercentage = 0;
            }

            if (MaxPercentageOver())
            {
                diamondPercentage = maxPercentage - (PercentageSum() - diamondPercentage);
                diamondInputField.text = diamondPercentage.ToString();
            }
        });
    }

    private ItemSO GetItemGacha()
    {
        int rng = Random.Range(0, int.MaxValue);
        rng %= (int) maxPercentage;

        int firstTier = (int) metalPercentage, secondTier = firstTier + (int) bronzePercentage, thirdTier = secondTier + (int) silverPercentage, fourthTier = thirdTier + (int) goldPercentage, fifthTier = fourthTier + (int) diamondPercentage;

        int chosenRarity = 0;
        if (rng >= 0 && rng <= firstTier)
            chosenRarity = 0;
        if (rng > firstTier && rng <= secondTier)
            chosenRarity = 1;
        if (rng > secondTier && rng <= thirdTier)
            chosenRarity = 2;
        if (rng > thirdTier && rng <= fourthTier)
            chosenRarity = 3;
        if (rng > fourthTier && rng <= Mathf.Min(fifthTier, maxPercentage))
            chosenRarity = 4;

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
            storageValue += item.GetItemValue();
        }

        storageValueText.text = $"Storage Value: {storageValue}";
    }

    private float PercentageSum() => (metalPercentage + bronzePercentage + silverPercentage + goldPercentage + diamondPercentage);

    private bool MaxPercentageOver() => PercentageSum() > maxPercentage;

    public void RemoveStorageItem(ItemSO itemSold)
    {
        playerCoins += itemSold.GetItemValue();

        storageQueue.Remove(itemSold);
        UpdateStorageVisual();
    }
}
