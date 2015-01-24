using UnityEngine;
using System.Collections;

public class CameraTestMovement : MonoBehaviour
{
	
	void Update()
	{
		transform.position = new Vector3(0, 0, 5 + 10 * Mathf.Sin(Time.time));
	}
}
