using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;


    public ItemData itemData;           //현재 플레이어와 상호작용 하는 아이템
    public Action addItem;              //
    public Transform dropPosition;      //아이템이 떨어질 위치

    private void OnValidate()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
