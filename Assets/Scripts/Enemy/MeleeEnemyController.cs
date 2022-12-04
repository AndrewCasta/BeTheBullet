using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class MeleeEnemyController : BaseEnemyController
{
    [Header("Effects")]
    [SerializeField] AudioClip attackSFX;

    public enum MeleeAiState { idle, chase, attack, dead }
    public MeleeAiState State;

    [Header("Properties")]
    [SerializeField] float attackRange;
    [SerializeField] float attackRate;
    float attackTimer;

    // Internal variables
    Transform playerTransform;

    override public void Start()
    {
        base.Start();
        playerTransform = GameObject.Find("Player").transform;
        State = MeleeAiState.chase;
    }

    override public void Update()
    {
        base.Update();
        StateMachine(State);
    }

    private void StateMachine(MeleeAiState state)
    {
        switch (state)
        {
            case MeleeAiState.chase:
                ChaseState();
                break;
            case MeleeAiState.attack:
                AttackState();
                break;
            case MeleeAiState.dead:
                DeadState();
                break;
        }
    }

    private void ChaseState()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
        // TODO - Reduce calls to this. Check range & add timer.
        agent.destination = playerTransform.position;
        // CheckAttachRange
        if (Vector3.Distance(transform.position, playerTransform.position) < attackRange)
            State = MeleeAiState.attack;
    }

    private void AttackState()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer < 0)
        {
            Attack();
            attackTimer = attackRate;
        }
        // CheckAttackRange
        if (Vector3.Distance(transform.position, playerTransform.position) > attackRange)
            State = MeleeAiState.chase;
    }

    private void Attack()
    {
        Debug.Log("KIYAAAA");
        audioSource.PlayOneShot(attackSFX);
        animator.SetFloat("Speed", agent.velocity.magnitude);
        animator.Play("Attack");
    }

    private void DeadState()
    {
        //
    }

    public override void OnDamage(float damage, float damageForce, RaycastHit hit)
    {
        base.OnDamage(damage, damageForce, hit);
        audioSource.PlayOneShot(damageSFX);
    }

    public override void OnDie(float damageForce, RaycastHit hit)
    {
        base.OnDie(damageForce, hit);
        State = MeleeAiState.dead;
    }
}
