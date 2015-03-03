using UnityEngine;
using UnityEngine.UI;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuController : MonoBehaviour
{
    public GameObject PlayerNumberDialogPrefab;

    public Text QualityLabel;

    private int fullscreenCooldown = 0;

    void Awake()
    {
        UpdateQualityLabel();
    }

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

    public void CycleGraphicsQuality()
    {
        if (QualitySettings.GetQualityLevel() >= 5)
        {
            QualitySettings.SetQualityLevel(0, true);
            UpdateQualityLabel();
            return;
        }
        QualitySettings.IncreaseLevel(true);
        UpdateQualityLabel();
    }

    public void UpdateQualityLabel()
    {
        string level = "Unknown";
        switch (QualitySettings.GetQualityLevel())
        {
            case 0:
                level = "Minimum";
                break;
            case 1:
                level = "Low";
                break;
            case 2:
                level = "Medium";
                break;
            case 3:
                level = "High";
                break;
            case 4:
                level = "Ultra";
                break;
            case 5:
                level = "Extreme";
                break;
            default:
                level = "What?";
                break;
        }

        QualityLabel.text = level;
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
