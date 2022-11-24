using System;
using UnityEngine;

public abstract class BaseEnemyController : MonoBehaviour, IDamageable
{
    [Header("Enemy Properties")]
    [SerializeField] int maxHP;

    [Header("Effects")]
    [SerializeField] ParticleSystem damageVFX;
    [SerializeField] AudioClip damageSFX;
    [SerializeField] AudioClip deathSFX;

    // Internal vars
    int currentHP;
    AudioSource audioSource;
    Rigidbody rb;

    public virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        SetAlivePhysics(true);
        currentHP = maxHP;
    }

    void Update()
    {
        // Nope
    }

    public void OnDamage(float damage, float damageForce, RaycastHit hit)
    {
        currentHP--;
        Debug.Log($"{name} took {damage} damage and has {currentHP} HP left");
        DamangeEffects(hit);
        if (currentHP < 1)
        {
            OnDie();
            rb.AddForceAtPosition(damageForce * -hit.normal, hit.point, ForceMode.Impulse);
        }
        else rb.AddForceAtPosition(damageForce / 10 * -hit.normal, hit.point, ForceMode.Impulse);
    }

    private void DamangeEffects(RaycastHit hit)
    {
        if (damageSFX != null) audioSource.PlayOneShot(damageSFX);
        if (damageVFX != null) Instantiate(damageVFX, hit.point, Quaternion.LookRotation(hit.normal));
    }

    public void OnDie()
    {
        Debug.Log($"{name} died.");
        if (deathSFX != null) audioSource.PlayOneShot(deathSFX);
        SetAlivePhysics(false);
    }

    public void SetAlivePhysics(bool state)
    {
        rb.freezeRotation = state;
    }
}
