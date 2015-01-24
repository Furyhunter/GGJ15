using UnityEngine;
using System.Collections;

public class CameraTargetControl : MonoBehaviour
{
    private GameObject[] players;
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length != 0)
        {	
        	Vector3 avg = players[0].transform.position;
            for (int i = 1; i < players.Length; ++i)
            {
                avg = Vector3.Lerp(avg, players[i].transform.position, 0.5f);
            }
            transform.position = avg;
        }
    }
}
