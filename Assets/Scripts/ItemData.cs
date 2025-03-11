using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Nature,
    Consumable,
    Equipable,
    Resource
}

public enum ConditionType
{
    Health,
    Hunger,
    Stamina
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConditionType type;     //������ ȸ����Ű�� �Һ� ����������
    public float value;             //�󸶸�ŭ ȸ������ �ִ���
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

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;    //�Һ� �������� ������ ��� �迭
}
