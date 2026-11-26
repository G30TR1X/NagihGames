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
    public Color itemOutlineColor;

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
}
