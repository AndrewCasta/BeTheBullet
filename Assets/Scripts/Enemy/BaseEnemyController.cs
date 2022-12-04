using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemyController : MonoBehaviour, IDamageable
{
    [Header("Enemy Properties")]
    [SerializeField] int maxHP;
    [Tooltip("Leave 0 to set MaxHP on spawn")]

    [Header("Effects")]
    [SerializeField] ParticleSystem damageVFX;
    [SerializeField] protected AudioClip damageSFX;
    [SerializeField] AudioClip deathSFX;

    // Internal variables
    [NonSerialized] public int CurrentHP;
    protected AudioSource audioSource;
    protected NavMeshAgent agent;
    protected Animator animator;
    Rigidbody[] rigidbodies;


    public virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        SetRagdoll(false);
        if (CurrentHP == 0) CurrentHP = maxHP;
    }

    public virtual void Update()
    {
        // Nope
    }

    public virtual void OnDamage(float damage, float damageForce, RaycastHit hit)
    {
        if (CurrentHP > 0) CurrentHP--;
        Debug.Log($"{name} took {damage} damage and has {CurrentHP} HP left");
        DamageEffects(hit);
        if (CurrentHP < 1)
        {
            OnDie();
            hit.rigidbody.AddForceAtPosition(damageForce * -hit.normal, hit.point, ForceMode.Impulse);
        }
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
        SetRagdoll(true);
    }

    public void SetRagdoll(bool state)
    {
        foreach (var rb in rigidbodies)
            rb.isKinematic = !state;
        agent.enabled = !state;
        animator.enabled = !state;
    }
}
