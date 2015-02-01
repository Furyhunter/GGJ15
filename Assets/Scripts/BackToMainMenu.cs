using UnityEngine;
using System.Collections;

public class BackToMainMenu : MonoBehaviour
{
    public double delay = 2.0;
	
	void Update()
	{
        delay -= Time.deltaTime;
        if (delay <= 0)
            Application.LoadLevel("MainMenu");
	}
}
