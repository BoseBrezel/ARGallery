using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RingManager : MonoBehaviour
{
    // ╭───────────────────────────────── ♡
    // │    RingManager
    // ╰───────────────────────────────── ♡

    // ── ♡  Bewegung  ♡ ──

    [SerializeField] public float speed = 5f;
    [SerializeField] public float grenze = 5f;
    [SerializeField] private float maxSpeedMultiplier = 3f;

    // ── ♡  Größe  ♡ ──

    [SerializeField] private float startScale = 0.0008f;
    [SerializeField] private float endScale = 0.8f;
    [SerializeField] public RectTransform area;
    [SerializeField] private Vector3 originalScale;

    // ── ♡  Animation  ♡ ──

    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private float redDuration = 0.15f;
    [SerializeField] private float redHoldDuration = 0.05f;
    [SerializeField] private float progress = -90f;

    // ── ♡  Moon-Ringe  ♡ ──

    [SerializeField] private RawImage[] circleRawImages;

    // ── ♡  Treffer & Status  ♡ ──

    [SerializeField] private Collider2D triggerPlane;
    [SerializeField] private Color originalRingColor;
    [SerializeField] private Coroutine flashCoroutine;
    [SerializeField] private bool wasHit = false;
    [SerializeField] private bool hasMissed = false;

    // ── ♡  RingSpawner  ♡ ──

    [SerializeField] public RingSpawner ringSpawner;

    // ╭───────────────────────────────── ♡
    // │    Initialisierung
    // ╰───────────────────────────────── ♡

    private void Start()
    {
        originalScale = transform.localScale;
        transform.localScale = originalScale * startScale;

        SpriteRenderer spriteRenderer =
            GetComponent<SpriteRenderer>();

        if(spriteRenderer != null)
        {
            originalRingColor = spriteRenderer.color;
        }
        else
        {
            originalRingColor = Color.white;
        }

        GameObject plane =
            GameObject.FindGameObjectWithTag("RingTrigger");

        if(plane != null)
        {
            triggerPlane = plane.GetComponent<Collider2D>();
        }

        if(triggerPlane == null)
        {
            Debug.Log("Kein FLugzeug gefunden");
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Ring bewegen
    // ╰───────────────────────────────── ♡

    private void Update()
    {
        if(area == null || wasHit || hasMissed)
        {
            return;
        }

        Vector3[] corners = new Vector3[4];
        area.GetWorldCorners(corners);

        float top = corners[1].y;
        float bottom = corners[0].y;

        progress =
            Mathf.InverseLerp(
                top,
                bottom,
                transform.position.y
            );

        progress = Mathf.Clamp01(progress);

        if(transform.position.y < grenze)
        {
            hasMissed = true;

            if(flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }

            flashCoroutine = StartCoroutine(MissAnimation());
        }
        else
        {
            // ── ♡  Größe anpassen  ♡ ──

            float currentScale = Mathf.Lerp(startScale,endScale,progress);

            transform.localScale = new Vector3( originalScale.x * currentScale, originalScale.y * currentScale,originalScale.z * currentScale);

            // ── ♡  Geschwindigkeit anpassen  ♡ ──

            float currentSpeed = speed * Mathf.Lerp(1f,maxSpeedMultiplier,progress);

            transform.position += Vector3.down * currentSpeed * Time.deltaTime;
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Ring getroffen
    // ╰───────────────────────────────── ♡

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("RingTrigger"))
        {
            return;
        }

        if(wasHit || hasMissed)
        {
            return;
        }

        wasHit = true;

        if(ringSpawner != null)
        {
            ringSpawner.AddScore();
        }

        if(flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashMoonCircles());
    }


    // ╭───────────────────────────────── ♡
    // │    Treffer-Animation
    // │    Vollständig KI-generiert.
    // ╰───────────────────────────────── ♡


    private IEnumerator FlashMoonCircles()
    {
        // ── ♡  Animationseinstellungen  ♡ ──

        float growDuration = 0.15f;
        float holdDuration = 0.05f;
        float scaleMultiplier = 1.15f;

        Color green = new Color(
            63f / 255f,
            255f / 255f,
            0f,
            1f
        );

        // ── ♡  Originalwerte speichern  ♡ ──

        Vector3[] originalScales =
            new Vector3[circleRawImages.Length];

        Color[] originalColors =
            new Color[circleRawImages.Length];

        for(int i = 0; i < circleRawImages.Length; i++)
        {
            if(circleRawImages[i] == null)
            {
                continue;
            }

            originalScales[i] =
                circleRawImages[i].transform.localScale;

            originalColors[i] =
                circleRawImages[i].color;
        }

        // ╭───────────────────────────────── ♡
        // │    Vergrößern & Grün werden
        // ╰───────────────────────────────── ♡

        float time = 0f;

        while(time < growDuration)
        {
            time += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    time / growDuration
                );

            t = Mathf.SmoothStep(0f, 1f, t);

            for(int i = 0; i < circleRawImages.Length; i++)
            {
                if(circleRawImages[i] == null)
                {
                    continue;
                }

                circleRawImages[i].transform.localScale =
                    Vector3.Lerp(
                        originalScales[i],
                        originalScales[i] * scaleMultiplier,
                        t
                    );

                circleRawImages[i].color =
                    Color.Lerp(
                        originalColors[i],
                        green,
                        t
                    );
            }

            yield return null;
        }

        // ── ♡  Grün halten  ♡ ──

        yield return new WaitForSeconds(holdDuration);

        // ╭───────────────────────────────── ♡
        // │    Grün ausblenden
        // ╰───────────────────────────────── ♡

        SpriteRenderer spriteRenderer =
            GetComponent<SpriteRenderer>();

        CanvasGroup canvasGroup =
            GetComponent<CanvasGroup>();

        time = 0f;

        while(time < fadeOutDuration)
        {
            time += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    time / fadeOutDuration
                );

            t = Mathf.SmoothStep(0f, 1f, t);

            float alpha =
                Mathf.Lerp(1f, 0f, t);

            // ── ♡  Ring ausblenden  ♡ ──

            if(spriteRenderer != null)
            {
                Color color = green;
                color.a = alpha;
                spriteRenderer.color = color;
            }

            // ── ♡  CanvasGroup ausblenden  ♡ ──

            if(canvasGroup != null)
            {
                canvasGroup.alpha = alpha;
            }

            // ── ♡  Moon-Ringe ausblenden  ♡ ──

            for(int i = 0; i < circleRawImages.Length; i++)
            {
                if(circleRawImages[i] == null)
                {
                    continue;
                }

                Color moonColor = green;
                moonColor.a = alpha;

                circleRawImages[i].color = moonColor;
            }

            yield return null;
        }

        // ── ♡  Originalwerte wiederherstellen  ♡ ──

        for(int i = 0; i < circleRawImages.Length; i++)
        {
            if(circleRawImages[i] == null)
            {
                continue;
            }

            circleRawImages[i].transform.localScale = originalScales[i];

            circleRawImages[i].color = originalColors[i];
        }

        flashCoroutine = null;

        Destroy(gameObject);
    }


    // ╭───────────────────────────────── ♡
    // │    Treffer- & Verfehlt-Animation
    // │    Vollständig KI-generiert.
    // ╰───────────────────────────────── ♡
    private IEnumerator MissAnimation()
    {
        Color red = new Color(
            1f,
            0f,
            0f,
            1f
        );

        SpriteRenderer spriteRenderer =
            GetComponent<SpriteRenderer>();

        CanvasGroup canvasGroup =
            GetComponent<CanvasGroup>();

        // ── ♡  Aktuelle Moon-Farben speichern  ♡ ──

        Color[] originalMoonColors =
            new Color[circleRawImages.Length];

        for(int i = 0; i < circleRawImages.Length; i++)
        {
            if(circleRawImages[i] == null)
            {
                continue;
            }

            originalMoonColors[i] =
                circleRawImages[i].color;
        }

        // ╭───────────────────────────────── ♡
        // │    Rot werden
        // ╰───────────────────────────────── ♡

        float time = 0f;

        while(time < redDuration)
        {
            time += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    time / redDuration
                );

            t = Mathf.SmoothStep(0f, 1f, t);

            // ── ♡  Ring rot  ♡ ──

            if(spriteRenderer != null)
            {
                Color color =
                    Color.Lerp(
                        originalRingColor,
                        red,
                        t
                    );

                color.a = 1f;
                spriteRenderer.color = color;
            }

            // ── ♡  Moon-Ringe rot  ♡ ──

            for(int i = 0; i < circleRawImages.Length; i++)
            {
                if(circleRawImages[i] == null)
                {
                    continue;
                }

                Color color =
                    Color.Lerp(
                        originalMoonColors[i],
                        red,
                        t
                    );

                color.a = 1f;
                circleRawImages[i].color = color;
            }

            yield return null;
        }

        // ╭───────────────────────────────── ♡
        // │    Rot halten
        // ╰───────────────────────────────── ♡

        if(spriteRenderer != null)
        {
            Color color = red;
            color.a = 1f;
            spriteRenderer.color = color;
        }

        for(int i = 0; i < circleRawImages.Length; i++)
        {
            if(circleRawImages[i] == null)
            {
                continue;
            }

            circleRawImages[i].color = red;
        }

        yield return new WaitForSeconds(redHoldDuration);

        // ╭───────────────────────────────── ♡
        // │    Rot ausblenden
        // ╰───────────────────────────────── ♡

        time = 0f;

        while(time < fadeOutDuration)
        {
            time += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    time / fadeOutDuration
                );

            t = Mathf.SmoothStep(0f, 1f, t);

            float alpha =
                Mathf.Lerp(1f, 0f, t);

            // ── ♡  Ring ausblenden  ♡ ──

            if(spriteRenderer != null)
            {
                Color color = red;
                color.a = alpha;

                spriteRenderer.color = color;
            }

            // ── ♡  CanvasGroup ausblenden  ♡ ──

            if(canvasGroup != null)
            {
                canvasGroup.alpha = alpha;
            }

            // ── ♡  Moon-Ringe ausblenden  ♡ ──

            for(int i = 0; i < circleRawImages.Length; i++)
            {
                if(circleRawImages[i] == null)
                {
                    continue;
                }

                Color color = red;
                color.a = alpha;

                circleRawImages[i].color = color;
            }

            yield return null;
        }

        // ── ♡  Ring löschen  ♡ ──

        flashCoroutine = null;

        Destroy(gameObject);
    }
}