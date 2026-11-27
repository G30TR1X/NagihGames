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
    [SerializeField] private TextMeshProUGUI playerCoinsText;
    [SerializeField] private GameObject gameOverUI;

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
    public int playerStorageCoins { get; private set; } = 0;
    
    private int itemCost = 30;

    private void Start()
    {
        string itemValue = "Item Value:\n";
        foreach (ItemSO item in itemListSO.itemList)
            itemValue += $"{item.itemRarity} = {item.GetItemValue()} Coins\n";
        itemValueText.text = itemValue;

        UpdatePlayerCoinsVisual();

        metalPercentage = float.Parse(metalInputField.text);
        bronzePercentage = float.Parse(bronzeInputField.text);
        silverPercentage = float.Parse(silverInputField.text);
        goldPercentage = float.Parse(goldInputField.text);
        diamondPercentage = float.Parse(diamondInputField.text);

        gachaButton.onClick.AddListener(() =>
        {
            if (playerCoins < itemCost)
            {
                Debug.Log("Not enough coin");
                return;
            }

            playerCoins -= itemCost;

            ItemSO randomItem = GetItemGacha();
            storageQueue.Add(randomItem);
            storageQueue.Sort((a, b) => b.GetItemValue().CompareTo(a.GetItemValue()));
            UpdateStorageVisual();
            UpdatePlayerCoinsVisual();
        });

        metalInputField.onValueChanged.AddListener((currPercentage) =>
        {
            if (!float.TryParse(currPercentage, out metalPercentage))
            {
                metalInputField.text = "0";
                metalPercentage = 0;
            }

            if (metalPercentage < 0)
            {
                metalPercentage = 0;
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

            if (bronzePercentage < 0)
            {
                bronzePercentage = 0;
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

            if (silverPercentage < 0)
            {
                silverPercentage = 0;
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

            if (goldPercentage < 0)
            {
                goldPercentage = 0;
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

            if (diamondPercentage < 0)
            {
                diamondPercentage = 0;
                diamondInputField.text = diamondPercentage.ToString();
            }
        });

        gameOverUI.SetActive(false);
    }

    private void Update()
    {
        HandleGameOver();
    }

    private void HandleGameOver()
    {
        if (playerCoins + playerStorageCoins >= 30)
            return;

        gameOverUI.SetActive(true);
        this.enabled = false;
    }

    private ItemSO GetItemGacha()
    {
        int metalPercent = (int) FinalPercentage(metalPercentage);
        int bronzePercent = (int) FinalPercentage(bronzePercentage);
        int silverPercent = (int) FinalPercentage(silverPercentage);
        int goldPercent = (int) FinalPercentage(goldPercentage);
        int diamondPercent = (int) FinalPercentage(diamondPercentage);

        int firstTier = metalPercent;
        int secondTier = firstTier + bronzePercent;
        int thirdTier = secondTier + silverPercent;
        int fourthTier = thirdTier + goldPercent;
        int fifthTier = fourthTier + diamondPercent;

        int rng = Random.Range(0, int.MaxValue);
        rng %= fifthTier;

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
        playerStorageCoins = storageValue;
    }

    private void UpdatePlayerCoinsVisual() => playerCoinsText.text = $"Player Coins: {playerCoins.ToString()}";

    private float PercentageSum() => (metalPercentage + bronzePercentage + silverPercentage + goldPercentage + diamondPercentage);

    private float FinalPercentage(float percent)
    {
        if (PercentageSum() == 0)
        {
            Debug.Log("Invalid percentages: total cannot be zero");
            return 0;
        }

        return (percent / PercentageSum()) * 100f;
    }

    public void RemoveStorageItem(ItemSO itemSold)
    {
        playerCoins += itemSold.GetItemValue();

        storageQueue.Remove(itemSold);
        storageQueue.Sort((a, b) => b.GetItemValue().CompareTo(a.GetItemValue()));
        UpdateStorageVisual();
        UpdatePlayerCoinsVisual();
    }
}
