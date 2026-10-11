using UnityEngine;
using TMPro;


// ╭───────────────────────────────── ♡
// │    Data
// ╰───────────────────────────────── ♡
public class Data : MonoBehaviour
{
    // ── ♡  Eingabefelder  ♡ ──
    [SerializeField] private TMP_Text ageInput;
    [SerializeField] private TMP_Dropdown genderDropdown;


    // ── ♡  UI Elemente  ♡ ──
    [SerializeField] private GameObject errorMesagge;
    [SerializeField] private GameObject finishAcitvate;
    [SerializeField] private GameObject finishClose;


    // ── ♡  Gespeicherte Daten  ♡ ──
    private int age = 0;
    private string gender = "";


    // ╭─────────────────────────────── ♡
    // │    Setter
    // ╰─────────────────────────────── ♡

    public void SetAge(int value)
    {
        age = value;
    }

    public void SetGender()
    {
        gender = genderDropdown.options[genderDropdown.value].text;
    }


    // ╭─────────────────────────────── ♡
    // │    Getter
    // ╰─────────────────────────────── ♡

    public int GetAge()
    {
        return age;
    }

    public string GetGender()
    {
        return gender;
    }

    public bool HasValidData()
    {
        return age > 0 && !string.IsNullOrEmpty(gender);
    }


    // ╭─────────────────────────────── ♡
    // │    Navigation
    // ╰─────────────────────────────── ♡

    public void GoNext()
    {
        SetGender();

        if(!HasValidData())
        {
            errorMesagge.SetActive(true);
            return;
        }

        errorMesagge.SetActive(false);
        finishAcitvate.SetActive(true);
        finishClose.SetActive(false);
    }
}