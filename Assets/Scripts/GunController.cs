using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;

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
    float shootTimer;
    IEnumerator shootAutoRoutine;
    int currentAmmo;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        muzzlePoint = transform.Find("MuzzlePoint");
        canShoot = true;
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (auto)
            {
                shootAutoRoutine = ShootAuto();
                StartCoroutine(shootAutoRoutine);
            }
            else
                ShootRound();
        }
        if (context.canceled)
            StopCoroutine(shootAutoRoutine);
    }
    public void Reload(InputAction.CallbackContext context)
    {
        if (context.performed) StartCoroutine(ReloadGun());
    }

    IEnumerator ShootAuto()
    {
        while (true)
        {
            ShootRound();
            yield return new WaitForSeconds(1 / rateOfFire);
        }
    }

    private void ShootRound()
    {
        if (canShoot)
        {
            currentAmmo--;
            ShootRay();
            ShootEffects();
            if (currentAmmo == 0) StartCoroutine(ReloadGun());
        }
    }

    private IEnumerator ReloadGun()
    {
        canShoot = false;
        yield return new WaitForSeconds(reloadTime);
        audioSource.PlayOneShot(gunReloadSFX);
        currentAmmo = maxAmmo;
        canShoot = true;
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
