using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;        //������ ���Կ� ���� ������

    public GameObject inventoryWindow;  //�κ��丮 â
    public Transform slotPanel;         //���� �ǳ�?
    public Transform dropPosition;      //�������� ���� �� ����߸� ��ġ

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;            //������ ������ �̸�
    public TextMeshProUGUI selectedItemDescription;     //������ ������ ����
    public TextMeshProUGUI selectedItemStatName;        //������ ������ ���� �̸�
    public TextMeshProUGUI selectedItemStatValue;       //������ ������ ���� ��
    public GameObject useButton;                        //��� ��ư
    public GameObject equipButton;                      //���� ��ư
    public GameObject unEquipButton;                    //���� ��ư
    public GameObject dropButton;                       //������ ��ư

    private PlayerController controller;     //������ �ְ���� �÷��̾� ��Ʈ�ѷ�
    private PlayerCondition condition;       //������ �ְ���� �÷��̾� �����

    private ItemData selectedItem;           //������ ������ 
    private int selectedItemIndex = 0;       //������ �������� �ε���

    private int curEquipIndex;               //���� �����ϰ� �ִ� �������� �ε���

    private Player player;

    private void OnValidate()
    {
        //inventoryWindow = GameObject.Find("UIInventory");
        inventoryWindow = this.gameObject;
        //slotPanel = GameObject.Find("Slots").transform;
        slotPanel = Utill.FindChildRecursive(transform, "Slots");
        //inventoryWindow.SetActive(false);
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        controller = player.GetComponent<PlayerController>();
        condition = player.GetComponent<PlayerCondition>();
        dropPosition = player.transform;

        controller.inventory += Toggle;
        controller.inventory += ()=> Debug.Log("Inventory Opened");
        player.addItem += AddItem;

        //ó���� �κ��丮�� ���̸� �ȵǹǷ� ���� ���·� ����
        inventoryWindow.SetActive(false);

        //������ ������ ���� �ǳ��� �ڽ� ������Ʈ�� �� ����
        //childCount�� ���� ���� �ǳ��� �ڽ� ������Ʈ ������ �޾ƿ� �� �׸�ŭ ������ ������ش�.
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }
    }

    //�κ��丮�� TabŰ�� ������ ���� ���� �� �ֵ��� �Ѵ�.
    //���������� �ݰ�, ���������� ������ �Ѵ�.
    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }

    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem()
    {
        ItemData data = player.itemData;

        //�ߺ����� ���� �� �ִ� ���������� Ȯ��
        if (data.isStackable)
        {
            ItemSlot slot = GetItemStack(data);
            //���� �ش� �������� �̹� �ϳ��� ������ �ִٸ�
            if (slot != null)
            {
                //�������� ����(quantity)�� 1 ������Ų��.
                slot.quantity++;
                UpdateUI();
                player.itemData = null;
                return;
            }
        }

        //����ִ� ������ �����´�.
        ItemSlot emptySlot = GetEmptySlot();

        //���� ����ִ� ������ ���ٸ�
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            player.itemData = null;
            return;
        }

        ThrowItem(data);
        player.itemData = null;
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            //���Կ� �̹� �����Ͱ� �� �ִٸ�
            if (slots[i].item != null)
            {
                //���Կ� �������� ���������
                slots[i].Set();
            }
            //���Կ� �����Ͱ� ���ٸ�
            else
            {
                //������ �����
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            //���� �ȿ� �ִ� �������� data�� ����, ������ �ִ� ���� �������� �۴ٸ�
            if (slots[i].item == data && slots[i].quantity < data.maxStack)
            {
                //�ش� ������ ��ȯ
                return slots[i];
            }
        }
        //�׷��� �ʴٸ� null�� ��ȯ
        return null;
    }

    //����ִ� ������ ã�Ƽ� ��ȯ�ϴ� �޼���
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        //��� ������ ���ִٸ� null�� ��ȯ
        return null;
    }

    public void ThrowItem(ItemData data)
    {
        //�ν��Ͻ�ȭ ��Ų��.
        //�ν��Ͻ�ȭ: �������� ������ ���� �� ���� ��
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }


    //�κ��丮���� �������� �������� �� ����Ǵ� �޼���
    //������ �������� ������ UI�� ǥ���Ѵ�.
    public void SelectItem(int index)
    {
        //���� ������ ���Կ� �������� ���ٸ� �ƹ��͵� ���� �ʴ´�.
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;                              
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.itemName;             
        selectedItemDescription.text = selectedItem.itemInfo; 

        //��� �����ۿ� ������ �ִ� ���� �ƴϹǷ� Empty�� �ʱ�ȭ
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        //consumables Length�� ������ ��쿡�� �ݺ����� ������ (= �Һ� �������� ���� ����)
        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedItemStatName.text += selectedItem.consumables[i].type.ToString() + "\n";   //-item
            selectedItemStatValue.text += selectedItem.consumables[i].value.ToString() + "\n"; //-item
        }

        //���Կ� ����ִ� ������ Ÿ���� Consumable�� ���� ���δ�.
        useButton.SetActive(selectedItem.itemType == ItemType.Consumable); //-item

        //���Կ� ����ִ� ������ Ÿ���� Equipable�̰�, �ش� �������� �����Ǿ� ���� ���� ���� ���δ�.
        equipButton.SetActive(selectedItem.itemType == ItemType.Equipable && !slots[index].equipped);  //-item

        //���Կ� ����ִ� ������ Ÿ���� Equipable�̰�, �ش� �������� �����Ǿ� ���� ���� ���δ�.
        unEquipButton.SetActive(selectedItem.itemType == ItemType.Equipable && slots[index].equipped); //-item

        //������ ��ư�� ������ ���̵��� ����
        dropButton.SetActive(true);
    }

}