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
        string str = $"{data.itemName}\n{data.itemInfo}";
        return str;
    }
    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();

        //EŰ�� ������ �κ��丮�� �̵���ų ���̹Ƿ� �ʿ� �ִ� Object�� �����Ѵ�. 
        Destroy(gameObject);
    }

}
