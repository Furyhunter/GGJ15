using UnityEngine;
using System.Collections;

public class DestroyParticlesOnEnd : MonoBehaviour
{
	void Update()
	{
        var partSys = GetComponent<ParticleSystem>();
        if (partSys != null)
        {
            if (!partSys.IsAlive())
            {
                Destroy(gameObject);
            }
        }
	}
}
