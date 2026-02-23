

using UnityEngine;

public class poweredwirebehaviour : MonoBehaviour {
    


    bool mouseDown =  false;
    public poweredwirestats powerWireS;
    void Start()
    {
        powerWireS = gameObject.GetComponent<poweredwirestats>();  
    }

    void Update()
    {
        MouseWire(); 
    }

    void OnMouseDown() {
        mouseDown = true;    
    }

    void OnMouseOver() {
        powerWireS.movable = true;    
    }
    void OnMouseExit() {
        if (!powerWireS.moving)
        {
            powerWireS.movable = false;  
        }       

    }
    void OnMouseUp()
    {
        mouseDown = false;
        gameObject.transform.position = powerWireS.startPosition;

    }

    void MouseWire()
    {
        if(mouseDown && powerWireS.movable)
        {
            
            powerWireS.move = true;
            float mouseX = input.mousePosition.x;               
            float mouseY = input.mousePosition.y;

            gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mouseX,mouseY, transform.position.z  )); 
        }
        else 
            powerWireS.moving = false; 
        }
}

