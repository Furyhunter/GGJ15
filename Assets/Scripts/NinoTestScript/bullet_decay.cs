using UnityEngine;
using System.Collections;

public class bullet_decay : MonoBehaviour
{
	public float BulletLife = 4.0f;
	void Update()
	{
		if(BulletLife < 0)
			GameObject.Destroy(gameObject, 0.0f);
		BulletLife -= Time.deltaTime;
	}
}
