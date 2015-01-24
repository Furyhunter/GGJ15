using UnityEngine;
using System.Collections;
using InControl;

public class PlayerController : MonoBehaviour
{
    InputDevice device;
    InputControl control;
    public float PlayerSpeed = 1.0f;
    CharacterController Mover; 
    void Start()
    {
        device = gameObject.GetComponent<PlayerAttrs>().controller;
        control = device.GetControl(InputControlType.Action1);
        Mover = gameObject.GetComponent<CharacterController>();
    }

	void Update()
	{
       // Debug.Log(device.Name);
        float x = device.LeftStickX;
        float y = device.LeftStickY;
       Vector3 InputDirection = new Vector3(x * PlayerSpeed, 0, 
            y * PlayerSpeed);
       Mover.SimpleMove(InputDirection);
        float Jumblies = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
       gameObject.GetComponentInChildren<Animator>().SetFloat("Speed", Jumblies);
        x = device.RightStickX;
        y = device.RightStickY;
        if (x != 0 && y != 0)
        {
            InputDirection = new Vector3(device.RightStickX * PlayerSpeed, 0,
                 device.RightStickY * PlayerSpeed);
            transform.rotation = Quaternion.LookRotation(InputDirection, Vector3.up);
        }
	}
}
