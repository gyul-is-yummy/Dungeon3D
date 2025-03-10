using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    // Start is called before the first frame update
    private void OnValidate()
    {
        health = transform.Find("Health").GetComponent<Condition>();
        hunger = transform.Find("Hunger").GetComponent<Condition>();
        stamina = transform.Find("Stamina").GetComponent<Condition>();
    }

}
