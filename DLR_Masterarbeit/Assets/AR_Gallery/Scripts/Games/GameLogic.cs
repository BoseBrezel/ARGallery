using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour
{
    // ╭───────────────────────────────── ♡
    // │    GameLogic
    // ╰───────────────────────────────── ♡

    // ── ♡  Manager  ♡ ──

    [SerializeField] private GameHandling gameHandler;
    [SerializeField] private ARTrackedImageManager imageManager;
    [SerializeField] private Transform gameContainer;
    [SerializeField] private List<GamePrefab> gamePrefabs;

    private GameObject currentGame;
    private GameObject currentGamePrefab;

    [SerializeField] private GameObject redoButton;
    [SerializeField] private GameObject closeButton;

    // ── ♡  Spiele  ♡ ──

    [SerializeField] private GameObject galileoGameObject;
    [SerializeField] private GameObject ottoPrefab;
    [SerializeField] private GameObject contentePrefab;
    [SerializeField] private GameObject marsExpressPrefab;
    [SerializeField] private GameObject newtonPrefab;

    // ╭───────────────────────────────── ♡
    // │    Margaret Hamilton
    // ╰───────────────────────────────── ♡

    // ── ♡  Papier  ♡ ──

    [SerializeField] private GameObject pilePaper;
    private GameObject spawnedPile;
    [SerializeField] private Transform spawnArea;

    // ── ♡  AR  ♡ ──

    [SerializeField] private GameObject aRWorldSpace;
    [SerializeField] private GameObject aRWorldSpaceGame;

    // ── ♡  Kalibrierung  ♡ ──

    public TMP_Text inputX;
    public TMP_Text inputY;
    public TMP_Text inputZ;

    public float x = 1.80f;
    public float y = 0.10f;
    public float z = -1.55f;

    // ╭───────────────────────────────── ♡
    // │    Marie Curie & Galileo Galilei
    // ╰───────────────────────────────── ♡

    // ── ♡  Marie Curie  ♡ ──

    [SerializeField] private GameObject marieBanner;
    private GameObject spawnedMarieBanner;
    [SerializeField] private GameObject geigerUI;

    // ── ♡  Galileo Galilei  ♡ ──

    public float galiX = 0f;
    public float galiY = 0f;
    public float galiZ = 0f;

    public TMP_Text galiInputX;
    public TMP_Text galiInputY;
    public TMP_Text galiInputZ;

    [SerializeField] private GameObject galiBanner;
    private GameObject spawnedGaliBanner;
    [SerializeField] private GameObject galiUI;

    // ╭───────────────────────────────── ♡
    // │    Spiele öffnen & schließen
    // ╰───────────────────────────────── ♡

    public void OpenGame(GameObject gamePrefab)
    {
        CloseAllGames();

        aRWorldSpace.SetActive(false);

        currentGamePrefab = gamePrefab;
        currentGame = Instantiate(gamePrefab, gameContainer);

        closeButton.SetActive(true);
        redoButton.SetActive(true);
    }

    public void CloseAllGames()
    {
        if(currentGame != null)
        {
            Destroy(currentGame);
            currentGame = null;
        }

        currentGamePrefab = null;

        redoButton.SetActive(false);
        closeButton.SetActive(false);

        CloseGalileoGame();

        aRWorldSpace.SetActive(true);
    }

    public void WonGame(string name)
    {
        Debug.Log("WonGame " + name);
        gameHandler.UnblockBanner(name);
    }

    public void RestartGame()
    {
        if(currentGamePrefab != null)
        {
            OpenGame(currentGamePrefab);
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Margaret Hamilton
    // ╰───────────────────────────────── ♡

    public void GameMargaretHamilton()
    {
        x = 1.80f;
        y = 0.10f;
        z = -1.55f;

        aRWorldSpace.SetActive(false);

        foreach(var image in imageManager.trackables)
        {
            if(image.referenceImage == null)
            {
                continue;
            }

            if(image.referenceImage.name == "Hamilton")
            {
                spawnedPile = Instantiate(pilePaper, image.transform);

                spawnedPile.transform.localPosition = Vector3.zero;
                spawnedPile.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

                UpdatePaperPosition();
                UpdateCalibrationText();
            }
        }
    }

    public void CloseGameMargaretHamilton()
    {
        if(spawnedPile != null)
        {
            Destroy(spawnedPile);
            spawnedPile = null;
        }

        aRWorldSpace.SetActive(true);
    }

    private void UpdatePaperPosition()
    {
        if(spawnedPile == null)
        {
            return;
        }

        spawnedPile.transform.localPosition =
            new Vector3(x, y, z);
    }

    // ╭───────────────────────────────── ♡
    // │    Galileo Galilei
    // ╰───────────────────────────────── ♡

    public void GameGaliMoons()
    {
        aRWorldSpace.SetActive(false);
        galiUI.SetActive(true);

        foreach(var image in imageManager.trackables)
        {
            if(image.referenceImage == null)
            {
                continue;
            }

            if(image.referenceImage.name == "GalileoGalilei")
            {
                spawnedGaliBanner = Instantiate(galiBanner, image.transform);

                spawnedGaliBanner.transform.localPosition = new Vector3(0.50f, 0f, 0.55f);

                UpdateGalileoPosition();
                UpdateGalileoCalibrationText();

                spawnedGaliBanner.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

                break;
            }
        }
    }

    private void UpdateGalileoPosition()
    {
        if(spawnedGaliBanner == null)
        {
            return;
        }

        spawnedGaliBanner.transform.localPosition = new Vector3(0.50f, 0f, 0.55f);
    }

    public void CloseGalileoGame()
    {
        if(spawnedGaliBanner != null)
        {
            Destroy(spawnedGaliBanner);
            spawnedGaliBanner = null;
        }

        galileoGameObject.SetActive(false);
        aRWorldSpace.SetActive(true);
    }

    // ╭───────────────────────────────── ♡
    // │    Galileo Kalibrierung
    // ╰───────────────────────────────── ♡

    public void ChangeGaliValue(ref float value, float amount)
    {
        value += amount;

        UpdateGalileoPosition();
        UpdateGalileoCalibrationText();
    }

    public void PlusGaliX() => ChangeGaliValue(ref galiX, 0.05f);
    public void MinusGaliX() => ChangeGaliValue(ref galiX, -0.05f);

    public void PlusGaliY() => ChangeGaliValue(ref galiY, 0.05f);
    public void MinusGaliY() => ChangeGaliValue(ref galiY, -0.05f);

    public void PlusGaliZ() => ChangeGaliValue(ref galiZ, 0.05f);
    public void MinusGaliZ() => ChangeGaliValue(ref galiZ, -0.05f);

    private void UpdateGalileoCalibrationText()
    {
        galiInputX.text = galiX.ToString("F2");
        galiInputY.text = galiY.ToString("F2");
        galiInputZ.text = galiZ.ToString("F2");
    }

    // ╭───────────────────────────────── ♡
    // │    Marie Curie
    // ╰───────────────────────────────── ♡

    public void MarieCurieGame()
    {
        aRWorldSpace.SetActive(false);
        geigerUI.SetActive(true);

        foreach(var image in imageManager.trackables)
        {
            if(image.referenceImage == null)
            {
                continue;
            }

            if(image.referenceImage.name == "MarieCurie")
            {
                spawnedMarieBanner = Instantiate(marieBanner, image.transform);

                spawnedMarieBanner.transform.localPosition = new Vector3(0f, 0.5f, 0f);

                spawnedMarieBanner.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

                break;
            }
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Hamilton Kalibrierung
    // │    Mithilfe von KI verkürzt und optimiert.
    // ╰───────────────────────────────── ♡

    private void UpdateCalibrationText()
    {
        inputX.text = x.ToString("F2");
        inputY.text = y.ToString("F2");
        inputZ.text = z.ToString("F2");
    }

    public void ChangeValue(ref float value, float amount)
    {
        value += amount;

        UpdatePaperPosition();
        UpdateCalibrationText();
    }

    public void PlusX() => ChangeValue(ref x, 0.05f);
    public void MinusX() => ChangeValue(ref x, -0.05f);

    public void PlusY() => ChangeValue(ref y, 0.05f);
    public void MinusY() => ChangeValue(ref y, -0.05f);

    public void PlusZ() => ChangeValue(ref z, 0.05f);
    public void MinusZ() => ChangeValue(ref z, -0.05f);

    public void CloseMarieCurieGame()
    {
        aRWorldSpace.SetActive(true);
        geigerUI.SetActive(false);

        if(marieBanner != null)
        {
            Destroy(spawnedMarieBanner);
            spawnedMarieBanner = null;
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Otto Lilienthal
    // ╰───────────────────────────────── ♡

    public void GameOttoLilienthal()
    {
        OpenGame(ottoPrefab);
    }

    public void CloseOttoLilienthal()
    {
        CloseAllGames();
    }

    // ╭───────────────────────────────── ♡
    // │    La Jamais Contente
    // ╰───────────────────────────────── ♡

    public void GameContente()
    {
        OpenGame(contentePrefab);
    }

    public void CloseContente()
    {
        CloseAllGames();
    }

    // ╭───────────────────────────────── ♡
    // │    Mars Express
    // ╰───────────────────────────────── ♡

    public void GameThreeD()
    {
        OpenGame(marsExpressPrefab);
    }

    public void CloseThreeD()
    {
        CloseAllGames();
    }

    // ╭───────────────────────────────── ♡
    // │    Isaac Newton
    // ╰───────────────────────────────── ♡

    public void GameSirIsaac()
    {
        OpenGame(newtonPrefab);
    }

    public void CloseSirIsaac()
    {
        CloseAllGames();
    }
}