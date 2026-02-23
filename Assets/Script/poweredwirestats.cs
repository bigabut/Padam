using UnityEngine;

public enum Color { blue, red, yellow , green};
 

public class poweredwirestats : MonoBehaviour 
    

{
    public bool movable = false;
    public bool moving = false;
    public Vector3  startPosition;
    public Color objectColor;

    void start()
    {
        startPosition = transform.position;
    }

    void  Update() {
        
    }
}
