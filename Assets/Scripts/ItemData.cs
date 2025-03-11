using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public enum ItemType
{
    Nature,
    Consumable,
    Equipable,
    Resource
}

public enum ContitionType
{
    Health,
    Hunger,
    Stamina
}

[System.Serializable]
public class ItemDataConsumable
{
    public ContitionType type;     //무엇을 회복시키는 소비 아이템인지
    public float value;             //얼마만큼 회복시켜 주는지
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

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;    //소비 아이템의 정보를 담는 배열
}
