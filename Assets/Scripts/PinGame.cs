using UnityEngine;
using UnityEngine.UI; // kalau pakai Text biasa
// kalau pakai TextMeshPro, ganti dengan: using TMPro;

public class PinGame : MonoBehaviour
{
    // kalau pakai Text biasa:
    public Text displayText;

    // kalau pakai TextMeshPro:
    // public TextMeshProUGUI displayText;

    public string correctPIN = "1234"; // PIN yang benar
    private string inputPIN = "";

    // Fungsi ini dipanggil dari tombol keypad
    public void OnKeyPress(string key)
    {
        if (key == "#") // tombol submit
        {
            if (inputPIN == correctPIN)
            {
                displayText.text = "BERHASIL\nPIN: " + inputPIN;
            }
            else
            {
                displayText.text = "PIN SALAH";
            }
            inputPIN = ""; // reset setelah submit
        }
        else if (key == "*") // tombol reset
        {
            inputPIN = "";
            displayText.text = "Reset PIN";
        }
        else if (key == "DEL") // tombol delete
        {
            if (inputPIN.Length > 0)
            {
                inputPIN = inputPIN.Substring(0, inputPIN.Length - 1);
                displayText.text = inputPIN;
            }
        }
        else // tombol angka 0–9
        {
            // batasi maksimal 4 digit
            if (inputPIN.Length < 4)
            {
                inputPIN += key;
                displayText.text = inputPIN;
            }
        }
    }
}