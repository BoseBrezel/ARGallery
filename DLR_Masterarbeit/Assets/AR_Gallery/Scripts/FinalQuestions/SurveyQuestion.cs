using UnityEngine;
using TMPro;

public class SurveyQuestion : MonoBehaviour
{
    // ╭───────────────────────────────── ♡
    // │    SurveyQuestion
    // ╰───────────────────────────────── ♡

    // ── ♡  Antwortfelder  ♡ ──

    [SerializeField] private GameObject field1;
    [SerializeField] private GameObject field2;
    [SerializeField] private GameObject field3;
    [SerializeField] private GameObject field4;
    [SerializeField] private GameObject field5;

    // ── ♡  Ausgewählte Felder  ♡ ──

    [SerializeField] private GameObject seletecedField1;
    [SerializeField] private GameObject seletecedField2;
    [SerializeField] private GameObject seletecedField3;
    [SerializeField] private GameObject seletecedField4;
    [SerializeField] private GameObject seletecedField5;

    // ── ♡  Frage  ♡ ──

    [SerializeField] private TMP_Text question;

    // ── ♡  Auswahl  ♡ ──

    [SerializeField] private string seletecedField;


    // ╭───────────────────────────────── ♡
    // │    Initialisierung
    // ╰───────────────────────────────── ♡

    private void Awake()
    {
        SetBool(false);
    }


    // ╭───────────────────────────────── ♡
    // │    Antwort auswählen
    // ╰───────────────────────────────── ♡

    public void SetSelectdField(string field)
    {
        Debug.Log("SelectField wurde aufgerufen");

        seletecedField = field;

        Debug.Log("test " + field);

        SetBool(false);

        switch(seletecedField)
        {
            case "1":
                seletecedField1.SetActive(true);
                break;

            case "2":
                seletecedField2.SetActive(true);
                break;

            case "3":
                seletecedField3.SetActive(true);
                break;

            case "4":
                seletecedField4.SetActive(true);
                break;

            case "5":
                seletecedField5.SetActive(true);
                break;
        }
    }


    // ╭───────────────────────────────── ♡
    // │    Frage
    // ╰───────────────────────────────── ♡

    public void SetQuestion(string questions)
    {
        question.text = questions;
    }

    public string GetQuestion()
    {
        return question.text;
    }


    // ╭───────────────────────────────── ♡
    // │    Auswahl auslesen
    // ╰───────────────────────────────── ♡

    public string GetSelectdField()
    {
        return seletecedField;
    }


    // ╭───────────────────────────────── ♡
    // │    Auswahlfelder verwalten
    // ╰───────────────────────────────── ♡

    public void SetBool(bool state)
    {
        if(seletecedField1 != null)
        {
            seletecedField1.SetActive(state);
        }

        if(seletecedField2 != null)
        {
            seletecedField2.SetActive(state);
        }

        if(seletecedField3 != null)
        {
            seletecedField3.SetActive(state);
        }

        if(seletecedField4 != null)
        {
            seletecedField4.SetActive(state);
        }

        if(seletecedField5 != null)
        {
            seletecedField5.SetActive(state);
        }
    }
}