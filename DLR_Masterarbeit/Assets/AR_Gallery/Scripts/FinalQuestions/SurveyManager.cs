using UnityEngine;
using TMPro;

public class SurveyManager : MonoBehaviour
{
    // ╭───────────────────────────────── ♡
    // │    SurveyManager
    // ╰───────────────────────────────── ♡

    // ── ♡  App Komponenten  ♡ ──

    [SerializeField] private Data data;
    [SerializeField] private EndQuizHandler endQuizHandler;

    // ── ♡  Survey  ♡ ──

    [SerializeField] private SurveyQuestion[] surveys;

    // ── ♡  UI Elemente  ♡ ──

    [SerializeField] private GameObject errorGameObject;
    [SerializeField] private GameObject finalFinish;
    [SerializeField] private TMP_InputField freeTextField;

    // ── ♡  Tastatur  ♡ ──

    private bool keyboardOpen = false;


    // ╭───────────────────────────────── ♡
    // │    Initialisierung
    // ╰───────────────────────────────── ♡

    private void Awake()
    {
        if(errorGameObject != null)
        {
            errorGameObject.SetActive(false);
        }
    }


    // ╭───────────────────────────────── ♡
    // │    Survey prüfen
    // ╰───────────────────────────────── ♡

    public void CheckSurvey()
    {
        foreach(SurveyQuestion survey in surveys)
        {
            if(survey == null)
            {
                continue;
            }

            if(string.IsNullOrEmpty(survey.GetSelectdField()))
            {
                if(errorGameObject != null)
                {
                    errorGameObject.SetActive(true);
                }

                Debug.Log("Nicht alle Fragen wurden beantwortet!");
                return;
            }
        }

        if(errorGameObject != null)
        {
            errorGameObject.SetActive(false);
        }

        Debug.Log("Alle Fragen wurden beantwortet!");

        FinishSurvey();
    }


    // ╭───────────────────────────────── ♡
    // │    Survey abschließen
    // ╰───────────────────────────────── ♡

    public void FinishSurvey()
    {
        Debug.Log("Survey ist vollständig.");

        // ── ♡  EndQuizHandler prüfen  ♡ ──

        if(endQuizHandler == null)
        {
            Debug.LogError("EndQuizHandler wurde im SurveyManager nicht gesetzt!");
            return;
        }

        finalFinish.SetActive(true);
        endQuizHandler.SaveResult();
    }


    // ╭───────────────────────────────── ♡
    // │    Survey-Antworten speichern
    // ╰───────────────────────────────── ♡

    public void SaveSurveyAnswers(SurveyResultData survey)
    {
        if(survey == null)
        {
            Debug.LogError("Keine Umfrage vorhanden!");
            return;
        }

        survey.surveyAnswers.Clear();

        // ── ♡  Survey-Fragen speichern  ♡ ──

        foreach(SurveyQuestion surveyQuestion in surveys)
        {
            if(surveyQuestion == null)
            {
                continue;
            }

            AnswerData answerData = new AnswerData();

            answerData.question = surveyQuestion.GetQuestion();
            answerData.answer = surveyQuestion.GetSelectdField();

            survey.surveyAnswers.Add(answerData);
        }

        // ── ♡  Freitext speichern  ♡ ──

        if(freeTextField != null)
        {
            survey.freeText = freeTextField.text;
        }

        Debug.Log("Survey-Antworten und Freitext wurden übernommen.");
    }


    // ╭───────────────────────────────── ♡
    // │    Freitext
    // ╰───────────────────────────────── ♡

    public string GetFreeText()
    {
        if(freeTextField == null)
        {
            return "";
        }

        return freeTextField.text;
    }


    // ╭───────────────────────────────── ♡
    // │    Tastatur
    // ╰───────────────────────────────── ♡

    public void ToggleKeyboard()
    {
        if(freeTextField == null)
        {
            return;
        }

        if(!keyboardOpen)
        {
            freeTextField.ActivateInputField();
            freeTextField.Select();

            keyboardOpen = true;
        }
        else
        {
            freeTextField.DeactivateInputField();

            TouchScreenKeyboard.hideInput = true;

            keyboardOpen = false;
        }
    }
}