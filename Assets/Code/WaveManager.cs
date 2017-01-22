using UnityEngine;

namespace Assets.Code
{
	public class WaveManager : MonoBehaviour, IResolveable
	{
		[SerializeField] public CosNormalGenerator Normals;
	    [SerializeField] public SurfaceBehaviour Surface;
	}
}
