using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{

    public Vector2 MovementInputVector {  get; private set; }

    public event Action OnJumpPressed;
    private void OnMove(InputValue value)
    {
        MovementInputVector = value.Get<Vector2>();
        //Debug.Log(value.Get<Vector2>());
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed) OnJumpPressed?.Invoke();
    }
}
