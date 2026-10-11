using UnityEngine;
using UnityEngine.InputSystem;

public class Fruit : MonoBehaviour
{
    // ╭───────────────────────────────── ♡
    // │    Fruit
    // ╰───────────────────────────────── ♡

    // ── ♡  Bewegung  ♡ ──

    public float speed = 3f;
    public float verticalSpeed = 5f;
    public float sensitivityY = 2f;

    // ── ♡  Spielfeld  ♡ ──

    public RectTransform area;

    // ── ♡  Frucht & Spiel  ♡ ──

    private FruitType fruitType;
    private FruitGame game;

    // ── ♡  Status  ♡ ──

    private bool alreadyFinished = false;

    // ╭───────────────────────────────── ♡
    // │    Initialisierung
    // ╰───────────────────────────────── ♡

    private void Start()
    {
        if(Accelerometer.current != null)
        {
            InputSystem.EnableDevice(Accelerometer.current);
        }
    }

    public void Setup(FruitType type, FruitGame fruitGame)
    {
        fruitType = type;
        game = fruitGame;
    }

    // ╭───────────────────────────────── ♡
    // │    Update
    // ╰───────────────────────────────── ♡

    private void Update()
    {
        if(alreadyFinished)
        {
            return;
        }

        if(game == null)
        {
            return;
        }

        MoveFruit();
        CheckBounds();
    }

    // ╭───────────────────────────────── ♡
    // │    Frucht bewegen
    // ╰───────────────────────────────── ♡

    private void MoveFruit()
    {
        float verticalMovement = -verticalSpeed * Time.deltaTime;
        float horizontal = 0f;

        // ── ♡  Bewegung mit Beschleunigungssensor  ♡ ──

        if(Accelerometer.current != null)
        {
            Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();

            horizontal = acceleration.x * sensitivityY;
        }

        Vector3 movement =
            new Vector3(horizontal * speed * Time.deltaTime,verticalMovement,0f);

        transform.position += movement;

        // ── ♡  Bewegung begrenzen  ♡ ──

        if(area != null)
        {
            Vector3[] corners = new Vector3[4];
            area.GetWorldCorners(corners);

            Vector3 pos = transform.position;

            pos.x = Mathf.Clamp(
                pos.x,
                corners[0].x,
                corners[2].x
            );

            transform.position = pos;
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Spielfeld verlassen
    // ╰───────────────────────────────── ♡

    private void CheckBounds()
    {
        if(transform.position.y <= -350f)
        {
            if(alreadyFinished)
            {
                return;
            }

            alreadyFinished = true;
            game.FruitNMissedewton(fruitType);
            Destroy(gameObject);
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Newton treffen
    // ╰───────────────────────────────── ♡

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(alreadyFinished)
        {
            return;
        }

        if(game == null)
        {
            return;
        }

        if(other.gameObject == game.newtonCollder)
        {
            alreadyFinished = true;
            game.FruitHitNewton(fruitType);
            Destroy(gameObject);
        }
    }
}