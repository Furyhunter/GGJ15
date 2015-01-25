using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour
{

    public GameObject player;
    public Transform[] spawnPoints;

    public GameHUD HUDObjectP1;
    public GameHUD HUDObjectP2;
    public GameHUD HUDObjectP3;
    public GameHUD HUDObjectP4;

	void Start()
	{
	    for(int k = 0; k < GlobalState.NumberOfPlayers; k++){
            PlayerAttrs attrs = ((GameObject)Instantiate(player, spawnPoints[k].position, spawnPoints[k].rotation)).GetComponent<PlayerAttrs>();
            switch(k)
            {
                case 0:
                    attrs.color = PlayerAttrs.PlayerColor.RED;
                    if (HUDObjectP1) HUDObjectP1.Player = attrs.gameObject;
                    continue;
                case 1:
                    attrs.color = PlayerAttrs.PlayerColor.BLUE;
                    if (HUDObjectP2) HUDObjectP2.Player = attrs.gameObject;
                    continue;
                case 2:
                    attrs.color = PlayerAttrs.PlayerColor.GREEN;
                    if (HUDObjectP3) HUDObjectP3.Player = attrs.gameObject;
                    continue;
                case 3:
                    attrs.color = PlayerAttrs.PlayerColor.YELLOW;
                    if (HUDObjectP4) HUDObjectP4.Player = attrs.gameObject;
                    continue;
            }
        }
	}
	
	void Update()
	{
	
	}
}
