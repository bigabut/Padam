 using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract: MonoBehaviour
{
    [Header("Referensi Objek")]
    public GameObject obeng;    
    public GameObject baterai;    
    public GameObject mainGameUI;   
    public GameObject hintPaperUI;
    public GameObject GeneratorUI;
    public GameObject generator;
    public GameObject jerigen;
    public GameObject pinUI;
    public GameObject pin;
    public GameObject tuasUI;
    public GameObject tuas;
    public Transform interactFixCable;    

    public GameObject fixCable;   
    public GameObject hintPaper;

    public Generator generatorTask;

    [Header("Pengaturan")]
    public float interactDistance = 2f;

    private bool isTriggerFixCable = false;
    public bool taskFixCable = false;


    private bool isTriggerGenerator = false;
    public bool taskGenerator = false;
    public bool taskPin = false;

    
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
               
            }
        }

        float distanceBaterai = Vector2.Distance(transform.position, baterai.transform.position);

        if (distanceBaterai <= interactDistance)
        {
            if (interactAction.WasPressedThisFrame())
            {
                baterai.SetActive(false);
               
            }
        }
         float distanceJerigen = Vector2.Distance(transform.position, jerigen.transform.position);

        if (distanceJerigen <= interactDistance)
        {
            if (interactAction.WasPressedThisFrame())
            {
                jerigen.SetActive(false);
                isTriggerGenerator = true;
               
            }
        }
        float distanceGenerator = Vector2.Distance(transform.position, generator.transform.position);

        if (distanceGenerator <= interactDistance && isTriggerGenerator && !taskGenerator)
        {
            if (interactAction.WasPressedThisFrame())
            {
                GeneratorUI.SetActive(true);
                generatorTask.missionTimer = generatorTask.missionTime;
               
            }
        }
           
        float distancePin = Vector2.Distance(transform.position, pin.transform.position);

        if (distancePin <= interactDistance )
        {
            if (interactAction.WasPressedThisFrame() && !taskPin)
            {
                pinUI.SetActive(true);
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
    public void CloseFixCableUILose()
    {
        fixCable.SetActive(false);
        mainGameUI.SetActive(true);

        if (playerMovement != null) playerMovement.enabled = true;

        Time.timeScale = 1f; 
    }

    public void CloseGeneratorUI()
    {
        GeneratorUI.SetActive(false);
        taskGenerator = true;
    }
    public void CloseGeneratorUILose()
    {
        GeneratorUI.SetActive(false);
    }
    
    public void closePinUI()
    {
        pinUI.SetActive(false);
        taskPin = true;
    }

    public void closePinUILose()
    {
        pinUI.SetActive(false);
    }
}