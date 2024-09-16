using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private float lowerYBound = -5f;
    [SerializeField]
    private float respawnHeight = 8f;
    [SerializeField]
    private float respawnXRange = 9.0f;

    private Player _player;
    private Animator _anim;
    private float _animSeconds = 2.8f;
    private AudioSource _audioSource;
    private bool _isDestroyed = false; // New flag to track if enemy is destroyed

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = gameObject.GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("PLAYER IS NULL, Looks like we need to fix it...");
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator component is missing on the enemy object.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource is missing on the enemy object.");
        }
    }

    void Update()
    {
        if (!_isDestroyed) // Only move the enemy if it's not destroyed
        {
            transform.Translate(_speed * Time.deltaTime * Vector3.down);

            if (transform.position.y < lowerYBound)
            {
                transform.position = new Vector3(Random.Range(-respawnXRange, respawnXRange), respawnHeight, 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isDestroyed) return; // Prevent further interactions after the enemy is destroyed

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            HandleDestruction();
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10); // Only add score if not already destroyed
            }
            HandleDestruction();
        }
    }

    // Helper method to handle enemy destruction
    private void HandleDestruction()
    {
        _isDestroyed = true; // Set the destroyed flag
        _anim.SetTrigger("OnEnemyDeath"); // Play death animation
        _speed = 0; // Stop enemy movement
        _audioSource.Play(); // Play destruction sound
        Destroy(this.gameObject, _animSeconds); // Destroy the enemy after animation ends
    }
}
