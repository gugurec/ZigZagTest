using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject Tile;//Тайлик из которого сторим дорогу для героя
    [SerializeField]
    private GameObject Character;//герой которым будем играть
    [SerializeField]
    private int DistanceTileGenerationForward = 10;//на сколько тайлов вперед генерировать дорогу перед игроком.
    [SerializeField]
    private int DistanceTileGenerationBack = 3;//если игрок прошел столько тайлов вперед, то тайлы позади начнут убираться.
    [SerializeField]
    private int _startFieldSize = 3;

    private static List<GameObject> _tiles = new List<GameObject>();//массив всех тайлов в игре
    private CharacterBehaviour _character;//ссылка на текущего персонажа
    private Vector3Int _currentTileEndPos = Vector3Int.zero;//координата на которой закончился текущий спавн тайлов
    private int _tileCounter = 0;//считаем тайлы по порядку от 1 до CrystalPeriod
    private int _crystalSpawnIndex = 0;// индекс тайла на котором заспавнится следующий кристал (от 0 до CrystalPeriod)
    private bool _crystalAlreadySpawn = false;


    public void InitMap()
    {
        SpawnCharacter();
        CreateStartPlatform(_startFieldSize);
        for (; _character.GetCurrentMaxTileCoords() + DistanceTileGenerationForward > GetCurrentTileEndCoords();)
            CreateRandomTile();
    }
    public void CreateStartPlatform(int startFieldSize)// генерим стартовое поле StartFieldSize х StartFieldSize
    {
        for (int i = 0; i < startFieldSize; i++)
            for (int j = 0; j < startFieldSize; j++)
                SpawnTile(new Vector2Int(i, j), true);

        _currentTileEndPos.x = startFieldSize - 1;
        _currentTileEndPos.z = startFieldSize - 1;
    }
    public void CreateRandomTile()// генерим дорогу вправо или влево (ширина зависит от difficulty)
    {
        int leftOrRight = Random.Range(0, 2);
        int XStartPos = 0;
        int ZStartPos = 0;
        int tileSize = 0;

        switch (GamemanagerBehaviour.GameDifficulty)
        {
            case GamemanagerBehaviour.Difficulty.Easy:
                tileSize = 3;
                break;
            case GamemanagerBehaviour.Difficulty.Medium:
                tileSize = 2;
                break;
            case GamemanagerBehaviour.Difficulty.Hard:
                tileSize = 1;
                break;
            case GamemanagerBehaviour.Difficulty.None:
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
    public void SpawnCharacter()//спавним персонажа
    {
        _character = Instantiate(Character, transform).GetComponent<CharacterBehaviour>();
    }
    public void SpawnTile(Vector2Int pos, bool isStartPlatformTile = false)//спавним тайл в указанных кординатах (если isStartPlatformTile, то не спавним на нем кристал)
    {
        GameObject disabledTile = null;
        foreach (var e in _tiles)
        {
            if (!e.activeInHierarchy)
            {
                disabledTile = e;
                break;
            }
        }

        if (disabledTile != null)
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
            var tile = Instantiate(Tile, transform);
            tile.transform.localPosition = new Vector3(pos.x, 0, pos.y);
            if (isStartPlatformTile)
                tile.GetComponent<TileBehaviour>().SetCrystalActive(false);
            else
                RandomizeSpawnCrystalOnTile(tile.GetComponent<TileBehaviour>());
            _tiles.Add(tile);
        }

        DeactivFarTiles();
    }

    private void RandomizeSpawnCrystalOnTile(TileBehaviour tile)//спавним кристал на указанном тайле при необходимости
    {
        bool activ = false;
        if (!_crystalAlreadySpawn)
        {
            if (_tileCounter == _crystalSpawnIndex)
            {
                activ = true;
                _crystalAlreadySpawn = true;
                //вычисляем следующий
                switch (GamemanagerBehaviour.Rule)
                {
                    case GamemanagerBehaviour.CrystalGenerationRule.Random: // случайным образом
                        {
                            _crystalSpawnIndex = Random.Range(0, GamemanagerBehaviour.Period + 1);
                            break;
                        }
                    case GamemanagerBehaviour.CrystalGenerationRule.Period: //по порядку.То есть на первом блоке - 1 - ый тайл с кристаллом, на 2 - ом - 2 тайл и так далее до 5 - ого блока.Далее опять с 1 - ого по 5.
                        {
                            _crystalSpawnIndex++;
                            if (_crystalSpawnIndex > GamemanagerBehaviour.Period)
                                _crystalSpawnIndex = 0;
                            break;
                        }
                    case GamemanagerBehaviour.CrystalGenerationRule.None:
                        {
                            break;
                        }
                }

            }
        }

        _tileCounter++;
        if (_tileCounter > GamemanagerBehaviour.Period)
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

    void Update()
    {
        if (_character.GetCurrentMaxTileCoords() + DistanceTileGenerationForward > GetCurrentTileEndCoords())
            CreateRandomTile();
    }
}
