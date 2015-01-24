using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{	
	void Update()
	{	
		transform.Rotate(Vector3.up * Time.deltaTime * 10);
	}
}
