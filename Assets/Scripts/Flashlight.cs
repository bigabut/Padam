using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Light2D flashlight;
    [SerializeField] BoxCollider2D Detector;
    [SerializeField] CinemachineVirtualCamera shake;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        flashlight = GetComponent<Light2D>();
        Detector = GetComponent<BoxCollider2D>();
        shake = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("monster"))
        {
            
        }
    }
}
