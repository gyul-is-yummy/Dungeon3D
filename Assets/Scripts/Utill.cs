using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Utill
{
    //���ڵ� �����ڵ� ���ǿ� �´� ������Ʈ�� ã�� ������ ����ϴ� �޼���
    //���߿� ���ʸ����� Transform, GameObject �� �� ���� �� �ֵ��� �ϰ���� (�ļ���)
    static public Transform FindChildRecursive(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            Transform found = FindChildRecursive(child, name);
            if (found != null)
                return found;
        }
        return null;
    }
}
