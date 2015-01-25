using UnityEngine;
using System.Collections;

public class IDIE : MonoBehaviour
{
    public float timeUntilDeath = 2;
	void Start()
	{
	
	}
	
	void Update()
	{
        if (timeUntilDeath < 0)
            GameObject.Destroy(gameObject);
        timeUntilDeath -= Time.deltaTime;
	}
}
