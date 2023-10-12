using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage;
    public float damageRate; // 데미지 주는 간격

    private List<IDamagable> thingsToDamage = new List<IDamagable>(); // IDamagable을 가진 오브젝트인지 확인하기 위한 변수 리스트

    private void Start() // 이 스크립트를 가진 오브젝트는 시작부터 데미지를 계속 주는 메서드를 가진다. 피격은 여기서 시작한다.
    {
        InvokeRepeating("DealDamage", 0, damageRate);
    }

    private void DealDamage()
    {
        for (int i = 0; i < thingsToDamage.Count; i++)
        {
            thingsToDamage[i].TakePhysicalDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other) // 이 오브젝트와 접촉 시
    {
        if (other.gameObject.TryGetComponent(out IDamagable damagable)) // 접촉한 오브젝트가 IDamagable을 갖고 있다면
        {
            thingsToDamage.Add(damagable); // 데미지를 입을 대상에 추가 => 바로 피격 실시
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            thingsToDamage.Remove(damagable);
        }
    }
}
