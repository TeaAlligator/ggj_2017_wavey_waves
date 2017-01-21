using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWaveDriver : MonoBehaviour
{
    private WaveOriginData[] waves = new WaveOriginData[WaveOriginData.MAX_WAVES];
    private Material material;

	// Use this for initialization
	void Start()
    {
        waves[0].Origin = new Vector2(10, 10);
        waves[0].Magnitude = 1.0f;
		waves[0].Age = 0.0f;
        //waves[1].Magnitude = 1.0f;
        material = gameObject.GetComponent<Renderer>().material;
    }

	// Update is called once per frame
	void Update()
    {
        Vector4[] data = new Vector4[WaveOriginData.MAX_WAVES];

        for (int i = 0; i < WaveOriginData.MAX_WAVES; i++)
        {
            //waves[i].Age += Time.deltaTime;
	        waves[i].Update();
            data[i] = new Vector4(waves[i].Origin.x, waves[i].Origin.y, waves[i].Age, waves[i].Magnitude);
        }

        material.SetVectorArray("waves", data);
    }
}
