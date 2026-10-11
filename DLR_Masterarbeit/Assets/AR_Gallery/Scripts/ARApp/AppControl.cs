using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.IO;

// ╭───────────────────────────────── ♡
// │    Datenklassen für Speicherung
// ╰───────────────────────────────── ♡
[Serializable]
public class BannerData
{
    public string name;
}

[Serializable]
public class GameStateData
{
    public string state;
}

[Serializable]
public class AppData
{
    public List<BannerData> banner = new List<BannerData>();
    public GameStateData gameState = new GameStateData();
}

// ╭───────────────────────────────── ♡
// │    Appcontrol
// ╰───────────────────────────────── ♡
public class AppControl : MonoBehaviour
{
    // ── ♡  App Komponenten  ♡ ──
    [SerializeField] RecognitionHandler recognitionHandler;
    [SerializeField] GameHandling gameHand;
    [SerializeField] VersionController versionController;

    private GameObject currentARGallery;
    [SerializeField] GameObject checkingPanel;
    [SerializeField] TMP_InputField checkInput;
    [SerializeField] GameObject umfragePanel;
    
    // ── ♡  Keyboard  ♡ ──
    private TouchScreenKeyboard keyboard;

    // ── ♡  AppData  ♡ ──
    private AppData appData;
    private string filePath;
    
    // ── ♡  QuizData  ♡ ──
    private QuizData quizData;
    private string quizFilePath;
    
    // ── ♡  Nach Neustart oder Start initialisieren   ♡ ──
    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath,"appData.json");

        quizFilePath = Path.Combine(Application.persistentDataPath,"quizData.json");

        LoadData();
        LoadQuizData();
        
        // ── ♡  Spielstand prüfen und laden   ♡ ──
        if(string.IsNullOrEmpty(appData.gameState.state))
        {
            versionController.Starting();
        }
        else if(appData.gameState.state == "NonGame")
        {
            versionController.NonGameSaved();
        }
        else if(appData.gameState.state == "Game")
        {
            UnblockBanners();
            versionController.GameSaved();
        }
    }
    // ╭─────────────────────────────── ♡
    // │    Umfrage
    // ╰─────────────────────────────── ♡
    public void AddSurvey(SurveyResultData survey)
    {
        if(quizData == null)
        {
            quizData = new QuizData();
        }

        if(quizData.surveys == null)
        {
            quizData.surveys = new List<SurveyResultData>();
        }

        quizData.surveys.Add(survey);
        SaveQuizData();
    }

    // ╭─────────────────────────────── ♡
    // │    Tastatur
    // ╰─────────────────────────────── ♡
    private void OpenKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open(checkInput.text,TouchScreenKeyboardType.NumberPad,false,false,false,false);
    }

    private void Update()
    {
        if(keyboard != null)
        {
            checkInput.text = keyboard.text;

            if(!keyboard.active)
            {
                keyboard = null;
            }
        }
    }

    public void CheckCode()
    {
        if(keyboard != null)
        {
            checkInput.text = keyboard.text;
        }

        string code = checkInput.text.Trim();

        if(code == "1245")
        {
            Debug.Log("Code richtig!");

            CloseKeyboard();
            checkingPanel.SetActive(false);
            umfragePanel.SetActive(true);
            CreateNewSurvey();
        }
        else
        {
            Debug.Log("Falscher Code: " + code);

            checkInput.text = "";
            checkInput.Select();
            checkInput.ActivateInputField();
            OpenKeyboard();
        }
    }
    private void CloseKeyboard()
    {
        if(keyboard != null)
        {
            keyboard.active = false;
            keyboard = null;
        }

        TouchScreenKeyboard.hideInput = true;
    }

    public void CloseChecking()
    {
        CloseKeyboard();
        checkingPanel.SetActive(false);
        checkInput.text = "";
    }

    // ╭─────────────────────────────── ♡
    // │    Appdaten
    // ╰─────────────────────────────── ♡
    public void LoadData()
    {
        if(File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            appData = JsonUtility.FromJson<AppData>(json);

            if(appData == null)
            {
                appData = new AppData();
            }

            if(appData.banner == null)
            {
                appData.banner = new List<BannerData>();
            }

            if(appData.gameState == null)
            {
                appData.gameState = new GameStateData();
            }
        }
        else
        {
            appData = new AppData();
            SaveData();
        }
    }

    // ── ♡  App-Daten speichern  ♡ ──
    private void SaveData()
    {
        string json = JsonUtility.ToJson(appData, true);

        File.WriteAllText(filePath,json);
    }

    // ╭─────────────────────────────── ♡
    // │    Quiz Daten
    // ╰─────────────────────────────── ♡
    public void LoadQuizData()
    {
        if(File.Exists(quizFilePath))
        {
            string json = File.ReadAllText(quizFilePath);
            quizData = JsonUtility.FromJson<QuizData>(json);

            if(quizData == null)
            {
                quizData = new QuizData();
            }

            if(quizData.surveys == null)
            {
                quizData.surveys = new List<SurveyResultData>();
            }
        }
        else
        {
            quizData = new QuizData();
            SaveQuizData();
        }
    }

    public void SaveQuizData()
    {
        string json = JsonUtility.ToJson(quizData,true);
        File.WriteAllText(quizFilePath,json);

        Debug.Log("quizData.json gespeichert.");
    }
    
    public SurveyResultData CreateNewSurvey()
    {
        if(quizData == null)
        {
            LoadQuizData();
        }

        SurveyResultData newSurvey = new SurveyResultData();
        int surveyNumber = quizData.surveys.Count + 1;
        newSurvey.name = "Umfrage" + surveyNumber;
        newSurvey.date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        if(appData != null && appData.gameState != null)
        {
            newSurvey.gameState = appData.gameState.state;
        }
        else
        {
            newSurvey.gameState = "";
        }
        return newSurvey;
    }

    public void ResetApp()
    {
        DeleteData();
        Application.Quit();
    }

    public void DeleteData()
    {
        if(File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("appData.json wurde gelöscht.");
        }

        appData = new AppData();

        Debug.Log("quizData.json bleibt erhalten.");
    }

    public void UnblockBanners()
    {
        List<string> bannerNames = new List<string>();

        foreach(BannerData banner in appData.banner)
        {
            bannerNames.Add(banner.name);
            gameHand.ActivateBadgeWithoutAnimation(banner.name);
        }

        recognitionHandler.putUnblockBanners(bannerNames);
    }

    public void putBanner(string name)
    {
        appData.banner.Add(new BannerData{name = name});
        SaveData();
    }

    public void PutVersionGame()
    {
        putGameState("Game");
    }

    public void PutVersionNoneGame()
    {
        putGameState("NonGame");
    }

    public void putGameState(string name)
    {
        appData.gameState.state = name;
        SaveData();
    }
}