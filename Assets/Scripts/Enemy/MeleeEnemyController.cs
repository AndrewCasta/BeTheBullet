using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class MeleeEnemyController : BaseEnemyController
{
    [Header("Effects")]
    [SerializeField] AudioClip attackSFX;
    [SerializeField] AudioClip damagedSFX;

    public enum MeleeAiState { idle, chase, attack, dead }
    public MeleeAiState State;

    [Header("Properties")]
    [SerializeField] float attackRange;
    [SerializeField] float attackRate;
    float attackTimer;

    // Internal variables
    Transform playerTransform;

    List<Color> hpColors = new List<Color> { Color.red, Color.magenta, Color.yellow, Color.green };
    private new Renderer renderer;



    override public void Start()
    {
        base.Start();
        playerTransform = GameObject.Find("Player").transform;
        renderer = GetComponent<Renderer>();
        State = MeleeAiState.chase;
    }

    override public void Update()
    {
        base.Update();
        StateMachine(State);
        UnitColor();
    }

    public override void OnDie()
    {
        base.OnDie();
        State = MeleeAiState.dead;
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
    }

    private void DeadState()
    {
        //
    }

    public override void OnDamage(float damage, float damageForce, RaycastHit hit)
    {
        base.OnDamage(damage, damageForce, hit);
        audioSource.PlayOneShot(damagedSFX);
    }

    private void UnitColor()
    {
        renderer.material.color = hpColors[CurrentHP];
    }
}
