using UnityEngine;
using System.Collections;
using InControl;

public class PlayerController : MonoBehaviour
{
    InputDevice device;
    InputControl control;
    public float PlayerSpeed = 1.0f;
    private float TossDelay = 0.0f;
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
        TossDelay -= Time.deltaTime;

       // Debug.Log(device.Name);
        Vector3 y = device.LeftStickY * Camera.main.transform.forward * PlayerSpeed;
        Vector3 x = device.LeftStickX * Camera.main.transform.right * PlayerSpeed;
        x.y = 0;
        y.y = 0;
       Mover.SimpleMove(x + y);
        float Jumblies = Mathf.Max(x.magnitude, y.magnitude);
       gameObject.GetComponentInChildren<Animator>().SetFloat("Speed", Jumblies);
        x = device.RightStickX * Camera.main.transform.right ;
        y = device.RightStickY * Camera.main.transform.forward ;
        if (x != Vector3.zero || y != Vector3.zero)
        {
            x.y = 0;
            y.y = 0;
            transform.rotation = Quaternion.LookRotation(x+y, Vector3.up);
        }

        if (device.Action4 && TossDelay <= 0)
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
               TossDelay = 1;
           }
        }
	}

    void OnTriggerStay(Collider collider)

    {
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
