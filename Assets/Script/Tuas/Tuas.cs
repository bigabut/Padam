using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections; // wajib untuk coroutine

public class Tuas : MonoBehaviour, IPointerDownHandler
{   
    public GameObject upTuas;
    public GameObject downTuas;
    public GameObject tuasUI;

    void Start()
    {
        upTuas.SetActive(true);
        downTuas.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {   
        upTuas.SetActive(false);
        downTuas.SetActive(true);
        Debug.Log("Tuas diklik, tunggu 1 detik...");
        StartCoroutine(DelayAction());
    }

    private IEnumerator DelayAction()
    {
        yield return new WaitForSeconds(2f); 
        tuasUI.SetActive(false);
        
        Debug.Log("Kode dijalankan setelah delay!");
    }
}