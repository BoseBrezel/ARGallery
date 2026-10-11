using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ThreeDPicutures : MonoBehaviour
{
    // ╭───────────────────────────────── ♡
    // │    ThreeDPicutures
    // ╰───────────────────────────────── ♡

    // ── ♡  Bilder  ♡ ──

    [SerializeField] private RawImage pictureFieldLeft1;
    [SerializeField] private RawImage pictureFieldRight2;
    [SerializeField] private RectTransform fotoArea;

    // ── ♡  Kalibrierung  ♡ ──

    [SerializeField] private Vector3 calibrationPosition = new Vector3(0f, -60f, 0f);

    [SerializeField] private float calibrationScale = 1.15f;

    // ── ♡  Bildpositionen  ♡ ──

    [SerializeField] private RectTransform leftRect;
    [SerializeField] private RectTransform rightRect;

    [SerializeField] private Vector3 initialLeftPosition;
    [SerializeField] private Vector3 initialRightPosition;

    [SerializeField] private Vector3 initialLeftScale;
    [SerializeField] private Vector3 initialRightScale;

    // ── ♡  Bildreihenfolge  ♡ ──

    private int initialLeftSiblingIndex;
    private int initialRightSiblingIndex;

    // ── ♡  Originalbilder  ♡ ──

    private Texture initialLeftTexture;
    private Texture initialRightTexture;

    // ── ♡  GameLogic  ♡ ──

    private GameLogic gameLogic;

    // ╭───────────────────────────────── ♡
    // │    Initialisierung
    // ╰───────────────────────────────── ♡
    private void Awake()
    {
        gameLogic = FindFirstObjectByType<GameLogic>();
        
        leftRect = pictureFieldLeft1.GetComponent<RectTransform>();
        rightRect = pictureFieldRight2.GetComponent<RectTransform>();

        initialLeftPosition = leftRect.localPosition;
        initialRightPosition = rightRect.localPosition;

        initialLeftScale = leftRect.localScale;
        initialRightScale = rightRect.localScale;

        initialLeftSiblingIndex = leftRect.GetSiblingIndex();
        initialRightSiblingIndex = rightRect.GetSiblingIndex();

        initialLeftTexture = pictureFieldLeft1.texture;
        initialRightTexture = pictureFieldRight2.texture;
    }

    // ╭───────────────────────────────── ♡
    // │    Spielsteuerung
    // ╰───────────────────────────────── ♡

    public void CloseGame()
    {
        gameLogic.CloseAllGames();
    }

    public void WonGame()
    {
        gameLogic.WonGame("MarsExpress");
    }


    // ╭───────────────────────────────── ♡
    // │    Foto aufnehmen
    // │    Vollständig KI-generiert.
    // ╰───────────────────────────────── ♡
    public void TakePicture()
    {
        StartCoroutine(TakePictureCoroutine());
    }

    private IEnumerator TakePictureCoroutine()
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenshot = TakeAreaScreenshot();

        if(screenshot == null)
        {
            yield break;
        }

        if(pictureFieldLeft1.texture == null)
        {
            pictureFieldLeft1.texture = screenshot;
        }
        else if(pictureFieldRight2.texture == null)
        {
            pictureFieldRight2.texture = screenshot;
        }
        else
        {
            Destroy(screenshot);
        }
    }

    // ╭───────────────────────────────── ♡
    // │    Bild einfärben
    // │    Vollständig KI-generiert.
    // ╰───────────────────────────────── ♡
    private void ColorPicture()
    {
        ColorizePicture( pictureFieldLeft1,true,false,false);

        ColorizePicture(pictureFieldRight2,false,true,true);
    }

    private void ColorizePicture(RawImage image,bool red,bool green,bool blue)
    {
        if(image.texture == null)
        {
            return;
        }

        Texture2D texture =
            image.texture as Texture2D;

        if(texture == null)
        {
            return;
        }

        Color[] pixels = texture.GetPixels();

        for(int i = 0; i < pixels.Length; i++)
        {
            Color pixel = pixels[i];

            pixels[i] =
                new Color(
                    red ? pixel.r : 0f,
                    green ? pixel.g : 0f,
                    blue ? pixel.b : 0f,
                    pixel.a
                );
        }

        texture.SetPixels(pixels);
        texture.Apply();
    }

    // ╭───────────────────────────────── ♡
    // │    Bild kalibrieren
    // ╰───────────────────────────────── ♡

    private void CalibrateStartPicture()
    {
        if(leftRect == null || rightRect == null)
        {
            return;
        }

        leftRect.localPosition = calibrationPosition;
        rightRect.localPosition = calibrationPosition;

        leftRect.localScale = Vector3.one * calibrationScale;

        rightRect.localScale = Vector3.one * calibrationScale;
    }

    // ╭───────────────────────────────── ♡
    // │    Foto aufnehmen
    // │    Vollständig KI-generiert.
    // ╰───────────────────────────────── ♡
    private Texture2D TakeAreaScreenshot()
    {
        Vector3[] corners = new Vector3[4];
        fotoArea.GetWorldCorners(corners);

        int x =
            Mathf.RoundToInt(corners[0].x);

        int y =
            Mathf.RoundToInt(corners[0].y);

        int width =
            Mathf.RoundToInt(
                corners[2].x - corners[0].x
            );

        int height =
            Mathf.RoundToInt(
                corners[2].y - corners[0].y
            );

        x = Mathf.Clamp(
            x,
            0,
            Screen.width
        );

        y = Mathf.Clamp(
            y,
            0,
            Screen.height
        );

        width = Mathf.Clamp(
            width,
            0,
            Screen.width - x
        );

        height = Mathf.Clamp(
            height,
            0,
            Screen.height - y
        );

        if(width <= 0 || height <= 0)
        {
            return null;
        }

        Texture2D texture =
            new Texture2D(
                width,
                height,
                TextureFormat.RGB24,
                false
            );

        texture.ReadPixels(
            new Rect(
                x,
                y,
                width,
                height
            ),
            0,
            0
        );

        texture.Apply();

        return texture;
    }

    // ╭───────────────────────────────── ♡
    // │    Spiel zurücksetzen
    // ╰───────────────────────────────── ♡

    private void Redo()
    {
        if(pictureFieldLeft1.texture != null && pictureFieldLeft1.texture != initialLeftTexture)
        {
            Destroy(pictureFieldLeft1.texture);
        }

        if(pictureFieldRight2.texture != null && pictureFieldRight2.texture != initialRightTexture)
        {
            Destroy(pictureFieldRight2.texture);
        }

        pictureFieldLeft1.texture = initialLeftTexture;

        pictureFieldRight2.texture = initialRightTexture;

        leftRect.localPosition = initialLeftPosition;

        rightRect.localPosition = initialRightPosition;

        leftRect.localScale = initialLeftScale;

        rightRect.localScale = initialRightScale;
    }
}