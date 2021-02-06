using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePool
{
    private GameObject _tile;//Тайлик из которого сторим дорогу для героя
    private Transform _rootTransform;
    private List<GameObject> _tiles = new List<GameObject>();

    public List<GameObject> GetActivTiles()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (var e in _tiles)
        {
            if (e.activeInHierarchy)
                list.Add(e);
        }
        return list;
    }
    public void SetRootTransform(Transform t)
    {
        _rootTransform = t;
    }

    public void SetTile(GameObject tile)
    {
        _tile = tile;
    }
    public GameObject Take()
    {
        foreach (var e in _tiles)
        {
            if (!e.activeInHierarchy)
                return e;
        }
        //если нет в пуле
        var tile = Object.Instantiate<GameObject>(_tile, _rootTransform);
        _tiles.Add(tile);
        return tile;
    }
    public void Deactiv(GameObject item)
    {
        item.SetActive(false);
    }

    public void Clear()
    {
        foreach (var e in _tiles)
            Object.Destroy(e);
        _tiles.Clear();
    }
}
