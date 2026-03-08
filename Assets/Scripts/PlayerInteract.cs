using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
 
    public Transform generator;           
    public GameObject generatorCanvas;    
    public GameObject mainGameUI;   
    public GameObject triggerGenerator;    
    public float interactDistance = 2f; 
    private bool taskFixGenerator = false;  

    private bool isTriggerGenerator = false;
    private PlayerMovement playerMovement;  

    private void Start()
    {
        generatorCanvas.SetActive(false);
        playerMovement = GetComponent<PlayerMovement>();  
    }

    private void Update()
    {
       
        float distanceGenerator = Vector2.Distance(transform.position, generator.position);

        if (distanceGenerator <= interactDistance && isTriggerGenerator)
        {
            if (Input.GetKeyDown(KeyCode.E) && !taskFixGenerator)
            {
                generatorCanvas.SetActive(true);
               

               
                if(playerMovement != null) playerMovement.enabled = false;
            }
        }

       
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
        
    }

    public void CloseGeneratorUI()
    {
        generatorCanvas.SetActive(false);
        mainGameUI.SetActive(true);

      
        if(playerMovement != null) playerMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        taskFixGenerator = true;
        Time.timeScale = 1f; 
    }
}