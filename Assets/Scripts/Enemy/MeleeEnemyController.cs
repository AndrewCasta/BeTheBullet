using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class MeleeEnemyController : BaseEnemyController
{
    // AI
    public enum MeleeAiState { idle, chase, attack, dead }
    public MeleeAiState State;
    Transform playerTransform;
    [SerializeField] float rotateSpeed;
    [SerializeField] float attackRange;

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
        Vector3 direction = Vector3.RotateTowards(transform.forward, playerTransform.position - transform.position, rotateSpeed * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(direction);
        CheckAttachRange();
    }

    private void CheckAttachRange()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < attackRange)
            State = MeleeAiState.attack;
    }

    private void AttackState()
    {
        Attack();
    }

    private void Attack()
    {
        Debug.Log("KIYAAAA");
    }

    private void DeadState()
    {
        //
    }

    private void UnitColor()
    {
        renderer.material.color = hpColors[CurrentHP];
    }
}
