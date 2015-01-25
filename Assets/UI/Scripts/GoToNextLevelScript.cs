using UnityEngine;
using System.Collections;

public class GoToNextLevelScript : MonoBehaviour
{

    public string nextLevel = "";

    void GoToNextLevel()
    {
        Application.LoadLevel(nextLevel);
    }
}
