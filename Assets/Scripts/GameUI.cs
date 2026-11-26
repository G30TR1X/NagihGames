using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private ItemListSO itemListSO;

    /* Storage UI Settings */
    [SerializeField] private GameObject itemStorageTemplate;

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

    public int playerCoins { get; private set; } = 200;

    private void Start()
    {
        metalInputField.onValueChanged.AddListener((currPercentage) =>
        {
            if (!float.TryParse(currPercentage, out metalPercentage))
            {
                metalInputField.text = "0";
                metalPercentage = 0;
            }
        });

        bronzeInputField.onValueChanged.AddListener((currPercentage) =>
        {
            if (!float.TryParse(currPercentage, out bronzePercentage))
            {
                bronzeInputField.text = "0";
                bronzePercentage = 0;
            }

            bronzePercentage = float.Parse(bronzeInputField.text);
        });

        silverInputField.onValueChanged.AddListener((currPercentage) =>
        {
            if (!float.TryParse(currPercentage, out silverPercentage))
            {
                silverInputField.text = "0";
                silverPercentage = 0;
            }
        });

        goldInputField.onValueChanged.AddListener((currPercentage) =>
        {
            if (!float.TryParse(currPercentage, out goldPercentage))
            {
                goldInputField.text = "0";
                goldPercentage = 0;
            }
        });

        diamondInputField.onValueChanged.AddListener((currPercentage) =>
        {
            if (!float.TryParse(currPercentage, out diamondPercentage))
            {
                diamondInputField.text = "0";
                diamondPercentage = 0;
            }
        });
    }
}
