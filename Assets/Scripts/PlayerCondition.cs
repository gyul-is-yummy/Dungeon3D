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

    public float noHungerHealthDecay = 5;   //���ָ� ������ �� ü���� �پ��� ��

    private void Start()
    {
        //�Ҵ��� ���� �ߴٸ� Find�� ã�Ƽ� �־��ش� (����ڵ�)
        if(uiCondition == null)
            uiCondition = GameObject.Find("Condition").GetComponent<UICondition>();
    }

    // Update is called once per frame
    void Update()
    {
        //��⸶�� ���ɿ� ���� �ʹ� ������ �þ�ų� �����ϴ� ���� ���� ���� Time.deltaTime�� �����ش�.
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue <= 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        //ü���� �� �������� �״´�.
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
        Debug.Log("�׾���!");
    }
}
