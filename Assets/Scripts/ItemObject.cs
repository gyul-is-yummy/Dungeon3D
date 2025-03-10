using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetinteractText();    //화면에 띄워줄 텍스트를 반환하는 함수
    public void OnInteract();           //상호작용을 했을 때 실행할 함수
}

public class ItemObject : MonoBehaviour, IInteractable
{ 
    public ItemData data;

    public string GetinteractText()
    {
        Debug.Log("GetinteractText에 들어왔다");
        string str = $"{data.itemName}\n{data.itemInfo}";
        return str;
    }
    public void OnInteract()
    {

    }

}
