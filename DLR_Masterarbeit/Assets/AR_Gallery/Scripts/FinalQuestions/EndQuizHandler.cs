using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// ╭───────────────────────────────── ♡
// │    EndQuizHandler
// │    Logik mithilfe von KI angepasst.
// ╰───────────────────────────────── ♡

public class EndQuizHandler : MonoBehaviour
{
    // ╭─────────────────────────────── ♡
    // │    App Komponenten
    // ╰─────────────────────────────── ♡

    // ── ♡  Daten & Umfrage  ♡ ──
    [SerializeField] private AppControl appControl;
    [SerializeField] private Data data;
    [SerializeField] private SurveyManager surveyManager;

    private SurveyResultData currentSurvey;


    // ╭─────────────────────────────── ♡
    // │    Quiz UI
    // ╰─────────────────────────────── ♡

    // ── ♡  Quizbereich  ♡ ──
    [SerializeField] GameObject quizPrefab;
    [SerializeField] Transform quizArea;


    // ── ♡  Navigation  ♡ ──
    [SerializeField] Button nextButton;
    [SerializeField] Button dontknow;
    [SerializeField] Button previousButton;
    [SerializeField] Button finishButton;


    // ── ♡  Ergebnisanzeige  ♡ ──
    [SerializeField] TMP_Text resultText;


    // ╭─────────────────────────────── ♡
    // │    Quizdaten
    // ╰─────────────────────────────── ♡

    private QuizCollection quizData;
    private List<EndQuiz> quizes = new List<EndQuiz>();

    private int currentQuiz = 0;
    private bool quizFinished = false;


    // ╭─────────────────────────────── ♡
    // │    Initialisierung
    // ╰─────────────────────────────── ♡

    // ── ♡  Quiz starten  ♡ ──
    private void Start()
    {
        CreateQuizes();

        dontknow.onClick.AddListener(SelectDontKnow);

        nextButton.onClick.AddListener(NextQuiz);
        previousButton.onClick.AddListener(PreviousQuiz);
        finishButton.onClick.AddListener(FinishQuiz);

        ShowQuiz(0);
    }


    // ╭─────────────────────────────── ♡
    // │    Quiz erstellen
    // ╰─────────────────────────────── ♡

    // ── ♡  Einzelnes Quiz erstellen  ♡ ──
    private void CreateQuiz(string name, QuizEntry data)
    {
        GameObject obj = Instantiate(quizPrefab, quizArea);

        EndQuiz quiz = obj.GetComponent<EndQuiz>();

        quiz.Setup(name, data);

        quizes.Add(quiz);
    }


    // ── ♡  Alle Quiz erstellen  ♡ ──
    public void CreateQuizes()
    {
        TextAsset json = Resources.Load<TextAsset>("quiz");

        if(json == null)
        {
            Debug.LogError("quiz.json wurde nicht in Resources gefunden!");
            return;
        }

        quizData = JsonUtility.FromJson<QuizCollection>(json.text);


        // ── ♡  Quizfragen hinzufügen  ♡ ──
        CreateQuiz("AlbertEinstein_EndQuiz", quizData.AlbertEinstein_EndQuiz);
        CreateQuiz("WernerVonSiemens_EndQuiz", quizData.WernerVonSiemens_EndQuiz);
        CreateQuiz("AmaliaMaryEarhart_EndQuiz", quizData.AmaliaMaryEarhart_EndQuiz);
        CreateQuiz("Buzz_EndQuiz", quizData.Buzz_EndQuiz);
        CreateQuiz("CarlBenz_EndQuiz", quizData.CarlBenz_EndQuiz);
        CreateQuiz("CaecilleBenz_EndQuiz", quizData.CaecilleBenz_EndQuiz);
        CreateQuiz("Derwittzer_EndQuiz", quizData.Derwittzer_EndQuiz);
        CreateQuiz("galileoMonde_EndQuiz", quizData.galileoMonde_EndQuiz);
        CreateQuiz("GalileoGalilei_EndQuiz", quizData.GalileoGalilei_EndQuiz);
        CreateQuiz("ISS_EndQuiz", quizData.ISS_EndQuiz);
        CreateQuiz("LaJamaisContente_EndQuiz", quizData.LaJamaisContente_EndQuiz);
        CreateQuiz("Hamilton_EndQuiz", quizData.Hamilton_EndQuiz);
        CreateQuiz("MarieCurie_EndQuiz", quizData.MarieCurie_EndQuiz);
        CreateQuiz("LiseMeitner_EndQuiz", quizData.LiseMeitner_EndQuiz);
        CreateQuiz("MarsExpress_EndQuiz", quizData.MarsExpress_EndQuiz);
        CreateQuiz("Otto_EndQuiz", quizData.Otto_EndQuiz);
        CreateQuiz("IsaacNewton_EndQuiz", quizData.IsaacNewton_EndQuiz);
        CreateQuiz("SpaceShuttle_EndQuiz", quizData.SpaceShuttle_EndQuiz);
    }


    // ╭─────────────────────────────── ♡
    // │    Quiz Navigation
    // ╰─────────────────────────────── ♡

    // ── ♡  Quiz anzeigen  ♡ ──
    private void ShowQuiz(int index)
    {
        if(index < 0 || index >= quizes.Count)
        {
            return;
        }

        foreach(EndQuiz quiz in quizes)
        {
            quiz.gameObject.SetActive(false);
        }

        currentQuiz = index;

        quizes[currentQuiz].gameObject.SetActive(true);

        previousButton.gameObject.SetActive(currentQuiz > 0);

        bool lastQuiz = currentQuiz == quizes.Count - 1;

        nextButton.gameObject.SetActive(!lastQuiz);
        finishButton.gameObject.SetActive(lastQuiz);
    }


    // ── ♡  "Weiß nicht" auswählen  ♡ ──
    private void SelectDontKnow()
    {
        quizes[currentQuiz].SelectDontKnow();
    }


    // ── ♡  Zum nächsten Quiz  ♡ ──
    private void NextQuiz()
    {
        if(currentQuiz < quizes.Count - 1)
        {
            ShowQuiz(currentQuiz + 1);
        }
    }


    // ── ♡  Zum vorherigen Quiz  ♡ ──
    private void PreviousQuiz()
    {
        if(currentQuiz > 0)
        {
            ShowQuiz(currentQuiz - 1);
        }
    }


    // ╭─────────────────────────────── ♡
    // │    Quiz beenden
    // ╰─────────────────────────────── ♡

    // ── ♡  Quiz auswerten  ♡ ──
    private void FinishQuiz()
    {
        if(quizFinished)
        {
            return;
        }

        quizFinished = true;

        int totalScore = 0;

        foreach(EndQuiz quiz in quizes)
        {
            totalScore += quiz.GetScore();
            quiz.ShowResult();
        }

        resultText.text = "Auswertung: " + totalScore + " Punkte";
    }


    // ╭─────────────────────────────── ♡
    // │    Ergebnisse speichern
    // ╰─────────────────────────────── ♡

    // ── ♡  Umfrageergebnis speichern  ♡ ──
    public void SaveResult()
    {
        if(currentSurvey != null)
        {
            Debug.LogWarning("Diese Umfrage wurde bereits gespeichert!");
            return;
        }

        currentSurvey = appControl.CreateNewSurvey();


        // ── ♡  Persönliche Daten speichern  ♡ ──
        currentSurvey.age = data.GetAge();
        currentSurvey.gender = data.GetGender();


        // ── ♡  Umfrage vorbereiten  ♡ ──
        surveyManager.SaveSurveyAnswers(currentSurvey);

        currentSurvey.endQuizAnswers.Clear();


        // ── ♡  Quizantworten speichern  ♡ ──
        int totalScore = 0;

        foreach(EndQuiz quiz in quizes)
        {
            EndQuizData quizAnswer = new EndQuizData();

            quizAnswer.quizName = quiz.GetQuizName();
            quizAnswer.question = quiz.GetQuestion();
            quizAnswer.selectedAnswer = quiz.GetSelectedAnswer();
            quizAnswer.correctAnswer = quiz.GetCorrectAnswer();
            quizAnswer.score = quiz.GetScore();

            currentSurvey.endQuizAnswers.Add(quizAnswer);

            totalScore += quizAnswer.score;
        }


        // ── ♡  Gesamtergebnis speichern  ♡ ──
        currentSurvey.totalScore = totalScore;

        appControl.AddSurvey(currentSurvey);

        Debug.Log("Umfrage vollständig gespeichert!");
    }
}