
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
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp() 
    {
        // Move the laser upwards every frame
        transform.Translate(_speed * Time.deltaTime * Vector3.up);

        // If the laser moves off-screen, destroy it
        if (transform.position.y > 8)
        {
            // Check if the laser has a parent (e.g., part of a triple shot)
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject); // Destroy the parent object
            }

            Destroy(gameObject); // Destroy the laser itself
        }
    }


    void MoveDown() 
    {
        // Move the laser upwards every frame
        transform.Translate(_speed * Time.deltaTime * Vector3.down);

        // If the laser moves off-screen, destroy it
        if (transform.position.y < -8)
        {
            // Check if the laser has a parent (e.g., part of a triple shot)
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject); // Destroy the parent object
            }

            Destroy(gameObject); // Destroy the laser itself
        }
    }
    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player" && _isEnemyLaser == true) 
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            else 
            {
                Debug.LogError("The player is NULL");
            }
        }
    }
}