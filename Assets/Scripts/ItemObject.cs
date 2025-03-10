using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetinteractText();    //ȭ�鿡 ����� �ؽ�Ʈ�� ��ȯ�ϴ� �Լ�
    public void OnInteract();           //��ȣ�ۿ��� ���� �� ������ �Լ�
}

public class ItemObject : MonoBehaviour, IInteractable
{ 
    public ItemData data;

    public string GetinteractText()
    {
        Debug.Log("GetinteractText�� ���Դ�");
        string str = $"{data.itemName}\n{data.itemInfo}";
        return str;
    }
    public void OnInteract()
    {

    }

}
