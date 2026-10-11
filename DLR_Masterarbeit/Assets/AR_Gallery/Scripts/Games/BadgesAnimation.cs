using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// ╭────────────────────────────────────────── ♡
// │    BadgeAnimation
// │
// │    KI-Kennzeichnung:
// │    Diese Klasse wurde KI-generiert und anschließend angepasst.
// ╰────────────────────────────────────────── ♡

public class BadgeAnimation : MonoBehaviour
{
    // ╭───────────────────────────────── ♡
    // │    BadgeAnimation
    // ╰───────────────────────────────── ♡

    // ── ♡  Animation  ♡ ──

    [SerializeField] private float waitTime = 3f;
    [SerializeField] private float duration = 2f;
    [SerializeField] private float fadeDuration = 1f;

    [SerializeField] private Vector2 startPosition = new Vector2(0f, 0f);
    [SerializeField] private Vector2 endPosition = new Vector2(240.8f, -358f);

    [SerializeField] private float startScale = 1f;
    [SerializeField] private float endScale = 0.4f;

    // ── ♡  UI Elemente  ♡ ──

    [SerializeField] private RawImage backgroundImage;

    // ── ♡  Komponenten  ♡ ──

    private RectTransform rectTransform;
    private CanvasGroup badgeCanvasGroup;
    private CanvasGroup imageCanvasGroup;

    // ── ♡  Coroutine  ♡ ──

    private Coroutine animationCoroutine;


    // ╭───────────────────────────────── ♡
    // │    Initialisierung
    // ╰───────────────────────────────── ♡

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        badgeCanvasGroup = GetComponent<CanvasGroup>();

        if(badgeCanvasGroup == null)
        {
            badgeCanvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        if(backgroundImage != null)
        {
            imageCanvasGroup =
                backgroundImage.GetComponent<CanvasGroup>();

            if(imageCanvasGroup == null)
            {
                imageCanvasGroup =
                    backgroundImage.gameObject.AddComponent<CanvasGroup>();
            }
        }
    }


    // ╭───────────────────────────────── ♡
    // │    Animation starten
    // ╰───────────────────────────────── ♡

    public void PlayAnimation()
    {
        if(animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(AnimateBadge());
    }


    // ╭───────────────────────────────── ♡
    // │    Badge Animation
    // ╰───────────────────────────────── ♡

    private IEnumerator AnimateBadge()
    {
        // ── ♡  Startzustand  ♡ ──

        rectTransform.anchoredPosition = startPosition;
        rectTransform.localScale = Vector3.one * startScale;

        badgeCanvasGroup.alpha = 1f;

        if(imageCanvasGroup != null)
        {
            imageCanvasGroup.alpha = 1f;
        }


        // ── ♡  Warten  ♡ ──

        yield return new WaitForSeconds(waitTime);


        // ╭───────────────────────────────── ♡
        // │    Bewegung & Ausblenden
        // ╰───────────────────────────────── ♡

        float timer = 0f;

        while(timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);

            // ── ♡  Position  ♡ ──

            rectTransform.anchoredPosition =
                Vector2.Lerp(
                    startPosition,
                    endPosition,
                    t
                );

            // ── ♡  Scale  ♡ ──

            float scale =
                Mathf.Lerp(
                    startScale,
                    endScale,
                    t
                );

            rectTransform.localScale =
                Vector3.one * scale;

            // ── ♡  Badge ausblenden  ♡ ──

            badgeCanvasGroup.alpha =
                Mathf.Lerp(
                    1f,
                    0f,
                    t
                );

            yield return null;
        }


        // ── ♡  Endzustand Badge  ♡ ──

        rectTransform.anchoredPosition = endPosition;
        rectTransform.localScale = Vector3.one * endScale;
        badgeCanvasGroup.alpha = 0f;


        // ╭───────────────────────────────── ♡
        // │    Hintergrund ausblenden
        // ╰───────────────────────────────── ♡

        if(imageCanvasGroup != null)
        {
            timer = 0f;

            while(timer < fadeDuration)
            {
                timer += Time.deltaTime;

                float t =
                    Mathf.Clamp01(
                        timer / fadeDuration
                    );

                imageCanvasGroup.alpha =
                    Mathf.Lerp(
                        1f,
                        0f,
                        t
                    );

                yield return null;
            }

            imageCanvasGroup.alpha = 0f;
        }


        // ── ♡  Animation beenden  ♡ ──

        Destroy(gameObject);
    }
}