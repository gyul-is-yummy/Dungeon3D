using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;        //������ ���Կ� ���� ������

    public GameObject inventoryWindow;  //�κ��丮 â
    public Transform slotPanel;         //������ ����â ��ġ
    public Transform dropPosition;      //�������� ���� �� ����߸� ��ġ

    public GameObject infoPanel;         //������ ����â

    [Header("Select Item")]
    public Image selectedItemImage;                //������ ������ �̹���
    public TextMeshProUGUI selectedItemName;            //������ ������ �̸�
    public TextMeshProUGUI selectedItemDescription;     //������ ������ ����
    public TextMeshProUGUI selectedItemStatName;        //������ ������ ���� �̸�
    public TextMeshProUGUI selectedItemStatValue;       //������ ������ ���� ��
    public GameObject useButton;                        //��� ��ư
    public GameObject dropButton;                       //������ ��ư

    private PlayerController controller;     //������ �ְ���� �÷��̾� ��Ʈ�ѷ�
    private PlayerCondition condition;       //������ �ְ���� �÷��̾� �����

    private ItemData selectedItem;           //������ ������ 
    private int selectedItemIndex = 0;       //������ �������� �ε���

    private int curEquipIndex;               //���� �����ϰ� �ִ� �������� �ε���

    private Player player;

    private void OnValidate()
    {
        inventoryWindow = this.gameObject;
        slotPanel = Utill.FindChildRecursive(transform, "Slots");
        infoPanel = Utill.FindChildRecursive(transform, "ItemInfoBG").gameObject;

        //Ȥ�� �𸣴ϱ� �� �� �״ٰ� ������ �� (��ä�� �����ϸ� start�� �� ���ư�)
        inventoryWindow.SetActive(true);

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

    private void Start()
    {
        //�����̳� �ڽ��� �ƴ� ��ü�� OnValidate�� ���� ������ �� �ִٰ� �ؼ� Start�� ����
        player = CharacterManager.Instance.Player;
        controller = player.controller;
        condition = player.condition;
        dropPosition = player.transform;

        controller.inventory += Toggle;
        controller.inventory += ()=> Debug.Log("Inventory Opened");
        player.addItem += AddItem;

        //ó���� �κ��丮�� ���̸� �ȵǹǷ� ���� ���·� ����
        ClearSelectedItemWindow();
        inventoryWindow.SetActive(false); 
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
        //���� ������ ���Կ� �������� ���ٸ� ����â�� ���ְ�, ����������.
        if (slots[index].item == null)
        {
            //���� �ǳ��� ����.
            infoPanel.SetActive(false);

            return;
        }

        selectedItem = slots[index].item;                              
        selectedItemIndex = index;

        //selectedItemImage.sprite = selectedItem.itemIcon;
        selectedItemName.text = selectedItem.itemName;             
        selectedItemDescription.text = selectedItem.itemInfo; 

        //��� �����ۿ� ������ �ִ� ���� �ƴϹǷ� Empty�� �ʱ�ȭ
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        //consumables Length�� ������ ��쿡�� �ݺ����� ������ (= �Һ� �������� ���� ����)
        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedItemStatName.text += selectedItem.consumables[i].type.ToString() + "\n";   
            selectedItemStatValue.text += selectedItem.consumables[i].value.ToString() + "\n"; 
        }

        //���Կ� ����ִ� ������ Ÿ���� Consumable�� ���� ���δ�.
        useButton.SetActive(selectedItem.itemType == ItemType.Consumable); 

        //������ ��ư�� ������ ���̵��� ����
        dropButton.SetActive(true);

        //�ڽ� ������Ʈ���� ������ �����ٸ� ����â�� ���ش�.
        infoPanel.SetActive(true);
    }

    public void OnUseButton()
    {
        //���� ������ ������ Ÿ���� �Һ� �������̶�� (Consumable�� ���� UseButton�� Ȱ��ȭ��)
        if (selectedItem.itemType == ItemType.Consumable)             
        {
            //ȸ�������ִ� ������ �� ��ŭ �ݺ����� ���ư���.
            for (int i = 0; i < selectedItem.consumables.Length; i++)
            {
                switch (selectedItem.consumables[i].type)
                {
                    case ConditionType.Health:
                        condition.Heal(selectedItem.consumables[i].value);
                        break;
                    case ConditionType.Hunger:
                        condition.Eat(selectedItem.consumables[i].value);
                        break;
                }
            }
            //�κ��丮���� �������� �����Ѵ�.
            RemoveSelctedItem();
        }
    }

    public void OnDropBotton()
    {
        //�������� ����߸���.
        ThrowItem(selectedItem);
        //�κ��丮���� �������� �����Ѵ�.
        RemoveSelctedItem();
    }

    void RemoveSelctedItem() 
    {
        //������ �������� ������ 1 ���ҽ�Ų��.
        slots[selectedItemIndex].quantity--;

        //���� -1�� ������ �� ������ 0�� �ǰų� ������ �ȴٸ�
        if (slots[selectedItemIndex].quantity <= 0)
        {
            //������ ������ �ʱ�ȭ
            selectedItem = null;
            //������ ������ �ʱ�ȭ
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

        //������� ����
        UpdateUI();
    }

    //�������� Ŭ������ �� ǥ�õǴ� ����(UI)���� �ʱ�ȭ ���ִ� �޼���
    void ClearSelectedItemWindow()
    {
        selectedItem = null;

        selectedItemImage = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        dropButton.SetActive(false);
        
        //���� �ǳ��� ����.
        infoPanel.SetActive(false);
    }


}