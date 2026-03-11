 using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("Referensi Objek")]
    public GameObject obeng;    
    public GameObject baterai;    
    public GameObject mainGameUI;   
    public GameObject hintPaperUI;
    public GameObject tuasUI;
    public GameObject tuas;
    public Transform interactFixCable;    

    public GameObject fixCable;   
    public GameObject hintPaper;

    [Header("Pengaturan")]
    public float interactDistance = 2f;

    private bool isTriggerFixCable = false;
    public bool taskFixCable = false;
    public PlayerMovement playerMovement;

    // Input System
    private InputAction interactAction;

    private void Awake()
    {
        // Binding tombol E untuk interaksi
        interactAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/e");
    }

    private void OnEnable() => interactAction.Enable();
    private void OnDisable() => interactAction.Disable();

    private void Start()
    {
        fixCable.SetActive(false);
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        float distanceFixCable = Vector2.Distance(transform.position, interactFixCable.position);

        if (distanceFixCable <= interactDistance && isTriggerFixCable)
        {
            if (interactAction.WasPressedThisFrame() && !taskFixCable)
            {
                fixCable.SetActive(true);
                mainGameUI.SetActive(false);

                if (playerMovement != null) playerMovement.enabled = false;
            }
        }

        float distanceObeng = Vector2.Distance(transform.position, obeng.transform.position);

        if (distanceObeng <= interactDistance)
        {
            if (interactAction.WasPressedThisFrame())
            {
                obeng.SetActive(false); 
                isTriggerFixCable = true;
            }
        }

        float distanceHintPaper = Vector2.Distance(transform.position, hintPaper.transform.position);

        if (distanceHintPaper <= interactDistance)
        {
            if (interactAction.WasPressedThisFrame())
            {
                hintPaperUI.SetActive(true);
                playerMovement.enabled = false;
            }
        }

        float distanceTuas = Vector2.Distance(transform.position, tuas.transform.position);

        if (distanceTuas <= interactDistance)
        {
            if (interactAction.WasPressedThisFrame())
            {
                tuasUI.SetActive(true);
               // playerMovement.enabled = false;
            }
        }

        float distanceBaterai = Vector2.Distance(transform.position, baterai.transform.position);

        if (distanceBaterai <= interactDistance)
        {
            if (interactAction.WasPressedThisFrame())
            {
                baterai.SetActive(false);
               // playerMovement.enabled = false;
            }
        }


        
    }

    public void CloseFixCableUI()
    {
        fixCable.SetActive(false);
        mainGameUI.SetActive(true);

        if (playerMovement != null) playerMovement.enabled = true;

        taskFixCable = true;
        Time.timeScale = 1f; 
    }
}