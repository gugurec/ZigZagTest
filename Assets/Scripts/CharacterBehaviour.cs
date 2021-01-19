using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    private const float Speed = 2f;

    private Vector3 _v;
    private bool _isStartMove = true;
    private Transform m_transform;

    void Start()
    {
        _v = Vector3.zero;
        m_transform = GetComponent<Transform>();
        if (m_transform == null)
            Debug.LogError("Transform not found GameObject: " + GetComponent<GameObject>().name);
    }

    void Update()
    {
        if (_isStartMove)
        {
            _v = m_transform.position;
            if (Input.GetMouseButton(0))
                _v.z = _v.z + Time.deltaTime * Speed;
            else
                _v.x = _v.x + Time.deltaTime * Speed;
            m_transform.position = _v;
        }
    }
}
