using System;
using System.Collections.Generic;
using UnityEngine;

// ╭───────────────────────────────── ♡
// │    AnswerData
// ╰───────────────────────────────── ♡

[Serializable]
public class AnswerData
{
    public string question;
    public string answer;
}


// ╭───────────────────────────────── ♡
// │    EndQuizData
// ╰───────────────────────────────── ♡

[Serializable]
public class EndQuizData
{
    public string quizName;
    public string question;
    public string selectedAnswer;
    public string correctAnswer;
    public int score;
}


// ╭───────────────────────────────── ♡
// │    SurveyResultData
// ╰───────────────────────────────── ♡

[Serializable]
public class SurveyResultData
{
    // ── ♡  Allgemeine Daten  ♡ ──

    public string name;
    public string date;
    public string gameState;

    // ── ♡  Persönliche Daten  ♡ ──

    public int age;
    public string gender;

    // ── ♡  Survey-Antworten  ♡ ──

    public List<AnswerData> surveyAnswers = new List<AnswerData>();

    // ── ♡  EndQuiz-Antworten  ♡ ──

    public List<EndQuizData> endQuizAnswers = new List<EndQuizData>();

    // ── ♡  Freitext & Ergebnis  ♡ ──

    public string freeText;
    public int totalScore;
}


// ╭───────────────────────────────── ♡
// │    QuizData
// ╰───────────────────────────────── ♡

[Serializable]
public class QuizData
{
    public List<SurveyResultData> surveys = new List<SurveyResultData>();
}