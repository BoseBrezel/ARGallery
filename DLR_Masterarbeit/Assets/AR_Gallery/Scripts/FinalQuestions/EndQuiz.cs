using UnityEngine;
using UnityEngine.UI;
using TMPro;


// ╭───────────────────────────────────────── ♡
// │    EndQuiz
// │    Basiert auf der bestehenden Quiz-Klasse.
// │    Logik mithilfe von KI angepasst.
// ╰───────────────────────────────────────── ♡
public class EndQuiz : MonoBehaviour
{
    // ╭─────────────────────────────── ♡
    // │    Fragen & Antworten
    // ╰─────────────────────────────── ♡

    // ── ♡  Frage  ♡ ──
    [SerializeField] TMP_Text question;


    // ── ♡  Antwort A  ♡ ──
    [SerializeField] TMP_Text answerA;
    [SerializeField] GameObject hoverA;
    [SerializeField] GameObject falseA;
    [SerializeField] GameObject trueA;
    [SerializeField] Button answerButtonA;


    // ── ♡  Antwort B  ♡ ──
    [SerializeField] TMP_Text answerB;
    [SerializeField] GameObject hoverB;
    [SerializeField] GameObject falseB;
    [SerializeField] GameObject trueB;
    [SerializeField] Button answerButtonB;


    // ── ♡  Antwort C  ♡ ──
    [SerializeField] TMP_Text answerC;
    [SerializeField] GameObject hoverC;
    [SerializeField] GameObject falseC;
    [SerializeField] GameObject trueC;
    [SerializeField] Button answerButtonC;


    // ── ♡  Antwort D  ♡ ──
    [SerializeField] TMP_Text answerD;
    [SerializeField] GameObject hoverD;
    [SerializeField] GameObject falseD;
    [SerializeField] GameObject trueD;


    [SerializeField] Button answerButtonD;


    // ── ♡  Antwort Weiß nicht  ♡ ──
    [SerializeField] GameObject hoverDontKnow;


    // ╭─────────────────────────────── ♡
    // │    Quizdaten
    // ╰─────────────────────────────── ♡

    private Button rightButton;
    private Button selectedButton = null;

    private string quizName;
    private string correctAnswer;
    private string selectedAnswer;


    // ╭─────────────────────────────── ♡
    // │    Initialisierung
    // ╰─────────────────────────────── ♡

    // ── ♡  Buttons verbinden  ♡ ──
    private void Awake()
    {
        answerButtonA.onClick.AddListener(() => SelectAnswer(answerButtonA, hoverA));
        answerButtonB.onClick.AddListener(() => SelectAnswer(answerButtonB, hoverB));
        answerButtonC.onClick.AddListener(() => SelectAnswer(answerButtonC, hoverC));
        answerButtonD.onClick.AddListener(() => SelectAnswer(answerButtonD, hoverD));
    }


    // ╭─────────────────────────────── ♡
    // │    Quiz vorbereiten
    // ╰─────────────────────────────── ♡

    // ── ♡  Quizdaten setzen  ♡ ──
    public void Setup(string name, QuizEntry data)
    {
        quizName = name;
        correctAnswer = data.answerRight;

        question.text = data.question;

        string[] answers = { data.answerRight,data.answer1,data.answer2,data.answer3};


        // ── ♡  Antworten mischen  ♡ ──
        for(int i = 0; i < answers.Length; i++)
        {
            int randomIndex = Random.Range(i, answers.Length);
            string temp = answers[i];
            answers[i] = answers[randomIndex];
            answers[randomIndex] = temp;
        }

        answerA.text = answers[0];
        answerB.text = answers[1];
        answerC.text = answers[2];
        answerD.text = answers[3];

        // ── ♡  Richtige Antwort bestimmen  ♡ ──
        if(answers[0] == correctAnswer)
        {
            rightButton = answerButtonA;
        }
        else if(answers[1] == correctAnswer)
        {
            rightButton = answerButtonB;
        }
        else if(answers[2] == correctAnswer)
        {
            rightButton = answerButtonC;
        }
        else
        {
            rightButton = answerButtonD;
        }

        selectedButton = null;
        selectedAnswer = "";

        ClearHover();
        ClearResult();

        EnableButtons();

        gameObject.SetActive(false);
    }


    // ╭─────────────────────────────── ♡
    // │    Antwortauswahl
    // ╰─────────────────────────────── ♡

    // ── ♡  Antwort auswählen  ♡ ──
    private void SelectAnswer(Button button, GameObject hover)
    {
        ClearHover();

        selectedButton = button;
        selectedAnswer = GetAnswerText(button);

        hover.SetActive(true);
    }


    // ── ♡  "Weiß nicht" auswählen  ♡ ──
    public void SelectDontKnow()
    {
        ClearHover();

        selectedButton = null;
        selectedAnswer = "";

        hoverDontKnow.SetActive(true);
    }


    // ── ♡  Antworttext abrufen  ♡ ──
    private string GetAnswerText(Button button)
    {
        if(button == answerButtonA)
        {
            return answerA.text;
        }

        if(button == answerButtonB)
        {
            return answerB.text;
        }

        if(button == answerButtonC)
        {
            return answerC.text;
        }

        return answerD.text;
    }

    public bool HasAnswer()
    {
        return selectedButton != null;
    }

    public bool IsCorrect()
    {
        return selectedButton == rightButton;
    }

    // ╭─────────────────────────────── ♡
    // │    Getter
    // ╰─────────────────────────────── ♡

    public string GetQuizName()
    {
        return quizName;
    }

    public string GetQuestion()
    {
        return question.text;
    }

    public string GetAnswerA()
    {
        return answerA.text;
    }

    public string GetAnswerB()
    {
        return answerB.text;
    }

    public string GetAnswerC()
    {
        return answerC.text;
    }

    public string GetAnswerD()
    {
        return answerD.text;
    }

    public string GetCorrectAnswer()
    {
        return correctAnswer;
    }

    public string GetSelectedAnswer()
    {
        return selectedAnswer;
    }


    // ╭─────────────────────────────── ♡
    // │    Ergebnis
    // ╰─────────────────────────────── ♡

    public void ShowResult()
    {
        if(!HasAnswer())
        {
            ShowCorrect(rightButton);
            DisableButtons();

            return;
        }

        if(IsCorrect())
        {
            ShowCorrect(selectedButton);
        }
        else
        {
            ShowWrong(selectedButton);
            ShowCorrect(rightButton);
        }

        DisableButtons();
    }

    private void ShowCorrect(Button button)
    {
        if(button == answerButtonA)
        {
            trueA.SetActive(true);
        }

        if(button == answerButtonB)
        {
            trueB.SetActive(true);
        }

        if(button == answerButtonC)
        {
            trueC.SetActive(true);
        }

        if(button == answerButtonD)
        {
            trueD.SetActive(true);
        }
    }

    private void ShowWrong(Button button)
    {
        if(button == answerButtonA)
        {
            falseA.SetActive(true);
        }

        if(button == answerButtonB)
        {
            falseB.SetActive(true);
        }

        if(button == answerButtonC)
        {
            falseC.SetActive(true);
        }

        if(button == answerButtonD)
        {
            falseD.SetActive(true);
        }
    }

    // ╭─────────────────────────────── ♡
    // │    UI
    // ╰─────────────────────────────── ♡

    private void ClearHover()
    {
        hoverA.SetActive(false);
        hoverB.SetActive(false);
        hoverC.SetActive(false);
        hoverD.SetActive(false);

        hoverDontKnow.SetActive(false);
    }

    private void ClearResult()
    {
        trueA.SetActive(false);
        trueB.SetActive(false);
        trueC.SetActive(false);
        trueD.SetActive(false);

        falseA.SetActive(false);
        falseB.SetActive(false);
        falseC.SetActive(false);
        falseD.SetActive(false);
    }

    private void DisableButtons()
    {
        answerButtonA.interactable = false;
        answerButtonB.interactable = false;
        answerButtonC.interactable = false;
        answerButtonD.interactable = false;
    }

    private void EnableButtons()
    {
        answerButtonA.interactable = true;
        answerButtonB.interactable = true;
        answerButtonC.interactable = true;
        answerButtonD.interactable = true;
    }

    // ╭─────────────────────────────── ♡
    // │    Auswertung
    // ╰─────────────────────────────── ♡
    public int GetScore()
    {
        if(!HasAnswer())
        {
            return 0;
        }

        if(IsCorrect())
        {
            return 1;
        }

        return -1;
    }
}