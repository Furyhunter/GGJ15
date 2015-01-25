using UnityEngine;
using System.Collections;
using InControl;

public class PlayerController : MonoBehaviour
{
    InputDevice device;
    InputControl control;
    public float PlayerSpeed = 1.0f;
    CharacterController Mover;

    Transform hand;
    void Start()
    {
        device = gameObject.GetComponent<PlayerAttrs>().controller;
        control = device.GetControl(InputControlType.Action1);
        Mover = gameObject.GetComponent<CharacterController>();
        hand = gameObject.GetComponent<PlayerAttrs>().AttachPoint;
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

        if (device.Action4)
        {
           Transform hand = gameObject.GetComponent<PlayerAttrs>().AttachPoint;
           if (hand.childCount > 0)
           {
               gameObject.GetComponent<Shoot>().CurrentWeapon = null;

               Transform gun = hand.GetChild(0);
               gun.gameObject.GetComponent<Weapon>().PickupDelay = 2f;
               gun.SetParent(null);
               gun.gameObject.rigidbody.isKinematic = false;
               
               gun.position += new Vector3(0, 1.5f, 0);
               gun.gameObject.rigidbody.AddForce(0, 250, 0);
               gun.gameObject.rigidbody.AddTorque(0, 200000, 0);
               gun.gameObject.rigidbody.detectCollisions = true;
           }
        }
	}

    void OnTriggerStay(Collider collider)

    {
        Debug.Log(collider.gameObject.tag);
        if (collider.gameObject.tag == "Weapon" && collider.gameObject.GetComponent<Weapon>().PickupDelay <= 0 && hand.childCount < 1)
        {
            collider.rigidbody.isKinematic = true;
            collider.rigidbody.detectCollisions = false;
            collider.transform.SetParent(hand, false);
            collider.gameObject.transform.localPosition = Vector3.zero;
            collider.transform.localRotation = Quaternion.identity;

            gameObject.GetComponent<Shoot>().CurrentWeapon = collider.gameObject.GetComponent<Weapon>();
        }
        else
        {
            Debug.Log(collider.gameObject.tag);
        }
    }
}
