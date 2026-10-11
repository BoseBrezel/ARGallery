using UnityEngine;
using System.Collections.Generic;

public class GameHandling : MonoBehaviour
{
    // ╭───────────────────────────────── ♡
    // │    GameHandling
    // ╰───────────────────────────────── ♡

    // ── ♡  App & Spiel  ♡ ──

    [SerializeField] private AppControl appController;
    [SerializeField] private GameObject finishScreen;
    [SerializeField] private GameLogic gameLogic;

    // ── ♡  Quiz  ♡ ──

    [SerializeField] private GameObject quizPrefab;
    [SerializeField] private Transform quizArea;

    private Quiz currentQuiz;
    private List<Quiz> quizes = new List<Quiz>();
    private QuizCollection quizData;

    // ── ♡  Banner & Badges  ♡ ──

    public List<Banner> banners = new List<Banner>();
    [SerializeField] private List<Badges> badges = new List<Badges>();

    // ╭───────────────────────────────── ♡
    // │    Initialisierung
    // ╰───────────────────────────────── ♡

    private void Start()
    {
        CreateQuizes();
    }

    // ╭───────────────────────────────── ♡
    // │    Banner
    // ╰───────────────────────────────── ♡

    public void RegisterBanner(Banner banner)
    {
        banners.Add(banner);
    }

    public void UnblockBanner(string pictureName)
    {
        foreach(Banner banner in banners)
        {
            if(banner.pictureName == pictureName)
            {
                banner.UnblockBanner();
                ActivateBadge(pictureName);
                appController.putBanner(pictureName);
                AreAllBadgesUnlocked();

                return;
            }
        }

        Debug.Log("Kein Banner gefunden: " + pictureName);
    }

    public void UnblockAllBanners()
    {
        foreach(Banner banner in banners)
        {
            if(banner != null)
            {
                Debug.Log("Banner " + banner.pictureName);

                banner.UnblockBanner();
                ActivateBadge(banner.pictureName);
            }
        }

        Debug.Log("Alle Banner wurden entblockt.");
    }

    // ╭───────────────────────────────── ♡
    // │    Badges
    // ╰───────────────────────────────── ♡

    public void ActivateBadge(string banner)
    {
        foreach(Badges badge in badges)
        {
            if(badge.BadgeBanner == banner)
            {
                badge.ActivateBadge();
                return;
            }
        }

        Debug.Log("Kein Badge gefunden: ");
    }

    public void ActivateBadgeWithoutAnimation(string banner)
    {
        foreach(Badges badge in badges)
        {
            if(badge.BadgeBanner == banner)
            {
                badge.ActivateBadgeWithoutAnimation();
                return;
            }
        }

        Debug.Log("Kein Badge gefunden: ");
    }

    public bool AreAllBadgesUnlocked()
    {
        foreach(Badges badge in badges)
        {
            if(badge == null)
            {
                continue;
            }

            if(!badge.IsUnlocked())
            {
                return false;
            }
        }

        finishScreen.SetActive(true);

        return true;
    }

    // ╭───────────────────────────────── ♡
    // │    Quiz erstellen
    // ╰───────────────────────────────── ♡

    public void CreateQuizes()
    {
        TextAsset json = Resources.Load<TextAsset>("quiz");
        quizData = JsonUtility.FromJson<QuizCollection>(json.text);

        CreateQuiz("AlbertEinstein", quizData.AlbertEinstein);
        CreateQuiz("WernerVonSiemens", quizData.WernerVonSiemens);
        CreateQuiz("AmaliaMaryEarhart", quizData.AmaliaMaryEarhart);
        CreateQuiz("Buzz", quizData.Buzz);
        CreateQuiz("CarlBenz", quizData.CarlBenz);
        CreateQuiz("CaecilleBenz", quizData.CaecilleBenz);
        CreateQuiz("Derwittzer", quizData.Derwittzer);
        CreateQuiz("galileoMonde", quizData.galileoMonde);
        CreateQuiz("GalileoGalilei", quizData.GalileoGalilei);
        CreateQuiz("ISS", quizData.ISS);
        CreateQuiz("LaJamaisContente", quizData.LaJamaisContente);
        CreateQuiz("Hamilton", quizData.Hamilton);
        CreateQuiz("MarieCurie", quizData.MarieCurie);
        CreateQuiz("LiseMeitner", quizData.LiseMeitner);
        CreateQuiz("MarsExpress", quizData.MarsExpress);
        CreateQuiz("Otto", quizData.Otto);
        CreateQuiz("IsaacNewton", quizData.IsaacNewton);
        CreateQuiz("SpaceShuttle", quizData.SpaceShuttle);
    }

    private void CreateQuiz(string name, QuizEntry data)
    {
        GameObject obj = Instantiate(quizPrefab, quizArea);

        Quiz quiz = obj.GetComponent<Quiz>();
        quiz.Setup(name, data);

        quizes.Add(quiz);
    }

    // ╭───────────────────────────────── ♡
    // │    Quiz öffnen
    // ╰───────────────────────────────── ♡

    public void OpenQuest(string bannerName)
    {
        foreach(Quiz quiz in quizes)
        {
            if(quiz.quizName == bannerName)
            {
                if(currentQuiz != null && currentQuiz != quiz)
                {
                    currentQuiz.Close();
                }

                currentQuiz = quiz;

                switch(quiz.quizName)
                {
                    case "Hamilton":
                        quiz.Open();
                        gameLogic.GameMargaretHamilton();
                        break;

                    case "GalileoGalilei":
                        quiz.Open();
                        gameLogic.GameGaliMoons();
                        break;

                    case "MarieCurie":
                        quiz.Open();
                        gameLogic.MarieCurieGame();
                        break;

                    case "Otto":
                        gameLogic.GameOttoLilienthal();
                        break;

                    case "LaJamaisContente":
                        gameLogic.GameContente();
                        break;

                    case "MarsExpress":
                        gameLogic.GameThreeD();
                        break;

                    case "IsaacNewton":
                        gameLogic.GameSirIsaac();
                        break;

                    default:
                        quiz.Open();
                        break;
                }

                return;
            }
        }

        Debug.LogWarning("Quiz nicht gefunden: " + bannerName);
    }

    // ╭───────────────────────────────── ♡
    // │    Quiz schließen
    // ╰───────────────────────────────── ♡

    public void CloseCurrentQuiz()
    {
        if(currentQuiz != null)
        {
            currentQuiz.Close();
            currentQuiz = null;
        }
    }

    public void CloseGameForQuiz(string quizName)
    {
        switch(quizName)
        {
            case "Hamilton":
                gameLogic.CloseGameMargaretHamilton();
                break;

            case "MarieCurie":
                gameLogic.CloseMarieCurieGame();
                break;

            case "GalileoGalilei":
                gameLogic.CloseAllGames();
                break;
        }

        if(currentQuiz != null && currentQuiz.quizName == quizName)
        {
            currentQuiz = null;
        }
    }
}