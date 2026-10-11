using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quiz : MonoBehaviour
{
    // ╭───────────────────────────────── ♡
    // │    Quiz
    // ╰───────────────────────────────── ♡

    // ── ♡  Quiz  ♡ ──

    public string quizName;
    public GameObject quizPanel;

    [SerializeField] private Button evaluationButton;
    [SerializeField] private TMP_Text question;
    [SerializeField] private GameObject rightPanel;
    [SerializeField] private GameObject wrongPanel;

    // ── ♡  Antwort A  ♡ ──

    [SerializeField] private TMP_Text answerA;
    [SerializeField] private GameObject hoverA;
    [SerializeField] private GameObject falseA;
    [SerializeField] private GameObject trueA;
    [SerializeField] private Button answerButtonA;

    // ── ♡  Antwort B  ♡ ──

    [SerializeField] private TMP_Text answerB;
    [SerializeField] private GameObject hoverB;
    [SerializeField] private GameObject falseB;
    [SerializeField] private GameObject trueB;
    [SerializeField] private Button answerButtonB;

    // ── ♡  Antwort C  ♡ ──

    [SerializeField] private TMP_Text answerC;
    [SerializeField] private GameObject hoverC;
    [SerializeField] private GameObject falseC;
    [SerializeField] private GameObject trueC;
    [SerializeField] private Button answerButtonC;

    // ── ♡  Antwort D  ♡ ──

    [SerializeField] private TMP_Text answerD;
    [SerializeField] private GameObject hoverD;
    [SerializeField] private GameObject falseD;
    [SerializeField] private GameObject trueD;
    [SerializeField] private Button answerButtonD;

    // ── ♡  Auswahl  ♡ ──

    private Button rightButton;
    private Button selectedButton = null;

    // ── ♡  Status  ♡ ──

    private bool quizOpen;
    private bool quizSolved;

    // ╭───────────────────────────────── ♡
    // │    Initialisierung
    // ╰───────────────────────────────── ♡

    private void Start()
    {
        answerButtonA.onClick.AddListener(() => SelectAnswer(answerButtonA, hoverA));
        answerButtonB.onClick.AddListener(() => SelectAnswer(answerButtonB, hoverB));
        answerButtonC.onClick.AddListener(() => SelectAnswer(answerButtonC, hoverC));
        answerButtonD.onClick.AddListener(() => SelectAnswer(answerButtonD, hoverD));

        evaluationButton.onClick.AddListener(EvaluateSelectedAnswer);
    }

    // ╭───────────────────────────────── ♡
    // │    Quiz einrichten
    // ╰───────────────────────────────── ♡

    public void Setup(string name, QuizEntry data)
    {
        quizName = name;

        // ── ♡  Status zurücksetzen  ♡ ──

        quizSolved = false;
        selectedButton = null;

        evaluationButton.interactable = true;

        // ── ♡  Frage setzen  ♡ ──

        question.text = data.question;

        // ── ♡  Antworten vorbereiten  ♡ ──

        string[] answers = new string[]
        {
            data.answerRight,
            data.answer1,
            data.answer2,
            data.answer3
        };

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

        // ── ♡  Richtige Antwort finden  ♡ ──

        if(answers[0] == data.answerRight)
        {
            rightButton = answerButtonA;
        }
        else if(answers[1] == data.answerRight)
        {
            rightButton = answerButtonB;
        }
        else if(answers[2] == data.answerRight)
        {
            rightButton = answerButtonC;
        }
        else
        {
            rightButton = answerButtonD;
        }

        quizPanel.SetActive(false);
    }

    // ╭───────────────────────────────── ♡
    // │    Quiz öffnen & schließen
    // ╰───────────────────────────────── ♡

    public void Open()
    {
        quizOpen = true;
        quizPanel.SetActive(true);
    }

    public void Close()
    {
        quizOpen = false;
        quizPanel.SetActive(false);

        GameHandling gameHand = FindFirstObjectByType<GameHandling>();

        if(gameHand != null)
        {
            gameHand.CloseGameForQuiz(quizName);
        }

        if(quizSolved)
        {
            Finish();
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Antwort auswählen
    // ╰───────────────────────────────── ♡

    private void SelectAnswer(Button button, GameObject hover)
    {
        if(selectedButton == null)
        {
            ClearHover();
            selectedButton = button;
            Debug.Log("Aktiviere Hover: " + hover.name);
            hover.SetActive(true);
            return;
        }

        if(selectedButton != button)
        {
            ClearHover();
            selectedButton = button;
            hover.SetActive(true);
            return;
        }

        // ── ♡  Bereits ausgewählte Antwort  ♡ ──

        return;
    }

    // ╭───────────────────────────────── ♡
    // │    Antwort auswerten
    // ╰───────────────────────────────── ♡

    public void EvaluateSelectedAnswer()
    {
        if(selectedButton == null)
        {
            Debug.Log("Bitte zuerst eine Antwort auswählen.");
            return;
        }

        Evaluation(selectedButton);
    }

    private void Evaluation(Button button)
    {
        DisableButtons();

        if(button == rightButton)
        {
            // ── ♡  Richtige Antwort  ♡ ──

            quizSolved = true;

            ShowCorrect(button);

            rightPanel.SetActive(true);
        }
        else
        {
            // ── ♡  Falsche Antwort  ♡ ──

            quizSolved = false;

            ShowWrong(button);
            ShowCorrect(rightButton);

            wrongPanel.SetActive(true);
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Antwort anzeigen
    // ╰───────────────────────────────── ♡

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

    // ╭───────────────────────────────── ♡
    // │    UI zurücksetzen
    // ╰───────────────────────────────── ♡

    private void ClearHover()
    {
        hoverA.SetActive(false);
        hoverB.SetActive(false);
        hoverC.SetActive(false);
        hoverD.SetActive(false);
    }

    private void DisableButtons()
    {
        answerButtonA.interactable = false;
        answerButtonB.interactable = false;
        answerButtonC.interactable = false;
        answerButtonD.interactable = false;

        evaluationButton.interactable = false;
    }

    // ╭───────────────────────────────── ♡
    // │    Quiz wiederholen
    // ╰───────────────────────────────── ♡

    public void ReDo()
    {
        quizSolved = false;
        selectedButton = null;

        ClearHover();

        trueA.SetActive(false);
        trueB.SetActive(false);
        trueC.SetActive(false);
        trueD.SetActive(false);

        falseA.SetActive(false);
        falseB.SetActive(false);
        falseC.SetActive(false);
        falseD.SetActive(false);

        rightPanel.SetActive(false);
        wrongPanel.SetActive(false);

        answerButtonA.interactable = true;
        answerButtonB.interactable = true;
        answerButtonC.interactable = true;
        answerButtonD.interactable = true;

        evaluationButton.interactable = true;
    }

    // ╭───────────────────────────────── ♡
    // │    Quiz abschließen
    // ╰───────────────────────────────── ♡

    private void Finish()
    {
        GameHandling gameHand = FindFirstObjectByType<GameHandling>();

        if(gameHand != null)
        {
            gameHand.UnblockBanner(quizName);
        }
    }
}