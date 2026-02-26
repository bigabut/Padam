using UnityEngine;
using UnityEngine.AI;

public class Heartbeat : MonoBehaviour
{
    public GameObject point1;
    public GameObject point2;
    public AudioSource heartbeat;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(point1.transform.position, point2.transform.position);
        Debug.Log(distance);

        if (distance > 20)
        {
            heartbeat.pitch = 1f;
        }
        else if (distance < 10)
        {
            heartbeat.pitch = 3f;
        }
        else
        {
            heartbeat.pitch = 2f;
        }
    }
}
