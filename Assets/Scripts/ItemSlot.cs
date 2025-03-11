using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;                    //����â�� �־��� ������ ����

    public Button button;
    public Image icon;
    public TextMeshProUGUI quatityText;
    //private Outline outline;

    public UIInventory inventory;            //UIInventory�� ���� ����

    public int index;           //�� �������� �� ��° ������ ���������� ���� ����
    public bool equipped;       //�� �������� �����Ǿ� �ִ����� ���� ����
    public int quantity;        //�� �������� ������ ���� ����

    private void OnValidate()
    {
        button = GetComponent<Button>();
        icon = Utill.FindChildRecursive(transform, "Icon").GetComponent<Image>();
        quatityText = Utill.FindChildRecursive(transform, "QuanitityText").GetComponent<TextMeshProUGUI>();
        inventory = GetComponentInParent<UIInventory>();
    }


    public void Set()
    {
        //������ �̹����� ���ش�.
        icon.gameObject.SetActive(true);
        icon.sprite = item.itemIcon;

        //�������� ������ 1�� �̻��̸� ������ ǥ�����ش�.
        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

    }

    //�������� �����ų� ������� �� ������ �޼���
    public void Clear()
    {
        //������ �����͸� ����ش�.
        item = null;
        //������ �̹����� ����.
        icon.gameObject.SetActive(false);
        //���� ǥ�ø� ����ش�.
        quatityText.text = string.Empty;
    }

    public void OnClickButton()
    {
        //��(����)�� �ε����� �Ѱ��ش�.
        inventory.SelectItem(index);
    }



}
