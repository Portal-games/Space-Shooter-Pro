
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private bool _isEnemyLaser = false;

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (_isEnemyLaser)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime); // Move downward for enemy
        }
        else
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime); // Move upward for player
        }
    }

    public void AssignEnemy()
    {
        _isEnemyLaser = true;
    }
}