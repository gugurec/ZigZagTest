using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHUDBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject _startPanel;
    [SerializeField]
    private GameObject _gameOverPanel;
    [SerializeField]
    private GameObject _gamePanel;
    [SerializeField]
    private UnityEngine.UI.Text _scoreText;
    private string _scoreDescription = "Score: ";
    public static MainHUDBehaviour Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<MainHUDBehaviour>();
            return _instance;
        }
    }

    private static MainHUDBehaviour _instance;

    public void ShowStartPanel()
    {
        _startPanel.SetActive(true);
        _gameOverPanel.SetActive(false);
        _gamePanel.SetActive(false);
    }
    public void ShowGameOverPanel()
    {
        _startPanel.SetActive(false);
        _gameOverPanel.SetActive(true);
        _gamePanel.SetActive(false);
    }
    public void ShowGamePanel()
    {
        _startPanel.SetActive(false);
        _gameOverPanel.SetActive(false);
        _gamePanel.SetActive(true);
    }

    public void UpdateScore()
    {
        _scoreText.text = _scoreDescription + GamemanagerBehaviour.Instance.GetCurrentScore().ToString();
    }

    private void OnEnable()
    {
        GamemanagerBehaviour.Instance.OnScoreChanged += UpdateScore;
    }
    private void OnDisable()
    {
        if(GamemanagerBehaviour.Instance)
            GamemanagerBehaviour.Instance.OnScoreChanged -= UpdateScore;
    }
}
