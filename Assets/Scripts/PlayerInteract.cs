using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction Settings")]
    public Transform generator;           // Drag object yang bisa di-interact
    public GameObject generatorCanvas;    // Drag Canvas mini task
    public GameObject mainGameUI;         // Drag UI utama
    public float interactDistance = 2f;   // Jarak interaksi
     // Text "Press E to interact"

    private void Start()
    {
        // Pastikan Canvas dan prompt tidak aktif di awal
        generatorCanvas.SetActive(false);
        
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, generator.position);

        // Kalau dekat generator
        if (distance <= interactDistance)
        {
          

            if (Input.GetKeyDown(KeyCode.E))
            {
                generatorCanvas.SetActive(true);
                mainGameUI.SetActive(false);

                // Biar UI bisa diklik
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Time.timeScale = 0f;
            }
        }
        
        // Kalau Canvas aktif dan tekan Escape
        if (generatorCanvas.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            generatorCanvas.SetActive(false);
            mainGameUI.SetActive(true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f;
        }
    }
}