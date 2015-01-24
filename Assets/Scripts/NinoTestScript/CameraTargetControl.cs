using UnityEngine;
using System.Collections;

public class CameraTargetControl : MonoBehaviour
{
	private GameObject[] players;
	void Update()
	{
		players = GameObject.FindGameObjectsWithTag("Player");
		Vector3 avg = new Vector3(0,0,0);
		foreach(GameObject e in players){
			avg += e.transform.position;
		}
		avg.y = 0;
		avg /= players.Length;
	}
}
