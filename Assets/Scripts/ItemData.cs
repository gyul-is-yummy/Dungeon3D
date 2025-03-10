using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public enum ItemType
{
    Nature,
    Consumable,
    Equipment,
    Resource
}

public enum ContitionType
{
    Health,
    Hunger,
    Stamina
}


//ScriptableObject를 만들 때 빠르게 만들 수 있도록 에셋 생성 메뉴창에 추가
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public ItemType itemType;
    public string itemInfo;
    public Sprite itemIcon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool isStackable;
    public int maxStack;
}
