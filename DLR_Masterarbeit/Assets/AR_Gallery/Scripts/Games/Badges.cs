using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Badges : MonoBehaviour
{
    // ╭───────────────────────────────── ♡
    // │    Badges
    // ╰───────────────────────────────── ♡

    // ── ♡  Badge UI  ♡ ──

    [SerializeField] private GameObject badgeActive;
    [SerializeField] private GameObject badgeInActive;
    [SerializeField] private GameObject parentForAnimation;
    [SerializeField] private RawImage information;

    // ── ♡  Badge Daten  ♡ ──

    public string BadgeBanner;

    // ── ♡  Animation  ♡ ──

    [SerializeField] private float waitTime = 3f;
    [SerializeField] private float duration = 2f;

    [SerializeField] private Vector2 startPosition = new Vector2(0f, 0f);
    [SerializeField] private Vector2 endPosition = new Vector2(240.8f, -358f);

    [SerializeField] private float startScale = 1f;
    [SerializeField] private float endScale = 0.4f;


    // ╭───────────────────────────────── ♡
    // │    Badge aktivieren
    // ╰───────────────────────────────── ♡

    public void ActivateBadge()
    {
        bool badgeActivBool = badgeActive.activeSelf;

        if(badgeActivBool)
        {
            return;
        }

        badgeActive.SetActive(true);

        GameObject animatedBadge = Instantiate(badgeActive, parentForAnimation.transform);

        animatedBadge.name = badgeActive.name + "_Animation";
        animatedBadge.SetActive(true);

        CoroutineHost host = parentForAnimation.GetComponent<CoroutineHost>();

        if(host == null)
        {
            host = parentForAnimation.AddComponent<CoroutineHost>();
        }

        host.StartCoroutine(AnimateBadge(animatedBadge));
    }


    // ╭───────────────────────────────── ♡
    // │    Badge Status
    // ╰───────────────────────────────── ♡

    public bool IsUnlocked()
    {
        return badgeActive.activeSelf;
    }


    // ╭───────────────────────────────── ♡
    // │    Badge ohne Animation aktivieren
    // ╰───────────────────────────────── ♡

    public void ActivateBadgeWithoutAnimation()
    {
        bool badgeActivBool = badgeActive.activeSelf;

        if(badgeActivBool)
        {
            return;
        }

        badgeActive.SetActive(true);
    }


    // ╭─────────────────────────────────────────────────────────────────────────────────────────────────── ♡
    // │    Badge Animation -  Abschnitt wurde komplett mit KI-erstellt
    // ╰─────────────────────────────────────────────────────────────────────────────────────────────────── ♡

    private IEnumerator AnimateBadge(GameObject animatedBadge)
    {
        CanvasGroup informationCanvasGroup = null;

        // ── ♡  Information vorbereiten  ♡ ──

        if(information != null)
        {
            informationCanvasGroup =
                information.GetComponent<CanvasGroup>();

            if(informationCanvasGroup == null)
            {
                informationCanvasGroup =
                    information.gameObject.AddComponent<CanvasGroup>();
            }

            informationCanvasGroup.alpha = 0f;
            information.gameObject.SetActive(false);
        }

        if(information != null)
        {
            information.gameObject.SetActive(true);
            informationCanvasGroup.alpha = 1f;
        }


        // ── ♡  Badge vorbereiten  ♡ ──

        RectTransform rectTransform =
            animatedBadge.GetComponent<RectTransform>();

        if(rectTransform == null)
        {
            Destroy(animatedBadge);
            yield break;
        }


        // ── ♡  CanvasGroup erstellen  ♡ ──

        CanvasGroup badgeCanvasGroup = animatedBadge.GetComponent<CanvasGroup>();

        if(badgeCanvasGroup == null)
        {
            badgeCanvasGroup = animatedBadge.AddComponent<CanvasGroup>();
        }

        // ── ♡  Startzustand  ♡ ──

        rectTransform.anchoredPosition = startPosition;
        rectTransform.localScale = Vector3.one * startScale;

        badgeCanvasGroup.alpha = 1f;

        // ╭───────────────────────────────── ♡
        // │    Warten
        // ╰───────────────────────────────── ♡

        yield return new WaitForSeconds(waitTime);

        // ╭───────────────────────────────── ♡
        // │    Phase 1 – Bewegung & Scale
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

            // Badge bleibt sichtbar
            badgeCanvasGroup.alpha = 1f;

            yield return null;
        }


        // ── ♡  Endzustand Bewegung  ♡ ──

        rectTransform.anchoredPosition = endPosition;
        rectTransform.localScale = Vector3.one * endScale;
        badgeCanvasGroup.alpha = 1f;


        // ╭───────────────────────────────── ♡
        // │    Phase 2 – Ausblenden
        // ╰───────────────────────────────── ♡

        timer = 0f;

        while(timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);

            // ── ♡  Badge ausblenden  ♡ ──

            badgeCanvasGroup.alpha =
                Mathf.Lerp(
                    1f,
                    0f,
                    t
                );

            // ── ♡  Information ausblenden  ♡ ──

            if(information != null)
            {
                informationCanvasGroup.alpha =
                    Mathf.Lerp(
                        1f,
                        0f,
                        t
                    );
            }

            yield return null;
        }


        // ╭───────────────────────────────── ♡
        // │    Animation beenden
        // ╰───────────────────────────────── ♡

        badgeCanvasGroup.alpha = 0f;

        if(information != null)
        {
            informationCanvasGroup.alpha = 0f;

            information.gameObject.SetActive(false);
        }


        // ── ♡  Animationskopie löschen  ♡ ──

        Destroy(animatedBadge);
    }
    // ╭─────────────────────────────────────────────────────────────────────────────────────────────────── ♡
    // │  Bis hier wurde der Inhalt mit Ki erstellt
    // ╰─────────────────────────────────────────────────────────────────────────────────────────────────── ♡
}