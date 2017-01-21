using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveOriginData
{
	public static readonly uint WAVE_LIFETIME = 100;
    public readonly static uint MAX_WAVES = 1;

	public Vector3 Origin;
	public float Age;
	public float Magnitude;
	public float PercentLife;

	public void Init()
	{
		Age = 1;
	}

	public void Update()
	{
		Age += Time.deltaTime;
		PercentLife = Mathf.Abs(Age - WAVE_LIFETIME) / WAVE_LIFETIME;
	}

	// source player for scoring
}
