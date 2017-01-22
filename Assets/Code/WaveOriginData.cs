using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveOriginData
{
	public static readonly float WAVE_LIFETIME = 15.0f;
    public readonly static uint MAX_WAVES = 4;
    public readonly static float WAVE_VELOCITY = 4.0f;
    public readonly static float WAVE_WIDTH = 2.0f;

    public Vector3 Origin;
	public float Age;
    public float Magnitude;
	public float PercentLife;

	public void Init()
	{
		Age = 0f;
        Magnitude = 1.0f;
	}

	public void Update()
	{
		Age += Time.deltaTime;
		PercentLife = 1.0f - (Age / WAVE_LIFETIME);
	}

	// source player for scoring
}
