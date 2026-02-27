using UnityEngine;


public class WireManager : MonoBehaviour {
    public int totalWire= 4;
    int connectedWire = 0;
    public bool Done = false;

    public void WireConnected()
    {
        connectedWire++;

        if(connectedWire>= totalWire && Done == false)
        {
            Debug.Log("Semua kabel terpasang");
            Done = true;
        }
    }


}