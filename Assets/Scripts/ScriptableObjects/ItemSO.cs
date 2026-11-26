using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObject/Item/ItemSO")]
public class ItemSO : ScriptableObject
{
    public enum Rarity
    {
        Metal,
        Bronze,
        Silver,
        Gold,
        Diamond
    }

    public Rarity itemRarity;
    public Sprite itemSprite;

    public int GetItemValue()
    {
        switch (itemRarity)
        {
            case Rarity.Bronze:
                return 20;
            case Rarity.Silver:
                return 30;
            case Rarity.Gold:
                return 40;
            case Rarity.Diamond:
                return 50;
            case Rarity.Metal:
            default:
                return 10;
        }
    }

    public Color GetOutlineColor()
    {
        switch (itemRarity)
        {
            case Rarity.Bronze:
                return new Color(192, 69, 17);
            case Rarity.Silver:
                return new Color(171, 171, 171);
            case Rarity.Gold:
                return new Color(255, 177, 0);
            case Rarity.Diamond:
                return new Color(0, 43, 255);
            case Rarity.Metal:
            default:
                return new Color(48, 48, 48);
        }
    }
}
