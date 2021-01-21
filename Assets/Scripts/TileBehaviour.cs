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
    [SerializeField]
    private Animation _platformDestroyAnim;
    [SerializeField]
    private GameObject _platform;

    private Vector3 _scaleBuf = Vector3.zero;

    private void Start()
    {
        _scaleBuf = _platform.transform.localScale;
    }
    public void SetCrystalActive(bool activ)
    {
        _crystal.SetActive(activ);
        _crystalCollider.enabled = activ;
    }
    public void PickUpCrystal()
    {
        SetCrystalActive(false);
        GamemanagerBehaviour.Instance.AddScore(1);
    }
    public bool isMyCrystalCollider(Collider collider)
    {
        return _crystalCollider.Equals(collider);
    }

    public void StartDeactivTile()
    {
        _platformDestroyAnim.Play();
    }

    public void OnPlatformDestroyAnimationEnd()
    {
        _platform.transform.localScale = _scaleBuf;
        this.gameObject.SetActive(false);
    }
}
