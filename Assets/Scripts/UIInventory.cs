using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    private ItemData selectedItem;           //선택한 아이템 (노션은 ItemData가 아니라 ItemSlot인데 뭐지)
    private int selectedItemIndex = 0;       //선택한 아이템의 인덱스

    private int curEquipIndex;               //현재 장착하고 있는 아이템의 인덱스

    private Player player;

    private void OnValidate()
    {
       
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        controller = player.GetComponent<PlayerController>();
        condition = player.GetComponent<PlayerCondition>();
        dropPosition = player.transform;

        //controller.inventory += Toggle;
        //player.addItem += AddItem;

        //처음에 인벤토리가 보이면 안되므로 꺼진 상태로 시작
        this.gameObject.SetActive(false);

        //아이템 슬롯은 슬롯 판넬의 자식 오브젝트로 들어가 있음
        //childCount을 통해 슬롯 판넬의 자식 오브젝트 개수를 받아온 뒤 그만큼 슬롯을 만들어준다.
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            //slots[i].index = i;
            //slots[i].inventory = this;
            //slots[i].Clear();
        }
    }
}
