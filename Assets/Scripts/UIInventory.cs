using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    private ItemData selectedItem;           //������ ������ (����� ItemData�� �ƴ϶� ItemSlot�ε� ����)
    private int selectedItemIndex = 0;       //������ �������� �ε���

    private int curEquipIndex;               //���� �����ϰ� �ִ� �������� �ε���

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

        //ó���� �κ��丮�� ���̸� �ȵǹǷ� ���� ���·� ����
        this.gameObject.SetActive(false);

        //������ ������ ���� �ǳ��� �ڽ� ������Ʈ�� �� ����
        //childCount�� ���� ���� �ǳ��� �ڽ� ������Ʈ ������ �޾ƿ� �� �׸�ŭ ������ ������ش�.
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
