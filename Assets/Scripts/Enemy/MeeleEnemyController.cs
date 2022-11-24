using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class MeeleEnemyController : BaseEnemyController
{
    List<Color> hpColors = new List<Color> { Color.red, Color.magenta, Color.yellow, Color.green };
    private new Renderer renderer;
    override public void Start()
    {
        base.Start();
        renderer = GetComponent<Renderer>();
    }

    override public void Update()
    {
        base.Update();
        // Unit behaviour
        UnitColour();
    }

    private void UnitColour()
    {
        renderer.material.color = hpColors[CurrentHP];
    }
}
