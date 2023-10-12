using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

[System.Serializable]
public class Condition
{
    public float curValue;
    public float startValue;
    public float maxValue;
    public float regenRate;
    public float decayRate;
    public Image uiBar;

    public void Add(float amount) // 수치 더하기
    {
        curValue = Mathf.Min(curValue + amount, maxValue); // maxValue는 이하
    }
    public void Subtract(float amount)// 수치 빼기
    {
        curValue = Mathf.Max(curValue - amount, 0f);
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}

public class PlayerConditions : MonoBehaviour, IDamagable
{
    public Condition health;
    public Condition stamina;

    public static PlayerConditions playerConditions;

    public UnityEvent OnTakeDamage;

    private void Awake()
    {
        playerConditions = this;
    }

    private void Start()
    {
        health.curValue = health.startValue;
        stamina.curValue = stamina.startValue;
        //InvokeRepeating("AccelRateOutput", 0, 1.0f);
    }

    private void Update()
    {
        if (PlayerController.instance.accelRateInput == 1)
        {
            stamina.Add(stamina.regenRate * Time.deltaTime); 
            if (stamina.curValue / stamina.maxValue > 0.3f)
            {
                PlayerController.instance.canAccel = true;
            }
        }
        else if (PlayerController.instance.accelRateInput == 2)
        {
            stamina.Subtract(stamina.decayRate * Time.deltaTime);
        }

        if (stamina.curValue <= 0f)
        {
            PlayerController.instance.canAccel = false;
        }

        health.uiBar.fillAmount = health.GetPercentage();
        stamina.uiBar.fillAmount = stamina.GetPercentage();
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        OnTakeDamage?.Invoke();
    }

    //void AccelRateOutput()
    //{
    //    Debug.Log($"{PlayerController.instance.accelRateInput}, {PlayerController.instance.canAccel}");
    //}

}
