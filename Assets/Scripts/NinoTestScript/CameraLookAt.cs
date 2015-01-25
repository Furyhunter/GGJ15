using UnityEngine;
using System.Collections;

public class CameraLookAt : MonoBehaviour
{
    public GameObject target;
	void Update()
	{
        transform.LookAt(target.transform);
	}
}
