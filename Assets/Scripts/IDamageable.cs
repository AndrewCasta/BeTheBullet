using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void OnDamage(float damage, float damageForce, RaycastHit hit);
    public void OnDie(float damageForce, RaycastHit hit);
}
