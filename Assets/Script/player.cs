using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;   // bisa diubah dari Inspector
    Rigidbody2D myRB;
    Vector2 movementInput;

    InputAction moveAction;

    void Awake()
    {
        // Setup input WASD
        moveAction = new InputAction(type: InputActionType.Value, expectedControlType: "Vector2");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
    }

    void OnEnable() => moveAction.Enable();
    void OnDisable() => moveAction.Disable();

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        if (myRB == null)
            Debug.LogError("Rigidbody2D tidak ditemukan di GameObject ini!");
    }

    void Update()
    {
        movementInput = moveAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        if (myRB != null)
            myRB.linearVelocity = movementInput * speed;
    }
}   