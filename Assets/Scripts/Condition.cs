using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue = 100;         //현재값
    public float maxValue = 100;         //최대값
    public float passiveValue = 5;       //패시브 값
    public Image uiBar;                  //컨디션 바 리소스

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
        //UI업데이트
        uiBar.fillAmount = GetPercentage();
    }

    private float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void Add(float amount)
    {
        //둘 중에 더 작은 값을 curValue에 넣어준다.
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        //둘 중에 더 큰 값을 curValue에 넣어준다.
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

}
