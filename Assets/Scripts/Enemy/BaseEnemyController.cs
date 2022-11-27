using System;
using UnityEngine;

public abstract class BaseEnemyController : MonoBehaviour, IDamageable
{
    [Header("Enemy Properties")]
    [SerializeField] int maxHP;
    [Tooltip("Leave 0 to set MaxHP on spawn")]
    public int CurrentHP;

    [Header("Effects")]
    [SerializeField] ParticleSystem damageVFX;
    [SerializeField] AudioClip damageSFX;
    [SerializeField] AudioClip deathSFX;

    // Internal variables
    AudioSource audioSource;
    Rigidbody rb;
    protected CharacterController characterController;

    public virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        SetDeadPhysics(false);
        if (CurrentHP == 0) CurrentHP = maxHP;
    }

    public virtual void Update()
    {
        // Nope
    }

    public void OnDamage(float damage, float damageForce, RaycastHit hit)
    {
        if (CurrentHP > 0) CurrentHP--;
        Debug.Log($"{name} took {damage} damage and has {CurrentHP} HP left");
        DamageEffects(hit);
        if (CurrentHP < 1)
        {
            OnDie();
            rb.AddForceAtPosition(damageForce * -hit.normal, hit.point, ForceMode.Impulse);
        }
        else rb.AddForceAtPosition(damageForce / 10 * -hit.normal, hit.point, ForceMode.Impulse);
    }

    private void DamageEffects(RaycastHit hit)
    {
        if (damageSFX != null) audioSource.PlayOneShot(damageSFX);
        if (damageVFX != null) Instantiate(damageVFX, hit.point, Quaternion.LookRotation(hit.normal));
    }

    public virtual void OnDie()
    {
        Debug.Log($"{name} died.");
        if (deathSFX != null) audioSource.PlayOneShot(deathSFX);
        SetDeadPhysics(true);
    }

    public void SetDeadPhysics(bool state)
    {
        rb.freezeRotation = !state;
    }
}
