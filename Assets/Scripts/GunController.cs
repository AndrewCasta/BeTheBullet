using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
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

    [Header("Effects")]
    [SerializeField] AudioClip gunSFX;
    [SerializeField] ParticleSystem muzzleVFX; // This might not be in the right place when played?
    [Tooltip("Generic effect that will play when a taget is hit that does not have it's own effect.")]
    [SerializeField] ParticleSystem hitVFX;

    // Internal vars
    AudioSource audioSource;
    Transform muzzlePoint;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        muzzlePoint = transform.Find("MuzzlePoint");
    }



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
            else
            {
                if (hitVFX != null) Instantiate(hitVFX, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
    private void ShootEffects()
    {
        audioSource.PlayOneShot(gunSFX);
        if (muzzleVFX != null) Instantiate(muzzleVFX, muzzlePoint);
    }

}
