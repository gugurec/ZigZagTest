using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GamemanagerBehaviour : MonoBehaviour
{
    public static GamemanagerBehaviour Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<GamemanagerBehaviour>();
            return _instance;
        }
    }

    public static CrystalGenerationRule Rule // правила для генерации кристалов
    {
        get
        {
            return crystalGenerationRule;
        }
    }
    public static int Period // периодичность появления кристалов
    {
        get
        {
            return _crystalPeriod;
        }
    }

    public static Difficulty GameDifficulty // игровая сложность (влияет на ширину дорожки)
    {
        get
        {
            return _difficulty;
        }
    }

    public enum Difficulty
    {
        None = 0,
        Hard = 1,
        Medium = 2,
        Easy = 3
    }

    public enum CrystalGenerationRule
    {
        None = 0,
        Random = 1,
        Period = 2
    }
    public delegate void ScoreChanged();
    public event ScoreChanged OnScoreChanged;

    [SerializeField]
    private static Difficulty _difficulty = Difficulty.Hard;
    [SerializeField]
    private static CrystalGenerationRule crystalGenerationRule = CrystalGenerationRule.Random;
    [SerializeField]
    private static int _crystalPeriod = 5;
    [SerializeField]
    private MapBehaviour _map;//Ссылка на ГО карты.

    private static GamemanagerBehaviour _instance;
    
    private int _scoreCounter = 0; //текущие набранные очки

    #region Public Metods
    public void AddScore(int score)
    {
        SetCurrentScore(_scoreCounter += score);
    }

    public int GetCurrentScore()
    {
        return _scoreCounter;
    }

    public void GameOver()
    {
        MainHUDBehaviour.Instance.ShowGameOverPanel();
    }

    public void InitGame()
    {
        _map.InitMap();
        MainHUDBehaviour.Instance.ShowStartPanel();
        PauseGame();
    }

    public void StartGame()
    {
        MainHUDBehaviour.Instance.ShowGamePanel();
        UnpauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        _map.ClearMap();
        _map.InitMap();
        SetCurrentScore(0);
        MainHUDBehaviour.Instance.ShowStartPanel();
        PauseGame();
    }
    #endregion

    private void SetCurrentScore(int score)
    {
        _scoreCounter = score;
        OnScoreChanged?.Invoke();
    }


    void Start()
    {
        InitGame();
    }
}
