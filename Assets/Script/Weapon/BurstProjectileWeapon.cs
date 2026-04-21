using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BurstProjectileWeapon : ProjectileWeapon
{
    [SerializeField] private int _burstShotCount;
    [SerializeField] private float _burstShotInterval;

    protected override IEnumerator AttackCoroutine()
    {
        for (int i = 0; i < _burstShotCount; i++)
        {
            yield return base.AttackCoroutine();
            yield return new WaitForSeconds(_burstShotInterval);
        }
    }
}