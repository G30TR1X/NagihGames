using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemListSO", menuName = "ScriptableObject/Item/ItemListSO")]
public class ItemListSO : ScriptableObject
{
    public ItemSO[] itemList;
}
