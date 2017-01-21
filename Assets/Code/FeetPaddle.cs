using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class FeetPaddle : MonoBehaviour
{
    public ConstantForce force;
    public float paddleTime;
    public float paddleStrenght;
    public bool paddleUp = false;

    private float _dt = 0.0f;

	// Update is called once per frame
	void Update ()
	{
        _dt += Time.deltaTime;
	    if (_dt >= paddleTime)
	    {
	        _dt = 0.0f;
	        paddleUp = !paddleUp;
	    }

	    if (paddleUp)
	    {
	        force.relativeTorque = new Vector3(0, 0, paddleStrenght);
	    }
        else
        {
            force.relativeTorque = new Vector3(0, 0, -paddleStrenght);
        }
    }
}
