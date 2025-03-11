using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;                    //슬롯창에 넣어질 아이템 정보

    public Button button;
    public Image icon;
    public TextMeshProUGUI quatityText;
    //private Outline outline;

    public UIInventory inventory;            //UIInventory에 대한 정보

    public int index;           //이 아이템이 몇 번째 아이템 슬롯인지에 대한 정보
    public bool equipped;       //이 아이템이 장착되어 있는지에 대한 정보
    public int quantity;        //이 아이템의 개수에 대한 정보

    private void OnValidate()
    {
        button = GetComponent<Button>();
        icon = Utill.FindChildRecursive(transform, "Icon").GetComponent<Image>();
        quatityText = Utill.FindChildRecursive(transform, "QuanitityText").GetComponent<TextMeshProUGUI>();
        inventory = GetComponentInParent<UIInventory>();
    }


    public void Set()
    {
        //아이콘 이미지를 켜준다.
        icon.gameObject.SetActive(true);
        icon.sprite = item.itemIcon;

        //아이템의 개수가 1개 이상이면 개수를 표시해준다.
        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

    }

    //아이템을 버리거나 사용했을 때 실행할 메서드
    public void Clear()
    {
        //아이템 데이터를 비워준다.
        item = null;
        //아이콘 이미지를 끈다.
        icon.gameObject.SetActive(false);
        //개수 표시를 비워준다.
        quatityText.text = string.Empty;
    }

    public void OnClickButton()
    {
        //나(슬롯)의 인덱스를 넘겨준다.
        inventory.SelectItem(index);
    }



}
