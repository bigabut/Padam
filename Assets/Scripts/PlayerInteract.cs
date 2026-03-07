using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction Settings")]
    public Transform generator;           
    public GameObject generatorCanvas;    
    public GameObject mainGameUI;   
    public GameObject triggerGenerator;   // ubah ke GameObject
    public float interactDistance = 2f; 
    private bool taskFixGenerator = false;  

    private bool isTriggerGenerator = false;
    private PlayerMovement playerMovement; // contoh script movement

    private void Start()
    {
        generatorCanvas.SetActive(false);
        playerMovement = GetComponent<PlayerMovement>(); // drag script movement player
    }

    private void Update()
    {
        // cek jarak ke generator
        float distanceGenerator = Vector2.Distance(transform.position, generator.position);

        if (distanceGenerator <= interactDistance && isTriggerGenerator)
        {
            if (Input.GetKeyDown(KeyCode.E) && !taskFixGenerator)
            {
                generatorCanvas.SetActive(true);
                mainGameUI.SetActive(false);

                // Disable movement, bukan pause global
                if(playerMovement != null) playerMovement.enabled = false;
            }
        }

        // cek jarak ke trigger (misalnya oil barrel)
        float distanceOil = Vector2.Distance(transform.position, triggerGenerator.transform.position);

        if (distanceOil <= interactDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                triggerGenerator.SetActive(false); // bisa di-disable sekarang
                isTriggerGenerator = true;
            }
        }

        // tutup UI generator dengan ESC
        if (generatorCanvas.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            generatorCanvas.SetActive(false);
            mainGameUI.SetActive(true);

            if(playerMovement != null) playerMovement.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
    }

    public void CloseGeneratorUI()
    {
        generatorCanvas.SetActive(false);
        mainGameUI.SetActive(true);

        // Aktifkan lagi kontrol player
        if(playerMovement != null) playerMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        taskFixGenerator = true;
        Time.timeScale = 1f; // kalau kamu masih pakai pause global
    }
}