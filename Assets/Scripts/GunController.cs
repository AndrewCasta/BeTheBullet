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
    [SerializeField] int maxAmmo;
    [SerializeField] float damageForce;
    [Tooltip("Automatic or semi auto weapon")]
    [SerializeField] bool auto;
    [Tooltip("Bullets per second")]
    [SerializeField] float rateOfFire;
    [SerializeField] float reloadTime;

    [Header("Setup")]
    [SerializeField] Transform rayOrigin;

    [Header("Effects")]
    [SerializeField] AudioClip gunShotSFX;
    [SerializeField] AudioClip gunDrySFX;
    [SerializeField] AudioClip gunReloadSFX;
    [SerializeField] ParticleSystem muzzleVFX; // This might not be in the right place when played?
    [Tooltip("Generic effect that will play when a target is hit that does not have it's own effect.")]
    [SerializeField] ParticleSystem hitVFX;

    // Internal vars
    AudioSource audioSource;
    Transform muzzlePoint;
    bool canShoot;
    int currentAmmo;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        muzzlePoint = transform.Find("MuzzlePoint");
        canShoot = true;
        currentAmmo = maxAmmo;
    }
    public void Shoot()
    {
        if (canShoot)
        {
            currentAmmo--;
            ShootRay();
            ShootEffects();
            if (currentAmmo > 0) Reload();
        }
    }

    private void Reload()
    {
        canShoot = false;
        audioSource.PlayOneShot(gunReloadSFX);
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
        audioSource.PlayOneShot(gunShotSFX);
        if (muzzleVFX != null) Instantiate(muzzleVFX, muzzlePoint);
    }

}
