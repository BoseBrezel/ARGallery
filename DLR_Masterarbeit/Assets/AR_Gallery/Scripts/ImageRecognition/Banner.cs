using UnityEngine;
using UnityEngine.UI;

public class Banner : MonoBehaviour
{
    // ╭───────────────────────────────── ♡
    // │    Banner
    // ╰───────────────────────────────── ♡

    // ── ♡  App Komponenten  ♡ ──

    public GameHandling gameHand;
    public GameHandling appControl;
    public RecognitionHandler recognitionHandler;

    // ── ♡  Freigeschalteter Banner  ♡ ──

    [SerializeField] private GameObject bannerBanner;
    [SerializeField] private GameObject bannerBase;
    [SerializeField] private GameObject bannerO_O;
    [SerializeField] private Button button;

    // ── ♡  Geblockter Banner  ♡ ──

    [SerializeField] private GameObject bannerBannerBlocked;
    [SerializeField] private GameObject bannerBaseBlocked;
    [SerializeField] private GameObject bannerO_OBlocked;
    [SerializeField] private Button buttonBlocked;

    // ── ♡  Sonstige UI  ♡ ──

    [SerializeField] private GameObject blocked;

    // ── ♡  Bannerstatus  ♡ ──

    // true = freigeschaltet
    // false = geblockt
    private bool isUnblocked = false;

    private bool infoOpen = false;

    public string pictureName;


    // ╭───────────────────────────────── ♡
    // │    Initialisierung
    // ╰───────────────────────────────── ♡

    private void Awake()
    {
        gameHand = FindFirstObjectByType<GameHandling>();
        recognitionHandler = FindFirstObjectByType<RecognitionHandler>();

        if(gameHand != null)
        {
            gameHand.RegisterBanner(this);
        }
    }

    private void Start()
    {
        button.onClick.AddListener(ToggleBanner);
        buttonBlocked.onClick.AddListener(ToggleBanner);
    }


    // ╭───────────────────────────────── ♡
    // │    Banner erstellen
    // ╰───────────────────────────────── ♡

    public void IniBanner(bool unblocked)
    {
        isUnblocked = unblocked;
        infoOpen = false;

        if(isUnblocked)
        {
            ShowSmallBanner();
        }
        else
        {
            ShowSmallBannerBlocked();
        }
    }


    // ╭───────────────────────────────── ♡
    // │    Banner öffnen & schließen
    // ╰───────────────────────────────── ♡

    public void ToggleBanner()
    {
        infoOpen = !infoOpen;

        if(isUnblocked)
        {
            if(infoOpen)
            {
                ShowInfoBanner();
            }
            else
            {
                ShowSmallBanner();
            }
        }
        else
        {
            if(infoOpen)
            {
                ShowInfoBannerBlocked();
            }
            else
            {
                ShowSmallBannerBlocked();
            }
        }
    }


    // ╭───────────────────────────────── ♡
    // │    Freigeschalteter Banner
    // ╰───────────────────────────────── ♡

    // ── ♡  Kleinen Banner anzeigen  ♡ ──

    public void ShowSmallBanner()
    {
        infoOpen = false;

        SetAll(false);

        button.gameObject.SetActive(true);
        bannerO_O.SetActive(true);
        bannerBase.SetActive(true);
    }


    // ── ♡  Info-Banner anzeigen  ♡ ──

    public void ShowInfoBanner()
    {
        infoOpen = true;

        SetAll(false);

        button.gameObject.SetActive(true);
        bannerO_O.SetActive(true);
        bannerBanner.SetActive(true);
    }


    // ╭───────────────────────────────── ♡
    // │    Geblockter Banner
    // ╰───────────────────────────────── ♡

    // ── ♡  Kleinen geblockten Banner anzeigen  ♡ ──

    public void ShowSmallBannerBlocked()
    {
        infoOpen = false;

        SetAll(false);

        buttonBlocked.gameObject.SetActive(true);
        bannerO_OBlocked.SetActive(true);
        bannerBaseBlocked.SetActive(true);
    }


    // ── ♡  Geblockten Info-Banner anzeigen  ♡ ──

    public void ShowInfoBannerBlocked()
    {
        infoOpen = true;

        SetAll(false);

        buttonBlocked.gameObject.SetActive(true);
        bannerO_OBlocked.SetActive(true);
        bannerBaseBlocked.SetActive(true);
    }


    // ╭───────────────────────────────── ♡
    // │    Banner entsperren
    // ╰───────────────────────────────── ♡

    public void UnblockBanner()
    {
        Debug.Log("Banner wird entsperrt: " + pictureName);

        isUnblocked = true;

        // RecognitionHandler merkt sich,
        // dass dieser Banner freigeschaltet wurde.
        if(recognitionHandler != null)
        {
            recognitionHandler.UnblockBanner(pictureName);
        }

        if(infoOpen)
        {
            ShowInfoBanner();
        }
        else
        {
            ShowSmallBanner();
        }
    }


    // ╭───────────────────────────────── ♡
    // │    Banner aktualisieren
    // ╰───────────────────────────────── ♡

    public void RefreshBanner()
    {
        if(isUnblocked)
        {
            if(infoOpen)
            {
                ShowInfoBanner();
            }
            else
            {
                ShowSmallBanner();
            }
        }
        else
        {
            if(infoOpen)
            {
                ShowInfoBannerBlocked();
            }
            else
            {
                ShowSmallBannerBlocked();
            }
        }
    }


    // ╭───────────────────────────────── ♡
    // │    Banner-Daten
    // ╰───────────────────────────────── ♡

    public string getPictureName()
    {
        return pictureName;
    }


    // ╭───────────────────────────────── ♡
    // │    Quest öffnen
    // ╰───────────────────────────────── ♡

    public void UnblockStarting()
    {
        if(gameHand != null)
        {
            gameHand.OpenQuest(pictureName);
        }
    }


    // ╭───────────────────────────────── ♡
    // │    Weitere Informationen
    // ╰───────────────────────────────── ♡

    public void OpenMore()
    {
        Debug.Log("OpenMore " + pictureName);
    }


    // ╭───────────────────────────────── ♡
    // │    UI verwalten
    // ╰───────────────────────────────── ♡

    private void SetAll(bool state)
    {
        button.gameObject.SetActive(state);
        bannerO_O.SetActive(state);
        bannerBase.SetActive(state);
        bannerBanner.SetActive(state);

        buttonBlocked.gameObject.SetActive(state);
        bannerO_OBlocked.SetActive(state);
        bannerBaseBlocked.SetActive(state);
        bannerBannerBlocked.SetActive(state);

        if(blocked != null)
        {
            blocked.SetActive(state);
        }
    }
}