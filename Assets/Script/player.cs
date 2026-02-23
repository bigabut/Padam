 using UnityEngine;
using UnityEngine.InputSystem;

public class AU_PlayerController : MonoBehaviour
{
    Rigidbody2D myRB;
    Vector2 movementInput;
    float speed = 5f;

    InputAction moveAction;

    void Awake()
    {
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