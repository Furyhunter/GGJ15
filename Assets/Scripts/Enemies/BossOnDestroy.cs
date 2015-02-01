using UnityEngine;
using System.Collections;

public class BossOnDestroy : MonoBehaviour
{
    void OnDestroy()
    {
        Application.LoadLevel("WinScene");
    }
}
