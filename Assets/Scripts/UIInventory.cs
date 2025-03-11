using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;        //아이템 슬롯에 대한 정보들

    public GameObject inventoryWindow;  //인벤토리 창
    public Transform slotPanel;         //슬롯 판넬?
    public Transform dropPosition;      //아이템을 버릴 때 떨어뜨릴 위치

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;            //선택한 아이템 이름
    public TextMeshProUGUI selectedItemDescription;     //선택한 아이템 설명
    public TextMeshProUGUI selectedItemStatName;        //선택한 아이템 스텟 이름
    public TextMeshProUGUI selectedItemStatValue;       //선택한 아이템 스텟 값
    public GameObject useButton;                        //사용 버튼
    public GameObject equipButton;                      //장착 버튼
    public GameObject unEquipButton;                    //해제 버튼
    public GameObject dropButton;                       //버리기 버튼

    private PlayerController controller;     //정보를 주고받을 플레이어 컨트롤러
    private PlayerCondition condition;       //정보를 주고받을 플레이어 컨디션

    private ItemData selectedItem;           //선택한 아이템 
    private int selectedItemIndex = 0;       //선택한 아이템의 인덱스

    private int curEquipIndex;               //현재 장착하고 있는 아이템의 인덱스

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

        //처음에 인벤토리가 보이면 안되므로 꺼진 상태로 시작
        inventoryWindow.SetActive(false);

        //아이템 슬롯은 슬롯 판넬의 자식 오브젝트로 들어가 있음
        //childCount을 통해 슬롯 판넬의 자식 오브젝트 개수를 받아온 뒤 그만큼 슬롯을 만들어준다.
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }
    }

    //인벤토리는 Tab키를 눌러서 열고 닫을 수 있도록 한다.
    //열려있으면 닫고, 닫혀있으면 열도록 한다.
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

        //중복으로 가질 수 있는 아이템인지 확인
        if (data.isStackable)
        {
            ItemSlot slot = GetItemStack(data);
            //만약 해당 아이템을 이미 하나라도 가지고 있다면
            if (slot != null)
            {
                //아이템의 수량(quantity)만 1 증가시킨다.
                slot.quantity++;
                UpdateUI();
                player.itemData = null;
                return;
            }
        }

        //비어있는 슬롯을 가져온다.
        ItemSlot emptySlot = GetEmptySlot();

        //만약 비어있는 슬롯이 없다면
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
            //슬롯에 이미 데이터가 들어가 있다면
            if (slots[i].item != null)
            {
                //슬롯에 아이템을 세팅해줘라
                slots[i].Set();
            }
            //슬롯에 데이터가 없다면
            else
            {
                //슬롯을 비워라
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            //슬롯 안에 있는 아이템이 data와 같고, 수량이 최대 스택 수량보다 작다면
            if (slots[i].item == data && slots[i].quantity < data.maxStack)
            {
                //해당 슬롯을 반환
                return slots[i];
            }
        }
        //그렇지 않다면 null을 반환
        return null;
    }

    //비어있는 슬롯을 찾아서 반환하는 메서드
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        //모든 슬롯이 차있다면 null을 반환
        return null;
    }

    public void ThrowItem(ItemData data)
    {
        //인스턴스화 시킨다.
        //인스턴스화: 프리팹을 실제로 게임 상에 띄우는 것
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }


    //인벤토리에서 아이템을 선택했을 때 실행되는 메서드
    //선택한 아이템의 정보를 UI에 표시한다.
    public void SelectItem(int index)
    {
        //만약 선택한 슬롯에 아이템이 없다면 아무것도 하지 않는다.
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;                              
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.itemName;             
        selectedItemDescription.text = selectedItem.itemInfo; 

        //모든 아이템에 스탯이 있는 것은 아니므로 Empty로 초기화
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        //consumables Length가 존재할 경우에만 반복문을 돌린다 (= 소비 아이템일 때만 돈다)
        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedItemStatName.text += selectedItem.consumables[i].type.ToString() + "\n";   //-item
            selectedItemStatValue.text += selectedItem.consumables[i].value.ToString() + "\n"; //-item
        }

        //슬롯에 들어있는 아이템 타입이 Consumable일 때만 보인다.
        useButton.SetActive(selectedItem.itemType == ItemType.Consumable); //-item

        //슬롯에 들어있는 아이템 타입이 Equipable이고, 해당 아이템이 장착되어 있지 않을 때만 보인다.
        equipButton.SetActive(selectedItem.itemType == ItemType.Equipable && !slots[index].equipped);  //-item

        //슬롯에 들어있는 아이템 타입이 Equipable이고, 해당 아이템이 장착되어 있을 때만 보인다.
        unEquipButton.SetActive(selectedItem.itemType == ItemType.Equipable && slots[index].equipped); //-item

        //버리기 버튼은 무조건 보이도록 설정
        dropButton.SetActive(true);
    }

}