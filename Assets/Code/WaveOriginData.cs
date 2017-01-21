using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WaveOriginData
{
	public Vector3 Origin;
	public float Age;
	public float Magnitude;

	public void Update()
	{
		//Age += Time.deltaTime;
	}

	// source player for scoring
}
