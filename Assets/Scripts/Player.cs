using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;


    public ItemData itemData;           //���� �÷��̾�� ��ȣ�ۿ� �ϴ� ������
    public Action addItem;              //
    public Transform dropPosition;      //�������� ������ ��ġ

    private void OnValidate()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
