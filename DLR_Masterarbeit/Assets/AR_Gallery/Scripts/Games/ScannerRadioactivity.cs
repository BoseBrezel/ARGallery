using UnityEngine;
using TMPro;

public class ScannerRadioactivity : MonoBehaviour
{
    public TMP_Text radiationText;
    public TMP_Text nameText;

    private Radioactivity currentRadioactivity;

    private void OnTriggerEnter(Collider other)
    {
        Radioactivity radioactivityBox = other.GetComponent<Radioactivity>();

        if(radioactivityBox != null)
        {
            currentRadioactivity = radioactivityBox;
            radiationText.text = radioactivityBox.radiationValue.ToString("0.###") + " " + radioactivityBox.unit;
            nameText.text = radioactivityBox.materialName;
        }
        else
        {
            Debug.Log("Das aktuelle Objekt ist nicht radioaktiv");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Radioactivity radioactivityBox = other.GetComponent<Radioactivity>();

        if(radioactivityBox != null && radioactivityBox == currentRadioactivity)
        {
            currentRadioactivity = null;
            radiationText.text = "0 µSv/h";
            nameText.text = "---------------";
        }
    }
}