using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuController : MonoBehaviour
{
    public GameObject PlayerNumberDialogPrefab;

    private int fullscreenCooldown = 0;

    void Update()
    {
        fullscreenCooldown--;
    }

    public void OpenPlayerDialog()
    {
        // Modal dialog
        var go = (GameObject) Instantiate(PlayerNumberDialogPrefab);

        Destroy(gameObject);
    }

    public void OpenOptionsDialog()
    {
        // Modal dialog
    }

    public void ToggleFullscreen()
    {
        if (fullscreenCooldown > 0)
        {
            return;
        }

        if (Screen.fullScreen)
        {
            Screen.SetResolution(1280, 720, false);
            fullscreenCooldown = 5;
        }
        else
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            fullscreenCooldown = 5;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
