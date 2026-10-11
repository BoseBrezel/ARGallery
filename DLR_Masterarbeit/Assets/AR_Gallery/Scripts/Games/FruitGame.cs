using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class FruitGame : MonoBehaviour
{
    // ╭───────────────────────────────── ♡
    // │    FruitGame
    // ╰───────────────────────────────── ♡

    // ── ♡  Frucht-Prefabs  ♡ ──

    public GameObject bananaPrefab;
    public GameObject strawberryPrefab;
    public GameObject applePrefab;

    // ── ♡  Spielfeld  ♡ ──

    public RectTransform fruitArea;
    public GameObject fruits;
    public GameObject newtonCollder;

    // ── ♡  Fortschritt  ♡ ──

    public int currentProgress = 0;
    public TMP_Text progressText;

    // ── ♡  UI Elemente  ♡ ──

    public GameObject failScreen;
    public GameObject finishScreen;

    // ── ♡  Spielstatus  ♡ ──

    private List<FruitType> spawnOrder = new List<FruitType>();
    private bool gameOver = false;

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
        if(failScreen != null)
        {
            failScreen.SetActive(false);
        }

        if(finishScreen != null)
        {
            finishScreen.SetActive(false);
        }

        currentProgress = 0;
        gameOver = false;

        UpdateProgressText();
        CreateRandomFruitOrder();
        SpawnNextFruit();
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
        gameLogic.WonGame("IsaacNewton");
    }

    // ╭───────────────────────────────── ♡
    // │    Fortschritt
    // ╰───────────────────────────────── ♡

    private void UpdateProgressText()
    {
        if(progressText != null)
        {
            progressText.text = currentProgress + "/7";
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Fruchtreihenfolge
    // ╰───────────────────────────────── ♡

    private void CreateRandomFruitOrder()
    {
        spawnOrder.Clear();

        spawnOrder.Add(FruitType.Banana);
        spawnOrder.Add(FruitType.Banana);
        spawnOrder.Add(FruitType.Banana);

        spawnOrder.Add(FruitType.Strawberry);
        spawnOrder.Add(FruitType.Strawberry);
        spawnOrder.Add(FruitType.Strawberry);

        spawnOrder.Add(FruitType.Apple);

        for(int i = spawnOrder.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            FruitType temp = spawnOrder[i];

            spawnOrder[i] = spawnOrder[randomIndex];
            spawnOrder[randomIndex] = temp;
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Früchte erzeugen
    // ╰───────────────────────────────── ♡

    public void SpawnNextFruit()
    {
        if(gameOver)
        {
            return;
        }

        if(currentProgress >= 7)
        {
            FinishGame();
            return;
        }

        FruitType type = spawnOrder[currentProgress];
        GameObject prefab = GetPrefab(type);

        if(prefab == null)
        {
            Debug.LogError("Prefab für " + type + " fehlt!");
            return;
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();

        GameObject newFruit = Instantiate(
            prefab,
            spawnPosition,
            Quaternion.identity,
            fruits.transform
        );

        Fruit fruit = newFruit.GetComponent<Fruit>();

        if(fruit != null)
        {
            fruit.Setup(type, this);
        }
        else
        {
            Debug.LogError("Das Prefab " + prefab.name + " fehlt.");
        }
    }

    private GameObject GetPrefab(FruitType type)
    {
        switch(type)
        {
            case FruitType.Banana:
                return bananaPrefab;

            case FruitType.Strawberry:
                return strawberryPrefab;

            case FruitType.Apple:
                return applePrefab;

            default:
                return null;
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Rect rect = fruitArea.rect;

        float x = Random.Range(rect.xMin, rect.xMax);
        float y = rect.yMax + 100f;

        Vector3 localPosition = new Vector3(x, y, 0);

        return fruitArea.TransformPoint(localPosition);
    }

    // ╭───────────────────────────────── ♡
    // │    Früchte treffen Newton
    // ╰───────────────────────────────── ♡

    public void FruitHitNewton(FruitType type)
    {
        if(gameOver)
        {
            return;
        }

        if(type == FruitType.Apple)
        {
            currentProgress++;
            UpdateProgressText();

            if(currentProgress >= 7)
            {
                FinishGame();
                return;
            }

            SpawnNextFruit();
        }
        else
        {
            GameOver();
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Früchte verfehlen Newton
    // ╰───────────────────────────────── ♡

    public void FruitNMissedewton(FruitType type)
    {
        if(gameOver)
        {
            return;
        }

        if(type == FruitType.Banana || type == FruitType.Strawberry)
        {
            currentProgress++;
            UpdateProgressText();

            if(currentProgress >= 7)
            {
                FinishGame();
                return;
            }

            SpawnNextFruit();
        }
        else if(type == FruitType.Apple)
        {
            GameOver();
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Spiel verloren
    // ╰───────────────────────────────── ♡

    private void GameOver()
    {
        if(gameOver)
        {
            return;
        }

        gameOver = true;

        if(fruits != null)
        {
            foreach(Transform child in fruits.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if(failScreen != null)
        {
            failScreen.SetActive(true);
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Spiel geschafft
    // ╰───────────────────────────────── ♡

    private void FinishGame()
    {
        gameOver = true;

        Debug.Log("🎉 SPIEL GESCHAFFT!");

        if(fruits != null)
        {
            foreach(Transform child in fruits.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if(finishScreen != null)
        {
            finishScreen.SetActive(true);
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Spiel neustarten
    // ╰───────────────────────────────── ♡

    public void RestartGame()
    {
        gameOver = false;
        currentProgress = 0;

        if(fruits != null)
        {
            foreach(Transform child in fruits.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if(failScreen != null)
        {
            failScreen.SetActive(false);
        }

        if(finishScreen != null)
        {
            finishScreen.SetActive(false);
        }

        UpdateProgressText();
        CreateRandomFruitOrder();
        SpawnNextFruit();
    }
}

// ╭───────────────────────────────── ♡
// │    Fruchttypen
// ╰───────────────────────────────── ♡

public enum FruitType
{
    Banana,
    Strawberry,
    Apple
}