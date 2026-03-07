using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction Settings")]
    public Transform generator;           
    public GameObject generatorCanvas;    
    public GameObject mainGameUI;         
    public float interactDistance = 2f; 
    private bool taskFixGenerator = false;  

    private PlayerMovement playerMovement; // contoh script movement

    private void Start()
    {
        generatorCanvas.SetActive(false);
        playerMovement = GetComponent<PlayerMovement>(); // drag script movement player
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, generator.position);

        if (distance <= interactDistance)
        {
            if (Input.GetKeyDown(KeyCode.E) && !taskFixGenerator)
            {
                generatorCanvas.SetActive(true);
                mainGameUI.SetActive(false);

                // Disable movement, bukan pause global
                if(playerMovement != null) playerMovement.enabled = false;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

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
    // kalau kamu disable movement script, aktifkan kembali di sini
    // contoh: playerMovement.enabled = true;

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    playerMovement.enabled = true;
    taskFixGenerator = true;
    Time.timeScale = 1f; // kalau kamu masih pakai pause global
}
}