using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using InControl;

public class PlayerNumberController : MonoBehaviour
{
    public string LevelToJumpTo = "LevelScene";

    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button Button4;

    public Text ControllerWarningText;

    private int waitFrames = 5;
    private bool lateSelect = false;

    void Awake()
    {
        FindObjectOfType<InControlInputModule>().enabled = false;
        var numControllers = InputManager.Devices.Count;
        Button2.interactable = false;
        Button3.interactable = false;
        Button4.interactable = false;

        if (numControllers >= 4)
        {
            Button4.interactable = true;
        }
        if (numControllers >= 3)
        {
            Button3.interactable = true;
        }
        if (numControllers >= 2)
        {
            Button2.interactable = true;
        }
        if (numControllers >= 1)
        {
            Button1.interactable = true;
        }
    }

    void LateUpdate()
    {
        // We do this to ensure the button isn't suppressed immediately upon loading.
        if (!lateSelect && waitFrames == 0)
        {
            FindObjectOfType<InControlInputModule>().enabled = true;
            lateSelect = true;
            Button1.Select();
        }

        waitFrames -= 1;
    }

    public void SetNumPlayersAndJump(int NumPlayers)
    {
        GlobalState.NumberOfPlayers = NumPlayers;
        Application.LoadLevel(LevelToJumpTo);
    }
}
