using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AU_PlayerController : MonoBehaviour
{
    // Components
    Rigidbody myRB;
    Transform myAvatar;

    // Player movement
    [SerializeField] InputAction WASD;
    Vector2 movementInput;

    [SerializeField] float baseSpeed = 5f;       // kecepatan awal
    [SerializeField] float maxSpeed = 12f;       // batas maksimal
    [SerializeField] float acceleration = 0.2f;  // seberapa cepat nambah speed

    float currentSpeed;

    private void OnEnable()
    {
        WASD.Enable();
    }

    private void OnDisable()
    {
        WASD.Disable();
    }

    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myAvatar = transform.GetChild(0);
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        movementInput = WASD.ReadValue<Vector2>();

        // Flip avatar sesuai arah X
        if (movementInput.x != 0)
        {
            myAvatar.localScale = new Vector2(Mathf.Sign(movementInput.x), 1);
        }

        // Jika ada input, tambah speed sampai max
        if (movementInput != Vector2.zero)
        {
            currentSpeed = Mathf.Min(currentSpeed + acceleration, maxSpeed);
        }
        else
        {
            // Kalau tidak ada input, reset ke baseSpeed
            currentSpeed = baseSpeed;
        }
    }

    private void FixedUpdate()
    {
        myRB.linearVelocity = movementInput * currentSpeed;
    }
}
