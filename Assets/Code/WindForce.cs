using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class WindForce : MonoBehaviour
{
    public float maxForce;
    public float maxInterval;
    /// <summary>
    /// This int is a 0 - 99 the hight the value the more chance for the force to be applied earlier
    /// </summary>
    public int wantitudiness;

    private Rigidbody _object;
    private float _dt = 0;
    private float _chanceCube = 0;
    private float _testTime = 0.5f;

    void Start()
    {
        _object = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _dt += Time.deltaTime;
        _chanceCube += Time.deltaTime;

        if (_chanceCube > _testTime)
        {
            _chanceCube = 0;
            int chance;
            chance = Random.Range(0, 99);
            if (chance < wantitudiness)
            {
                ApplyForce();
                _dt = 0;
            }

        }

        if (_dt > maxInterval)
        {
            _dt = 0;
            ApplyForce();
        }

    }

    void ApplyForce()
    {
        _object.AddForce(new Vector3(maxForce * Rand(), maxForce * Rand(), maxForce * Rand()));
    }

    float Rand()
    {
        return Random.Range(0, 100) / 100.0f;
    }
}
