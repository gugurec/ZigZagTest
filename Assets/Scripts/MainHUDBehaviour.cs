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
}
