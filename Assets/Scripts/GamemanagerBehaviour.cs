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

    public static CrystalGenerationRule Rule
    {
        get
        {
            return crystalGenerationRule;
        }
    }
    public static int Period
    {
        get
        {
            return _crystalPeriod;
        }
    }

    public static Difficulty GameDifficulty
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

    [SerializeField]
    private float Speed = 2.0f;
    [SerializeField]
    private static Difficulty _difficulty = Difficulty.Hard;
    [SerializeField]
    private static CrystalGenerationRule crystalGenerationRule = CrystalGenerationRule.Random;
    [SerializeField]
    private static int _crystalPeriod = 5;
    [SerializeField]
    private MapBehaviour _map;//Ссылка на ГО карты.

    private static GamemanagerBehaviour _instance;
    
    private int _scoreCounter = 0;

    #region Public Metods
    public void AddScore(int score)
    {
        _scoreCounter += score;
        MainHUDBehaviour.Instance.ShowScore(_scoreCounter);
    }
    public void GameOver()
    {
        //gameover
        Debug.Log("Game over");
        MainHUDBehaviour.Instance.ShowGameOverPanel();
        PauseGame();
    }
    public void InitGame()
    {
        _map.InitMap();
        MainHUDBehaviour.Instance.ShowStartPanel();
        PauseGame();
    }
    public void StartGame()
    {
        Debug.Log("Game start");
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
        Debug.Log("Restart");
    }
    #endregion

    #region Private Metods
    
    #endregion
    void Start()
    {
        InitGame();
    }

    
}
