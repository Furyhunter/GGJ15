/*
Copyright (c) 2014, Unity Technologies
Copyright (c) 2015, Ronald Kinard <ronkinard93@gmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using UnityEngine;
using UnityEngine.EventSystems;
using InControl;
using System.Collections;
using System;

[AddComponentMenu("Event/InControl Input Module")]
public class InControlInputModule : PointerInputModule
{
    private float m_NextAction;

    [SerializeField]
    private bool m_AllowActivationOnMobileDevice;

    private Vector2 m_LastMousePosition = new Vector2();
    private Vector2 m_MousePosition = new Vector2();

    [SerializeField]
    private int m_ActionsPerSecond = 10;

    [Range(0, 16)]
    [Tooltip("The device index (plus 1) to use. 0 is ActiveDevice")]
    public int InControlDeviceNumber = 0;

    [SerializeField]
    private InputControlType m_SubmitControl = InputControlType.Action1;

    [SerializeField]
    private InputControlType m_CancelControl = InputControlType.Action2;

    [SerializeField]
    private InputControlType m_HorizontalControl = InputControlType.LeftStickX;

    [SerializeField]
    private InputControlType m_VerticalControl = InputControlType.LeftStickY;

    [Tooltip("Also allow the Dpad to control movement")]
    public bool AllowDPadControl = true;

    public int ActionsPerSecond
    {
        get { return m_ActionsPerSecond; }
        set { m_ActionsPerSecond = value; }
    }

    public bool AllowActivationOnMobileDevice
    {
        get { return m_AllowActivationOnMobileDevice; }
        set { m_AllowActivationOnMobileDevice = value; }
    }

    public InputControlType SubmitControlType
    {
        get { return m_SubmitControl; }
        set { m_SubmitControl = value; }
    }

    public InputControlType CancelControlType
    {
        get { return m_CancelControl; }
        set { m_CancelControl = value; }
    }

    public InputControlType HorizontalControlType
    {
        get { return m_HorizontalControl; }
        set { m_HorizontalControl = value; }
    }

    public InputControlType VerticalControlType
    {
        get { return m_VerticalControl; }
        set { m_VerticalControl = value; }
    }

    public InputDevice ActiveDevice
    {
        get
        {
            if (InControlDeviceNumber == 0)
            {
                return InputManager.ActiveDevice;
            }
            else
            {
                return InputManager.Devices[InControlDeviceNumber - 1];
            }
        }
    }

    public override void UpdateModule()
    {
        base.UpdateModule();
        m_LastMousePosition = m_MousePosition;
        m_MousePosition = Input.mousePosition;
    }

    public override bool IsModuleSupported()
    {
        // Check for mouse presence instead of whether touch is supported,
        // as you can connect mouse to a tablet and in that case we'd want
        // to use StandaloneInputModule for non-touch input events.
        return m_AllowActivationOnMobileDevice || Input.mousePresent;
    }

    public override void ActivateModule()
    {
        base.ActivateModule();
        m_MousePosition = Input.mousePosition;
        m_LastMousePosition = Input.mousePosition;

        var toSelect = eventSystem.currentSelectedGameObject;
        if (toSelect == null)
            toSelect = eventSystem.lastSelectedGameObject;
        if (toSelect == null)
            toSelect = eventSystem.firstSelectedGameObject;

        eventSystem.SetSelectedGameObject(toSelect, GetBaseEventData());

        if (FindObjectOfType<InControlManager>() == null)
        {
            Debug.LogWarning("InControlManager was not found in scene, and gamepad input will not work in the UI.");
        }
    }

    public override bool ShouldActivateModule()
    {
        if (!base.ShouldActivateModule())
        {
            return false;
        }

        var shouldActivate = ActiveDevice.GetControl(m_SubmitControl).IsPressed;
        shouldActivate |= ActiveDevice.GetControl(m_CancelControl).IsPressed;
        shouldActivate |= !Mathf.Approximately(ActiveDevice.GetControl(m_HorizontalControl).Value, 0.0f);
        shouldActivate |= !Mathf.Approximately(ActiveDevice.GetControl(m_VerticalControl).Value, 0.0f);
        if (AllowDPadControl)
        {
            shouldActivate |= !Mathf.Approximately(ActiveDevice.DPadX, 0.0f);
            shouldActivate |= !Mathf.Approximately(ActiveDevice.DPadY, 0.0f);
        }
        shouldActivate |= (m_MousePosition - m_LastMousePosition).sqrMagnitude > 0.0f;
        shouldActivate |= Input.GetMouseButtonDown(0);
        return shouldActivate;
    }

    public override void DeactivateModule()
    {
        base.DeactivateModule();
        ClearSelection();
    }

    public override void Process()
    {
        bool usedEvent = SendUpdateEventToSelectedObject();

        if (eventSystem.sendNavigationEvents)
        {
            if (!usedEvent)
                usedEvent |= SendMoveEventToSelectedObject();

            if (!usedEvent)
                SendSubmitEventToSelectedObject();
        }

        ProcessMouseEvent();
    }

    private bool SendSubmitEventToSelectedObject()
    {
        if (eventSystem.currentSelectedGameObject == null)
            return false;

        var data = GetBaseEventData();
        if (ActiveDevice.GetControl(m_SubmitControl).IsPressed)
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.submitHandler);

        if (ActiveDevice.GetControl(m_CancelControl).IsPressed)
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.cancelHandler);
        return data.used;
    }

    private bool m_hWasPressed;
    private bool m_vWasPressed;

    private bool AllowMoveEventProcessing(float time)
    {
        m_hWasPressed = !m_hWasPressed && ActiveDevice.GetControl(m_HorizontalControl).IsPressed != m_hWasPressed;
        m_vWasPressed = !m_vWasPressed && ActiveDevice.GetControl(m_VerticalControl).IsPressed != m_vWasPressed;

        if (AllowDPadControl)
        {
            m_hWasPressed = !m_hWasPressed && ActiveDevice.GetControl(m_HorizontalControl).IsPressed != m_hWasPressed;
            m_vWasPressed = !m_vWasPressed && ActiveDevice.GetControl(m_VerticalControl).IsPressed != m_vWasPressed;
        }
        bool allow = ActiveDevice.GetControl(m_HorizontalControl).WasPressed;
        allow |= ActiveDevice.GetControl(m_VerticalControl).WasPressed;
        if (AllowDPadControl)
        {
            allow |= ActiveDevice.GetControl(InputControlType.DPadLeft).WasPressed;
            allow |= ActiveDevice.GetControl(InputControlType.DPadRight).WasPressed;
            allow |= ActiveDevice.GetControl(InputControlType.DPadUp).WasPressed;
            allow |= ActiveDevice.GetControl(InputControlType.DPadDown).WasPressed;
        }
        allow |= (time > m_NextAction);
        return allow;
    }

    private Vector2 GetRawMoveVector()
    {
        Vector2 move = Vector2.zero;
        move.x = ActiveDevice.GetControl(m_HorizontalControl).Value;
        move.y = ActiveDevice.GetControl(m_VerticalControl).Value;

        if (AllowDPadControl)
        {
            move.x += ActiveDevice.DPadX;
            move.y += ActiveDevice.DPadY;
        }

        if (ActiveDevice.GetControl(m_HorizontalControl).Value != 0)
        {
            if (move.x < 0)
                move.x = -1f;
            if (move.x > 0)
                move.x = 1f;
        }
        if (ActiveDevice.GetControl(m_VerticalControl).Value != 0)
        {
            if (move.y < 0)
                move.y = -1f;
            if (move.y > 0)
                move.y = 1f;
        }

        if (AllowDPadControl)
        {
            if (!Mathf.Approximately(ActiveDevice.DPadX, 0.0f))
            {
                if (move.x < 0)
                    move.x = -1f;
                if (move.x > 0)
                    move.x = 1f;
            }
            if (!Mathf.Approximately(ActiveDevice.DPadY, 0.0f))
            {
                if (move.y < 0)
                    move.y = -1f;
                if (move.y > 0)
                    move.y = 1f;
            }
        }
        return move;
    }

    private bool SendMoveEventToSelectedObject()
    {
        float time = Time.unscaledTime;

        if (!AllowMoveEventProcessing(time))
            return false;

        Vector2 movement = GetRawMoveVector();
        var axisEventData = GetAxisEventData(movement.x, movement.y, 0.6f);
        if (!Mathf.Approximately(axisEventData.moveVector.x, 0f)
            || !Mathf.Approximately(axisEventData.moveVector.y, 0f))
        {
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
        }
        m_NextAction = time + 1f / m_ActionsPerSecond;
        return axisEventData.used;
    }

    private bool SendUpdateEventToSelectedObject()
    {
        if (eventSystem.currentSelectedGameObject == null)
            return false;

        var data = GetBaseEventData();
        ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
        return data.used;
    }

    private void ProcessMouseEvent()
    {
        var mouseData = GetMousePointerEventData();

        var pressed = mouseData.AnyPressesThisFrame();
        var released = mouseData.AnyReleasesThisFrame();

        var leftButtonData = mouseData.GetButtonState(PointerEventData.InputButton.Left).eventData;

        if (!UseMouse(pressed, released, leftButtonData.buttonData))
            return;

        // Process the first mouse button fully
        ProcessMousePress(leftButtonData);
        ProcessMove(leftButtonData.buttonData);
        ProcessDrag(leftButtonData.buttonData);

        // Now process right / middle clicks
        ProcessMousePress(mouseData.GetButtonState(PointerEventData.InputButton.Right).eventData);
        ProcessDrag(mouseData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
        ProcessMousePress(mouseData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
        ProcessDrag(mouseData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);

        if (!Mathf.Approximately(leftButtonData.buttonData.scrollDelta.sqrMagnitude, 0.0f))
        {
            var scrollHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(leftButtonData.buttonData.pointerCurrentRaycast.gameObject);
            ExecuteEvents.ExecuteHierarchy(scrollHandler, leftButtonData.buttonData, ExecuteEvents.scrollHandler);
        }
    }

    private static bool UseMouse(bool pressed, bool released, PointerEventData pointerData)
    {
        if (pressed || released || pointerData.IsPointerMoving() || pointerData.IsScrolling())
            return true;

        return false;
    }

    private void ProcessMousePress(MouseButtonEventData data)
    {
        var pointerEvent = data.buttonData;
        var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;

        // PointerDown notification
        if (data.PressedThisFrame())
        {
            pointerEvent.eligibleForClick = true;
            pointerEvent.delta = Vector2.zero;
            pointerEvent.dragging = false;
            pointerEvent.useDragThreshold = true;
            pointerEvent.pressPosition = pointerEvent.position;
            pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

            DeselectIfSelectionChanged(currentOverGo, pointerEvent);

            // search for the control that will receive the press
            // if we can't find a press handler set the press
            // handler to be what would receive a click.
            var newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);

            // didnt find a press handler... search for a click handler
            if (newPressed == null)
                newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

            // Debug.Log("Pressed: " + newPressed);

            float time = Time.unscaledTime;

            if (newPressed == pointerEvent.lastPress)
            {
                var diffTime = time - pointerEvent.clickTime;
                if (diffTime < 0.3f)
                    ++pointerEvent.clickCount;
                else
                    pointerEvent.clickCount = 1;

                pointerEvent.clickTime = time;
            }
            else
            {
                pointerEvent.clickCount = 1;
            }

            pointerEvent.pointerPress = newPressed;
            pointerEvent.rawPointerPress = currentOverGo;

            pointerEvent.clickTime = time;

            // Save the drag handler as well
            pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentOverGo);

            if (pointerEvent.pointerDrag != null)
                ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
        }

        // PointerUp notification
        if (data.ReleasedThisFrame())
        {
            // Debug.Log("Executing pressup on: " + pointer.pointerPress);
            ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

            // Debug.Log("KeyCode: " + pointer.eventData.keyCode);

            // see if we mouse up on the same element that we clicked on...
            var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

            // PointerClick and Drop events
            if (pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick)
            {
                ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
            }
            else if (pointerEvent.pointerDrag != null)
            {
                ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.dropHandler);
            }

            pointerEvent.eligibleForClick = false;
            pointerEvent.pointerPress = null;
            pointerEvent.rawPointerPress = null;

            if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
                ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);

            pointerEvent.dragging = false;
            pointerEvent.pointerDrag = null;

            // redo pointer enter / exit to refresh state
            // so that if we moused over somethign that ignored it before
            // due to having pressed on something else
            // it now gets it.
            if (currentOverGo != pointerEvent.pointerEnter)
            {
                HandlePointerExitAndEnter(pointerEvent, null);
                HandlePointerExitAndEnter(pointerEvent, currentOverGo);
            }
        }
    }
}
