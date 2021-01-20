using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject _crystal;
    [SerializeField]
    private BoxCollider _collider;
    
    public BoxCollider GetCollider()
    {
        return _collider;
    }
    public void SetCrystalActive(bool activ)
    {
        _crystal.SetActive(activ);
    }
}
