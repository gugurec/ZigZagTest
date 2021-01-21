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
    private int DistanceTileGenerationForward = 10;//на сколько тайлов вперед генерировать дорогу перед игроком.
    [SerializeField]
    private int DistanceTileGenerationBack = 3;//если игрок прошел столько тайлов вперед, то тайлы позади начнут убираться.
    [SerializeField]
    private int StartFieldSize = 3;
    [SerializeField]
    private Difficulty difficulty = Difficulty.Hard;
    [SerializeField]
    private CrystalGenerationRule crystalGenerationRule = CrystalGenerationRule.Random;
    [SerializeField]
    private int CrystalPeriod = 5;
    [SerializeField]
    private GameObject Map;//Ссылка на ГО карты.
    [SerializeField]
    private GameObject Tile;//Тайлик из которого сторим дорогу для героя
    [SerializeField]
    private GameObject Character;//герой которым будем играть

    private static GamemanagerBehaviour _instance;
    private static List<GameObject> _tiles = new List<GameObject>();//массив всех тайлов в игре
    private CharacterBehaviour _character;
    private Vector3Int _currentTileEndPos = Vector3Int.zero;
    private int _tileCounter = 0;//считаем тайлы по порядку от 1 до CrystalPeriod
    private int _crystalSpawnIndex = 0;// индекс тайла на котором заспавнится следующий кристал (от 0 до CrystalPeriod)
    private bool _crystalAlreadySpawn = false;

    #region Public Metods
    public void GameOver()
    {
        //gameover
        Debug.Log("Game over");
        MainHUDBehaviour.Instance.ShowGameOverPanel();
        PauseGame();
    }
    public void InitGame()
    {
        SpawnCharacter();
        CreateStartPlatform();
        for (; _character.GetCurrentMaxTileCoords() + DistanceTileGenerationForward > GetCurrentTileEndCoords();)
            CreateRandomTile();
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
    public void CreateStartPlatform()// стартовое поле StartFieldSize х StartFieldSize CrystalPeriod раз
    {
        for(int i = 0; i < StartFieldSize; i++)
            for(int j = 0; j < StartFieldSize; j++)
                SpawnTile(new Vector2Int(i, j), true);

        _currentTileEndPos.x = StartFieldSize - 1;
        _currentTileEndPos.z = StartFieldSize - 1;
    }
    public void CreateRandomTile()
    {
        int leftOrRight = Random.Range(0, 2);
        int XStartPos = 0;
        int ZStartPos = 0;
        int tileSize = 0;

        switch (difficulty)
        {
            case Difficulty.Easy :
                tileSize = 3;
                break;
            case Difficulty.Medium:
                tileSize = 2;
                break;
            case Difficulty.Hard:
                tileSize = 1;
                break;
            case Difficulty.None:
                tileSize = 1;
                break;
        }
        
        if (leftOrRight == 1)
        {
            XStartPos = _currentTileEndPos.x + 1;
            ZStartPos = _currentTileEndPos.z - (tileSize - 1);
        }
        else
        {
            ZStartPos = _currentTileEndPos.z + 1;
            XStartPos = _currentTileEndPos.x - (tileSize - 1);
        }

        for (int i = XStartPos; i < tileSize + XStartPos; i++)
            for (int j = ZStartPos; j < tileSize + ZStartPos; j++)
                SpawnTile(new Vector2Int(i, j));

        _currentTileEndPos.x = tileSize + XStartPos - 1;
        _currentTileEndPos.z = tileSize + ZStartPos - 1;
    }
    public void SpawnCharacter()
    {
        _character = Instantiate(Character, Map.transform).GetComponent<CharacterBehaviour>();
    }
    public void SpawnTile(Vector2Int pos, bool isStartPlatformTile = false)
    {
        GameObject disabledTile = null;
        foreach(var e in _tiles)
        {
            if (!e.activeInHierarchy)
            {
                disabledTile = e;
                break;
            }
        }

        if(disabledTile != null)
        {
            disabledTile.transform.localPosition = new Vector3(pos.x, 0, pos.y);
            disabledTile.SetActive(true);
            if (isStartPlatformTile)
                disabledTile.GetComponent<TileBehaviour>().SetCrystalActive(false);
            else
                RandomizeSpawnCrystalOnTile(disabledTile.GetComponent<TileBehaviour>());
        }
        else
        {
            var tile = Instantiate(Tile, Map.transform);
            tile.transform.localPosition = new Vector3(pos.x, 0, pos.y);
            if (isStartPlatformTile)
                tile.GetComponent<TileBehaviour>().SetCrystalActive(false);
            else
                RandomizeSpawnCrystalOnTile(tile.GetComponent<TileBehaviour>());
            _tiles.Add(tile);
        }

        DeactivFarTiles();
    }
    #endregion

    #region Private Metods
    private void RandomizeSpawnCrystalOnTile(TileBehaviour tile)
    {
        bool activ = false;
        if (!_crystalAlreadySpawn)
        {
            if (_tileCounter == _crystalSpawnIndex)
            {
                activ = true;
                _crystalAlreadySpawn = true;
                //вычисляем следующий
                switch (crystalGenerationRule)
                {
                    case CrystalGenerationRule.Random: // случайным образом
                        {
                            _crystalSpawnIndex = Random.Range(0, CrystalPeriod + 1);
                            break;
                        }
                    case CrystalGenerationRule.Period: //по порядку.То есть на первом блоке - 1 - ый тайл с кристаллом, на 2 - ом - 2 тайл и так далее до 5 - ого блока.Далее опять с 1 - ого по 5.
                        {
                            _crystalSpawnIndex++;
                            if (_crystalSpawnIndex > CrystalPeriod)
                                _crystalSpawnIndex = 0;
                            break;
                        }
                    case CrystalGenerationRule.None:
                        {
                            break;
                        }
                }
                
            }
        }

        _tileCounter++;
        if (_tileCounter > CrystalPeriod)
        {
            _crystalAlreadySpawn = false;
            _tileCounter = 0;
        }

        tile.SetCrystalActive(activ);
    }
    private void DeactivFarTiles()
    {
        foreach (var e in _tiles)
        {
            if (e.activeInHierarchy)
                if (_character.GetCurrentMaxTileCoords() - GetTileCoords(e) > DistanceTileGenerationBack)
                    e.SetActive(false);
        }
    }
    private int GetCurrentTileEndCoords()
    {
        return Mathf.Max(_currentTileEndPos.x, _currentTileEndPos.z);
    }
    private int GetTileCoords(GameObject tile)
    {
        return Mathf.RoundToInt(Mathf.Max(tile.transform.position.x, tile.transform.position.z));
    }
    #endregion
    void Start()
    {
        InitGame();
    }

    void Update()
    {
        if(_character.GetCurrentMaxTileCoords() + DistanceTileGenerationForward > GetCurrentTileEndCoords())
            CreateRandomTile();
    }
}
