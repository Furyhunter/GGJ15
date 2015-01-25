using UnityEngine;
using System.Collections;

public class GoToNextLevelScript : MonoBehaviour
{

    public int nextLevelIndex = 1;

    void GoToNextLevel()
    {
        Application.LoadLevel(nextLevelIndex);
    }
}
