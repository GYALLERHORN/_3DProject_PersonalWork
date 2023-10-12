using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage;
    public float damageRate; // ������ �ִ� ����

    private List<IDamagable> thingsToDamage = new List<IDamagable>(); // IDamagable�� ���� ������Ʈ���� Ȯ���ϱ� ���� ���� ����Ʈ

    private void Start() // �� ��ũ��Ʈ�� ���� ������Ʈ�� ���ۺ��� �������� ��� �ִ� �޼��带 ������. �ǰ��� ���⼭ �����Ѵ�.
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

    private void OnTriggerEnter(Collider other) // �� ������Ʈ�� ���� ��
    {
        if (other.gameObject.TryGetComponent(out IDamagable damagable)) // ������ ������Ʈ�� IDamagable�� ���� �ִٸ�
        {
            thingsToDamage.Add(damagable); // �������� ���� ��� �߰� => �ٷ� �ǰ� �ǽ�
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
