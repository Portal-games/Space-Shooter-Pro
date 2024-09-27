
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private bool _isDestroyed = false; // Flag to track if the enemy is destroyed

    [SerializeField]
    private GameObject _laserPrefab;
    private float _firerate = 3f;
    private float _canfire = -1f;

    void Start()
    {
        
        // Attempt to find and initialize the Player component
   /* _player = GameObject.Find("Player")?.GetComponent<Player>();
    if (_player == null)
    {
        Debug.LogError("Player GameObject is not found or Player component is missing.");
    }*/

    // Attempt to get the AudioSource component
    _audioSource = GetComponent<AudioSource>();
    if (_audioSource == null)
    {
        Debug.LogError("AudioSource component is missing on the Enemy.");
    }

    // Attempt to get the Animator component
    _anim = GetComponent<Animator>();
    if (_anim == null)
    {
        Debug.LogError("Animator component is missing on the Enemy.");
    }

        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("Player component is missing.");
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator component is missing.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource component is missing.");
        }
    }

    void Update()
    {
        // Only move and fire lasers if the enemy is not destroyed
        if (!_isDestroyed)
        {
            CalculateMovement();

            if (Time.time > _canfire)
            {
                _firerate = Random.Range(3f, 7f);
                _canfire = Time.time + _firerate;

                GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity); // No rotation
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

                // Assign the laser to be an enemy laser
                foreach (Laser laser in lasers)
                {
                    laser.AssignEnemyLaser();
                }
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);

        if (transform.position.y < lowerYBound)
        {
            // Respawn at random X within bounds and at a fixed height
            transform.position = new Vector3(Random.Range(-respawnXRange, respawnXRange), respawnHeight, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isDestroyed) return; // Prevent further interactions if already destroyed

        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            HandleDestruction();
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }
            HandleDestruction();
        }
    }

    // Handle destruction of the enemy
    private void HandleDestruction()
    {
        _isDestroyed = true; // Mark as destroyed
        _anim.SetTrigger("OnEnemyDeath"); // Play destruction animation
        _speed = 0; // Stop movement
        _audioSource.Play(); // Play destruction sound
        Destroy(gameObject, _animSeconds); // Destroy object after animation plays
    }
}

