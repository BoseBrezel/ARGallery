using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;


// ╭───────────────────────────────── ♡
// │    VersionController
// ╰───────────────────────────────── ♡
public class VersionController : MonoBehaviour
{
    // ── ♡  App Komponenten  ♡ ──
    [SerializeField] RecognitionHandler recognitionHandler;
    [SerializeField] AppControl appController;


    // ── ♡  UI Elemente  ♡ ──
    [SerializeField] GameObject tutorialGame;
    [SerializeField] GameObject tutorialNonGame;
    [SerializeField] GameObject badges;
    [SerializeField] GameObject Games;
    [SerializeField] GameObject background;

    [SerializeField] TMP_InputField nameInput;
    [SerializeField] Button fertigButton;
    [SerializeField] GameObject backgound;


    // ── ♡  Keyboard  ♡ ──
    private TouchScreenKeyboard keyboard;


    // ╭─────────────────────────────── ♡
    // │    Initialisierung
    // ╰─────────────────────────────── ♡

    // ── ♡  Komponenten initialisieren  ♡ ──
    private void Start()
    {
        fertigButton.onClick.AddListener(Fertig);
        nameInput.onSelect.AddListener(OnInputSelected);
    }


    // ╭─────────────────────────────── ♡
    // │    Start & Einrichtung
    // ╰─────────────────────────────── ♡

    // ── ♡  App beim ersten Start anzeigen  ♡ ──
    public void Starting()
    {
        fertigButton.gameObject.SetActive(true);
        backgound.gameObject.SetActive(true);
        nameInput.gameObject.SetActive(true);
        background.SetActive(true);
    }


    // ── ♡  NonGame Modus einrichten  ♡ ──
    public void SetUpNonGame()
    {
        SetAll(false);

        tutorialNonGame.SetActive(true);
        recognitionHandler.setBlockState(true);
        appController.putGameState("NonGame");
    }


    // ── ♡  Game Modus einrichten  ♡ ──
    public void SetUpGame()
    {
        SetAll(false);

        recognitionHandler.setBlockState(false);
        tutorialGame.SetActive(true);
        badges.SetActive(true);
        Games.SetActive(true);
        appController.putGameState("Game");
    }
    
    // ── ♡  Tastatur   ♡ ──
    private void OnInputSelected(string text)
    {
        keyboard = TouchScreenKeyboard.Open(nameInput.text,TouchScreenKeyboardType.Default,false,false,false,false);
    }

    private void Update()
    {
        if(keyboard != null)
        {
            nameInput.text = keyboard.text;
        }
    }


    // ── ♡  Eingabe checken  ♡ ──
    public void Fertig()
    {
        if(keyboard != null)
        {
            nameInput.text = keyboard.text;
            keyboard.active = false;
            keyboard = null;
        }

        TouchScreenKeyboard.hideInput = true;

        string inhalt = nameInput.text.Trim();

        if(inhalt == "7872")
        {
            SetUpNonGame();
            appController.PutVersionNoneGame();
        }
        else if(inhalt == "3840")
        {
            SetUpGame();
            appController.PutVersionGame();
        }
        else
        {
            Debug.Log("Ungültiger Inhalt: " + inhalt);
        }
    }


    // ╭─────────────────────────────── ♡
    // │    UI Verwaltung
    // ╰─────────────────────────────── ♡

    private void SetAll(bool state)
    {
        fertigButton.gameObject.SetActive(state);
        backgound.gameObject.SetActive(state);
        nameInput.gameObject.SetActive(state);
        background.SetActive(state);

        tutorialGame.SetActive(state);
        tutorialNonGame.SetActive(state);
        badges.SetActive(state);
        Games.SetActive(state);
    }


    // ╭─────────────────────────────── ♡
    // │   Spielstand
    // ╰─────────────────────────────── ♡

    public void NonGameSaved()
    {
        recognitionHandler.setBlockState(true);
        appController.putGameState("NonGame");
    }

    public void GameSaved()
    {
        SetAll(false);

        recognitionHandler.setBlockState(false);

        badges.SetActive(true);
        Games.SetActive(true);

        appController.putGameState("Game");
    }
}