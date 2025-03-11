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
        string str = $"{data.itemName}\n{data.itemInfo}";
        return str;
    }
    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();

        //E키를 누르면 인벤토리로 이동시킬 것이므로 맵에 있는 Object는 삭제한다. 
        Destroy(gameObject);
    }

}
