using UnityEngine;
using System.Collections;

public class FadeCanvasController : MonoBehaviour
{
    Animator animator;

    public bool Closed = false;

    public bool ToggleOnAwake = true;

    private bool toggledOnAwake = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Closed", Closed);
        if (!animator)
        {
            Debug.LogWarning("FadeCanvasController not attached to object with an Animator! Disabling.", this);
            enabled = false;
        }
    }

	void Update()
	{
	    if (animator)
        {
            animator.SetBool("Closed", Closed);
            if (ToggleOnAwake && !toggledOnAwake)
            {
                toggledOnAwake = true;
                Closed = !Closed;
            }
        }
	}
}
