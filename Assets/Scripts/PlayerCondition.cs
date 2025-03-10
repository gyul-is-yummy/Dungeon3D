using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay = 5;   //굶주린 상태일 때 체력이 줄어드는 양

    private void Start()
    {
        //할당을 깜빡 했다면 Find로 찾아서 넣어준다 (방어코드)
        if(uiCondition == null)
            uiCondition = GameObject.Find("Condition").GetComponent<UICondition>();
    }

    // Update is called once per frame
    void Update()
    {
        //기기마다 성능에 따라서 너무 빠르게 늘어나거나 감소하는 것을 막기 위해 Time.deltaTime을 곱해준다.
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue <= 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        //체력이 다 떨어지면 죽는다.
        if (health.curValue <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Die()
    {
        Debug.Log("죽었다!");
    }
}
