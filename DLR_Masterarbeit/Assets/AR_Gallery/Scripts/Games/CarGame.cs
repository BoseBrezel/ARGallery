using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarGame : MonoBehaviour
{
    // ╭───────────────────────────────── ♡
    // │    CarGame
    // ╰───────────────────────────────── ♡

    // ── ♡  Karosserie  ♡ ──

    [SerializeField] private GameObject karos1;
    [SerializeField] private GameObject karos2;
    [SerializeField] private GameObject karos3;

    // ── ♡  Räder  ♡ ──

    [SerializeField] private GameObject wheel1;
    [SerializeField] private GameObject wheel2;
    [SerializeField] private GameObject wheel3;

    // ── ♡  Motoren  ♡ ──

    [SerializeField] private GameObject motor1;
    [SerializeField] private GameObject motor2;
    [SerializeField] private GameObject motor3;

    // ── ♡  UI Elemente  ♡ ──

    [SerializeField] private GameObject FinishScreen;
    [SerializeField] private GameObject ReDoScreen;
    [SerializeField] private GameObject CheckButton;
    [SerializeField] private TMP_Text kmhRedo;

    // ── ♡  Auswahl  ♡ ──

    private int karosIndex = 0;
    private int wheelIndex = 0;
    private int motorIndex = 0;

    // ── ♡  Komponenten  ♡ ──

    private GameLogic gameLogic;

    // ╭───────────────────────────────── ♡
    // │    Initialisierung
    // ╰───────────────────────────────── ♡

    private void Awake()
    {
        gameLogic = FindFirstObjectByType<GameLogic>();
    }

    private void Start()
    {
        FinishScreen.SetActive(false);
        ReDoScreen.SetActive(false);

        UpdateKaros();
        UpdateWheel();
        UpdateMotor();
    }

    // ╭───────────────────────────────── ♡
    // │    Spiel verlassen & gewinnen
    // ╰───────────────────────────────── ♡

    public void CloseGame()
    {
        gameLogic.CloseAllGames();
    }

    public void WonGane()
    {
        gameLogic.WonGame("LaJamaisContente");
    }

    // ╭───────────────────────────────── ♡
    // │    Karosserie
    // ╰───────────────────────────────── ♡

    public void NextKaros()
    {
        karosIndex++;

        if(karosIndex > 2)
        {
            karosIndex = 0;
        }

        UpdateKaros();
    }

    public void BackKaros()
    {
        karosIndex--;

        if(karosIndex < 0)
        {
            karosIndex = 2;
        }

        UpdateKaros();
    }

    private void UpdateKaros()
    {
        karos1.SetActive(karosIndex == 0);
        karos2.SetActive(karosIndex == 1);
        karos3.SetActive(karosIndex == 2);
    }

    // ╭───────────────────────────────── ♡
    // │    Räder
    // ╰───────────────────────────────── ♡

    public void NextWheel()
    {
        wheelIndex++;

        if(wheelIndex > 2)
        {
            wheelIndex = 0;
        }

        UpdateWheel();
    }

    public void BackWheel()
    {
        wheelIndex--;

        if(wheelIndex < 0)
        {
            wheelIndex = 2;
        }

        UpdateWheel();
    }

    private void UpdateWheel()
    {
        wheel1.SetActive(wheelIndex == 0);
        wheel2.SetActive(wheelIndex == 1);
        wheel3.SetActive(wheelIndex == 2);
    }

    // ╭───────────────────────────────── ♡
    // │    Motor
    // ╰───────────────────────────────── ♡

    public void NextMotor()
    {
        motorIndex++;

        if(motorIndex > 2)
        {
            motorIndex = 0;
        }

        UpdateMotor();
    }

    public void BackMotor()
    {
        motorIndex--;

        if(motorIndex < 0)
        {
            motorIndex = 2;
        }

        UpdateMotor();
    }

    private void UpdateMotor()
    {
        motor1.SetActive(motorIndex == 0);
        motor2.SetActive(motorIndex == 1);
        motor3.SetActive(motorIndex == 2);
    }

    // ╭───────────────────────────────── ♡
    // │    Auto überprüfen
    // ╰───────────────────────────────── ♡

    public void CheckingCar()
    {
        GameObject currentKaros = GetCurrentKaros();
        GameObject currentWheel = GetCurrentWheel();
        GameObject currentMotor = GetCurrentMotor();

        int karosKmh =
            currentKaros.GetComponent<CarPart>().GetKmhValue();

        int wheelKmh =
            currentWheel.GetComponent<CarPart>().GetKmhValue();

        int motorKmh =
            currentMotor.GetComponent<CarPart>().GetKmhValue();

        int totalKmh = karosKmh + wheelKmh + motorKmh;

        if(totalKmh == 100)
        {
            GameComplete(totalKmh);
        }
        else
        {
            Redo(totalKmh);
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Aktuelle Teile
    // ╰───────────────────────────────── ♡

    private GameObject GetCurrentKaros()
    {
        if(karosIndex == 0)
        {
            return karos1;
        }

        if(karosIndex == 1)
        {
            return karos2;
        }

        return karos3;
    }

    private GameObject GetCurrentWheel()
    {
        if(wheelIndex == 0)
        {
            return wheel1;
        }

        if(wheelIndex == 1)
        {
            return wheel2;
        }

        return wheel3;
    }

    private GameObject GetCurrentMotor()
    {
        if(motorIndex == 0)
        {
            return motor1;
        }

        if(motorIndex == 1)
        {
            return motor2;
        }

        return motor3;
    }

    // ╭───────────────────────────────── ♡
    // │    Spielabschluss
    // ╰───────────────────────────────── ♡

    public void GameComplete(int totalKmh)
    {
        ReDoScreen.SetActive(false);
        FinishScreen.SetActive(true);
        CheckButton.SetActive(false);
    }

    public void Redo(int totalKmh)
    {
        kmhRedo.text = totalKmh + " km/h";

        FinishScreen.SetActive(false);
        ReDoScreen.SetActive(true);
        CheckButton.SetActive(false);
    }

    // ╭───────────────────────────────── ♡
    // │    Spiel zurücksetzen
    // ╰───────────────────────────────── ♡

    public void CloseRedoScreen()
    {
        ReDoScreen.SetActive(false);
        CheckButton.SetActive(true);
    }

    public void RestartGame()
    {
        FinishScreen.SetActive(false);
        ReDoScreen.SetActive(false);
        CheckButton.SetActive(true);
    }

    public void CloseFinishScreen()
    {
        FinishScreen.SetActive(false);
    }
}