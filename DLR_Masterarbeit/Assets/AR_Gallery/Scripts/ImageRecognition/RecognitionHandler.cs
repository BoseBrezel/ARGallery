using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class RecognitionHandler : MonoBehaviour
{
    // ╭───────────────────────────────── ♡
    // │    RecognitionHandler
    // ╰───────────────────────────────── ♡

    // ── ♡  Banner  ♡ ──

    [SerializeField]
    public Dictionary<string, GameObject> bannerDict;

    private Dictionary<TrackableId, GameObject> spawnedButtons = new Dictionary<TrackableId, GameObject>();

    [SerializeField]
    private List<GameObject> bannerPrefabs;

    // Hier werden alle freigeschalteten Banner gespeichert
    [SerializeField]
    private List<string> unblockedBanners = new List<string>();

    // ── ♡  Calibration  ♡ ──

    public float x = 0.0f;
    public float y = 0.0f;
    public float z = 0.0f;
    public float size2 = 0.0006f;

    public TMP_Text inputX;
    public TMP_Text inputY;
    public TMP_Text inputZ;
    public TMP_Text inputSize;

    public bool unBlocked;

    // ── ♡  AR Setup  ♡ ──

    [SerializeField]
    private ARSession arSession;

    [SerializeField]
    private ARTrackedImageManager imageManager;

    [SerializeField]
    private Transform spawnArea;


    // ╭───────────────────────────────── ♡
    // │    Initialisierung
    // ╰───────────────────────────────── ♡

    private void Awake()
    {
        bannerDict = new Dictionary<string, GameObject>();

        foreach(GameObject prefab in bannerPrefabs)
        {
            Banner banner = prefab.GetComponent<Banner>();

            if(banner != null)
            {
                string key = banner.getPictureName();

                if(!bannerDict.ContainsKey(key))
                {
                    bannerDict.Add(key, prefab);
                }
            }
        }

        UpdateCalibrationText();
    }


    private void OnEnable()
    {
        imageManager.trackablesChanged.AddListener(OnChanged);
    }


    private void OnDisable()
    {
        imageManager.trackablesChanged.RemoveListener(OnChanged);
    }


    // ╭───────────────────────────────── ♡
    // │    Banner freischalten
    // ╰───────────────────────────────── ♡

    public void UnblockBanner(string bannerName)
    {
        if(string.IsNullOrEmpty(bannerName))
        {
            Debug.LogWarning("UnblockBanner: bannerName ist leer.");
            return;
        }

        if(!unblockedBanners.Contains(bannerName))
        {
            unblockedBanners.Add(bannerName);
            Debug.Log("Banner freigeschaltet: " + bannerName);
        }
        else
        {
            Debug.Log("Banner war bereits freigeschaltet: " + bannerName);
        }
    }


    private bool IsBannerUnblocked(string bannerName)
    {
        return unblockedBanners != null && unblockedBanners.Contains(bannerName);
    }


    public void putUnblockBanners(List<string> unblockedList)
    {
        if(unblockedList == null)
        {
            unblockedBanners = new List<string>();
        }
        else
        {
            unblockedBanners = new List<string>(unblockedList);
        }
    }


    public void UnblockAllBanners()
    {
        unblockedBanners.Clear();

        foreach(string bannerName in bannerDict.Keys)
        {
            if(!unblockedBanners.Contains(bannerName))
            {
                unblockedBanners.Add(bannerName);
            }
        }
    }


    public void setBlockState(bool stateBlock)
    {
        unBlocked = stateBlock;

        if(unBlocked)
        {
            UnblockAllBanners();
        }
    }

    // ╭───────────────────────────────── ♡
    // │    AR Tracking
    // ╰───────────────────────────────── ♡

    private void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach(var img in args.added)
        {
            BannerSpawn(img);
        }

        foreach(var img in args.updated)
        {
            BannerUpdate(img);
        }

        foreach(var img in args.removed)
        {
            BannerRemove(img.Value);
        }
    }


    // ── ♡  Banner erzeugen  ♡ ──

    private void BannerSpawn(ARTrackedImage image)
    {
        if(image.referenceImage == null)
        {
            return;
        }

        string imageName = image.referenceImage.name;

        if(string.IsNullOrEmpty(imageName))
        {
            return;
        }

        // ── ♡  Prefab suchen  ♡ ──

        if(!bannerDict.TryGetValue(imageName, out GameObject prefab))
        {
            Debug.Log("BannerDict Error: " + imageName);
            return;
        }

        // ── ♡  Freischaltungsstatus prüfen  ♡ ──

        bool isUnblocked = IsBannerUnblocked(imageName);


        if(spawnedButtons.ContainsKey(image.trackableId))
        {
            Debug.Log("Banner existiert bereits.");
            return;
        }

        // ── ♡  Banner erzeugen  ♡ ──

        GameObject spawned = Instantiate(prefab, spawnArea);

        Banner banner = spawned.GetComponent<Banner>();

        if(banner == null)
        {
            Debug.LogError( "Banner-Komponente fehlt auf: " +imageName );

            Destroy(spawned);
            return;
        }

        // ── ♡  RecognitionHandler setzen  ♡ ──

        banner.recognitionHandler = this;

        if(unBlocked)
        {
            banner.IniBanner(true);
        }
        else
        {
            banner.IniBanner(isUnblocked);
        }

        // ── ♡  Position setzen  ♡ ──

        UpdateTransform(spawned, image);

        spawnedButtons.Add(image.trackableId, spawned);
    }


    // ── ♡  Banner aktualisieren  ♡ ──

    private void BannerUpdate(ARTrackedImage image)
    {
        TrackableId id = image.trackableId;

        if(!spawnedButtons.ContainsKey(id))
        {
            BannerSpawn(image);
            return;
        }

        UpdateTransform(spawnedButtons[id], image);
    }


    // ── ♡  Banner entfernen  ♡ ──

    private void BannerRemove(ARTrackedImage image)
    {
        TrackableId id = image.trackableId;

        if(spawnedButtons.ContainsKey(id))
        {
            Destroy(spawnedButtons[id]);
            spawnedButtons.Remove(id);
        }
    }


    // ╭───────────────────────────────── ♡
    // │    Banner aktualisieren
    // ╰───────────────────────────────── ♡

    public void RefreshAllBanners()
    {
        foreach(KeyValuePair<TrackableId, GameObject> pair in spawnedButtons)
        {
            Banner banner = pair.Value.GetComponent<Banner>();

            if(banner == null)
            {
                continue;
            }

            // Der individuelle Zustand des Banners bleibt erhalten.
            banner.RefreshBanner();
        }
    }


    // ╭───────────────────────────────── ♡
    // │    AR zurücksetzen
    // ╰───────────────────────────────── ♡

    public void BannerReset()
    {
        StartCoroutine(ResetAR());
    }


    private IEnumerator ResetAR()
    {
        imageManager.enabled = false;

        foreach(KeyValuePair<TrackableId, GameObject> pair in spawnedButtons)
        {
            if(pair.Value != null)
            {
                Destroy(pair.Value);
            }
        }

        spawnedButtons.Clear();
        yield return null;

        // ── ♡  AR Session zurücksetzen  ♡ ──
        if(arSession != null)
        {
            arSession.Reset();
        }

        yield return null;
        imageManager.enabled = true;
    }


    // ╭───────────────────────────────── ♡
    // │    Transform
    // ╰───────────────────────────────── ♡

    private void UpdateTransform(GameObject obj,ARTrackedImage image)
    {
        BannerPosition bannerPosition = obj.GetComponent<BannerPosition>();

        if(bannerPosition == null)
        {
            Debug.Log("BannerPosition fehlt");
            return;
        }

        float bannerX = bannerPosition.X;
        float bannerY = bannerPosition.Y;
        float bannerZ = bannerPosition.Z;
        float bannerSize = bannerPosition.Size;

        float finalX = bannerX + x;
        float finalY = bannerY + y;
        float finalZ = bannerZ + z;
        float finalSize = bannerSize;

        obj.transform.localScale = Vector3.one * finalSize;

        obj.transform.position =
            image.transform.position
            + image.transform.right * finalX
            + image.transform.up * finalY
            + image.transform.forward * finalZ;

        obj.transform.LookAt(Camera.main.transform);
        obj.transform.Rotate(0, 180f, 0);

        UpdateCalibrationText();
    }


    // ╭───────────────────────────────── ♡
    // │    Calibration
    // ╰───────────────────────────────── ♡

    private void UpdateCalibrationText()
    {
        inputX.text = x.ToString("F2");
        inputY.text = y.ToString("F2");
        inputZ.text = z.ToString("F2");
        inputSize.text = size2.ToString("F5");
    }


    // ── ♡  X-Achse  ♡ ──

    public void PlusX()
    {
        x += 0.05f;
        UpdateCalibrationText();
    }


    public void MinusX()
    {
        x -= 0.05f;
        UpdateCalibrationText();
    }


    // ── ♡  Y-Achse  ♡ ──

    public void PlusY()
    {
        y += 0.05f;
        UpdateCalibrationText();
    }


    public void MinusY()
    {
        y -= 0.05f;
        UpdateCalibrationText();
    }


    // ── ♡  Z-Achse  ♡ ──

    public void PlusZ()
    {
        z += 0.05f;
        UpdateCalibrationText();
    }


    public void MinusZ()
    {
        z -= 0.05f;
        UpdateCalibrationText();
    }


    // ── ♡  Größe  ♡ ──

    public void PlusSize()
    {
        size2 += 0.00005f;
        UpdateCalibrationText();
    }


    public void MinusSize()
    {
        size2 -= 0.00005f;
        UpdateCalibrationText();
    }
}