using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
	public class CosNormalGenerator : MonoBehaviour
	{
		public List<Vector2> Normals = new List<Vector2>();

		// Use this for initialization
		void Start ()
		{
			List<Vector2> Points = new List<Vector2>();

			for (float i = -0.00390625f; i <= 1.0f; i += 0.00390625f)
			{
				Points.Add(new Vector2(i, (float)-Math.Cos(i)));
			}

			Vector2 p0p1Normal = Normalize(Points[1], Points[0]);
			Vector2 p1p2Normal = Normalize(Points[2], Points[1]);
			Vector2 p2p3Normal = Normalize(Points[3], Points[2]);
			Vector2 p3p4Normal = Normalize(Points[4], Points[3]);

			Vector2 n1 = Average(p0p1Normal, p1p2Normal);
			Vector2 n2 = Average(p1p2Normal, p2p3Normal);
			Vector2 n3 = Average(p2p3Normal, p3p4Normal);

			for (int i = 1; i < Points.Count - 1; i++)
			{
				Vector2 lastToCurrentSegment = Normalize(Points[i], Points[i-1]);
				Vector2 currentToNextSegment = Normalize(Points[i + 1], Points[i + 1]);
				Vector2 currentPointNormal = Average(lastToCurrentSegment, currentToNextSegment);

				Normals.Add(currentPointNormal);
			}
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}

		Vector2 Normalize(Vector2 v1, Vector2 v2)
		{
			Vector2 normal1 = new Vector2(-(v2.y - v1.y), v2.x - v1.x).normalized;
			Vector2 normal2 = new Vector2(v2.y - v1.y, -(v2.x - v1.x)).normalized;

			if (normal1.y >= 0)
				return normal1;
			
			return normal2;
		}

		private Vector2 Average(Vector2 v1, Vector2 v2)
		{
			return new Vector2((v1.x + v2.x)/2, (v1.y + v2.y)/2).normalized;
		}
	}
}
