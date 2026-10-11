using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RingSpawner : MonoBehaviour
{
    // ╭───────────────────────────────── ♡
    // │    RingSpawner
    // ╰───────────────────────────────── ♡

    // ── ♡  Ring-Komponenten  ♡ ──

    [SerializeField] private GameObject ringPrefab;
    [SerializeField] private GameObject ringParent;
    [SerializeField] private GameObject game;
    [SerializeField] private GameObject finish;
    [SerializeField] private GameHandling gameHandler;

    // ── ♡  Ring-Erzeugung  ♡ ──

    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnOffset = 3f;
    [SerializeField] private float ringSpeed = 100f;
    [SerializeField] private float zeroRotationZ = -90f;
    [SerializeField] private float spawnTimer;

    // ── ♡  Spielfeld & Punktestand  ♡ ──

    [SerializeField] private RectTransform area;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private int score = 0;

    // ── ♡  GameLogic  ♡ ──

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
        UpdateScoreText();
    }

    // ╭───────────────────────────────── ♡
    // │    Spielsteuerung
    // ╰───────────────────────────────── ♡

    public void CloseGame()
    {
        gameLogic.CloseAllGames();
    }

    public void WonGane()
    {
        gameLogic.WonGame("Otto");
    }

    // ╭───────────────────────────────── ♡
    // │    Ring-Erzeugung
    // ╰───────────────────────────────── ♡

    private void Update()
    {
        if(ringPrefab == null || area == null)
        {
            return;
        }

        spawnTimer += Time.deltaTime;

        if(spawnTimer >= spawnInterval)
        {
            SpawnRing();
            spawnTimer = 0f;
        }
    }

    private void SpawnRing()
    {
        Vector3[] corners = new Vector3[4];
        area.GetWorldCorners(corners);

        float randomX =Random.Range(corners[0].x,corners[2].x);

        float spawnY = corners[1].y + spawnOffset;

        Vector3 spawnPosition = new Vector3(randomX,spawnY,transform.position.z);

        Quaternion spawnRotation =
            Quaternion.Euler(
                0f,
                0f,
                zeroRotationZ
            );

        GameObject ring = Instantiate(ringPrefab,spawnPosition,spawnRotation,ringParent != null ? ringParent.transform: null);

        RingManager movement = ring.GetComponent<RingManager>();

        if(movement != null)
        {
            movement.speed = ringSpeed;
            movement.area = area;
            movement.ringSpawner = this;
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Punktestand
    // ╰───────────────────────────────── ♡

    public void AddScore()
    {
        score++;
        UpdateScoreText();

        if(score >= 10)
        {
            game.SetActive(false);
            finish.SetActive(true);
            gameHandler.UnblockBanner("Otto");
        }
    }

    private void UpdateScoreText()
    {
        if(scoreText != null)
        {
            scoreText.text = score + "/10";
        }
    }
}