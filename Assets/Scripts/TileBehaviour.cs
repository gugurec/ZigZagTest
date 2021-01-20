using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject _crystal;
    [SerializeField]
    private Collider _crystalCollider;
    [SerializeField]
    private Collider _tileCollider;
    public void SetCrystalActive(bool activ)
    {
        _crystal.SetActive(activ);
        _crystalCollider.enabled = activ;
    }
    public void PickUpCrystal()
    {
        SetCrystalActive(false);
    }
    public bool isMyCrystalCollider(Collider collider)
    {
        return _crystalCollider.Equals(collider);
    }
}
