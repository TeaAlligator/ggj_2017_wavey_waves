using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WaveOriginData
{
    public readonly static uint MAX_WAVES = 2;

    public Vector2 Position;
	public float Age;
	public float Magnitude;

	// source player for scoring
}
