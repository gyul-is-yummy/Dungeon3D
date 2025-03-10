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


//ScriptableObject�� ���� �� ������ ���� �� �ֵ��� ���� ���� �޴�â�� �߰�
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
