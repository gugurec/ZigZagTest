using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    private const float Speed = 2f;

    private Vector3 _v = Vector3.zero;
    private bool _isStartMove = true;

    public int GetCurrentMaxTileCoords()
    {
        if (transform.position.z > transform.position.x)
            return Mathf.RoundToInt(transform.position.z);
        else
            return Mathf.RoundToInt(transform.position.x);
    }

    void Update()
    {
        if (_isStartMove)
        {
            _v = transform.position;
            if (Input.GetMouseButton(0))
                _v.z = _v.z + Time.deltaTime * Speed;
            else
                _v.x = _v.x + Time.deltaTime * Speed;
            transform.position = _v;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var tile = other.gameObject.GetComponent<TileBehaviour>();
        if (tile.isMyCrystalCollider(other))
            tile.PickUpCrystal();
    }
    private void OnTriggerExit(Collider other)
    {
        var tile = other.gameObject.GetComponent<TileBehaviour>();
        if (!tile.isMyCrystalCollider(other))//вышли из коллайдера тайла, нужно проверить если ли под нами другой коллайдер
        {
            RaycastHit info;
            if(!Physics.Raycast(new Ray(transform.position, Vector3.down), out info, 100))//коллайдера нет, шарик упал
                GamemanagerBehaviour.Instance.GameOver();
        }
    }
}
