using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue = 100;         //���簪
    public float maxValue = 100;         //�ִ밪
    public float passiveValue = 5;       //�нú� ��
    public Image uiBar;                  //����� �� ���ҽ�

    private void OnValidate()
    {
        uiBar = transform.Find("Bar").GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        curValue = maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        //UI������Ʈ
        uiBar.fillAmount = GetPercentage();
    }

    private float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void Add(float amount)
    {
        //�� �߿� �� ���� ���� curValue�� �־��ش�.
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        //�� �߿� �� ū ���� curValue�� �־��ش�.
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

}
