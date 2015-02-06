using UnityEngine;
using System.Collections;

public class CameraTargetControl : MonoBehaviour
{
    private GameObject[] players;

    private Vector3 oldTarget = new Vector3();

    void FixedUpdate()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length != 0)
        {	
        	Vector3 avg = players[0].transform.position;
            var curPos = transform.position;
            for (int i = 1; i < players.Length; ++i)
            {
                avg = Vector3.Lerp(avg, players[i].transform.position, 0.5f);
            }
            transform.position = Vector3.Lerp(transform.position, avg, .1f);
            oldTarget = avg;
        }
    }
}
