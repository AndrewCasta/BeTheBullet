using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Gun Properties")]
    [Tooltip("Bullets per second")]
    [SerializeField] int damage;
    [SerializeField] int ammo;
    [SerializeField] float damageForce;
    int ammoRemaining;
    [Tooltip("Automatic or semi auto weapon")]
    [SerializeField] bool auto;
    [Tooltip("Bullets per second")]
    [SerializeField] float rateOfFire;
    [SerializeField] float reloadTime;

    [Header("Setup")]
    [SerializeField] Transform rayOrigin;
    [SerializeField] AudioSource audioSource;

    [Header("Effects")]
    [SerializeField] AudioClip gunSFX;
    [Tooltip("Not implemented")]
    [SerializeField] ParticleSystem muzzleVFX;
    [Tooltip("Not implemented")]

    [SerializeField] ParticleSystem hitVFX;



    public void Shoot()
    {
        ShootRay();
        ShootEffects();
    }
    private void ShootRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit))
        {
            Debug.Log($"{hit.collider.name}: ouch!");
            IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.OnDamage(damage, damageForce, hit);
            }
        }
    }
    private void ShootEffects()
    {
        audioSource.PlayOneShot(gunSFX);
    }

}
