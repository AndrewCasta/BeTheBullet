using System;
using UnityEngine;

public abstract class BaseEnemyController : MonoBehaviour, IDamageable
{
    [Header("Enemy Properties")]
    [SerializeField] int maxHP;
    [SerializeField] int currentHP;

    [Header("Effects")]
    [SerializeField] ParticleSystem damageVFX;
    [SerializeField] AudioClip damageSFX;
    [SerializeField] AudioClip deathSFX;

    // Internal vars
    AudioSource audioSource;
    Rigidbody rb;

    public virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        SetAlivePhysics(true);
    }

    void Update()
    {
        // Nope
    }

    public void OnDamage(float damage, float damageForce, RaycastHit hit)
    {
        Debug.Log($"{name} took {damage} damage.");
        currentHP--;
        DamangeEffects(hit);
        if (currentHP < 1) OnDie();
        // Add forece after death, so it applies once physics are enabled
        rb.AddForceAtPosition(damageForce * -hit.normal, hit.point, ForceMode.Impulse);
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
