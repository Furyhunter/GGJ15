using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour
{

    public GameObject player;
    public Transform[] spawnPoints;

	void Start()
	{
	    for(int k = 0; k < GlobalState.NumberOfPlayers; k++){
            PlayerAttrs attrs = ((GameObject)Instantiate(player, spawnPoints[k].position, spawnPoints[k].rotation)).GetComponent<PlayerAttrs>();
            switch(k)
            {
                case 0: attrs.color = PlayerAttrs.PlayerColor.RED; continue;
                case 1: attrs.color = PlayerAttrs.PlayerColor.BLUE; continue;
                case 2: attrs.color = PlayerAttrs.PlayerColor.GREEN; continue;
                case 3: attrs.color = PlayerAttrs.PlayerColor.YELLOW; continue;
            }
        }
	}
	
	void Update()
	{
	
	}
}
