using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private int currentSceneIndex;
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab; 
    [SerializeField] private GameObject _shieldOnPlayer;

    [SerializeField]
    private float _firerate = 0.5f;
    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private UiManager _uiManager;
    
    private bool _tripleShotActive = false;
    [SerializeField]
    private bool _speedBoostActive = false;
    private bool _shieldsActive = false;

    [SerializeField]
    private GameObject _tripleShotPrefab;
    private int _speedMultiplier = 2;
    [SerializeField] private int _score;
    //player hurt
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private GameObject _leftEngine;
    //var to store audio clip
    [SerializeField] private AudioClip _laserSound;

    [SerializeField] private int PlayerId; // 0 for Player 1, 1 for Player 2
    private AudioSource _audioSource;

    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 1)
        {
            transform.position = new Vector3(0, 0, 0);
        }
        else if (currentSceneIndex == 2)
        {
            switch (PlayerId)
            {
                case 0: transform.position = new Vector3(-5, 0, 0); break;
                case 1: transform.position = new Vector3(5, 0, 0); break;
            }
        }

        _spawnManager = GameObject.Find("Spawn_Manager")?.GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas")?.GetComponent<UiManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            UnityEngine.Debug.LogError("SpawnManager is NULL");
        }

        if (_uiManager == null)
        {
            UnityEngine.Debug.LogError("UiManager is NULL");
        }

        if (_audioSource == null)
        {
            UnityEngine.Debug.LogError("AudioSource on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }
    }

    void Update()
    {
        if (PlayerId == 0)
        {
            CalculateMovement();  // Player 1 movement using WASD
        }
        else if (PlayerId == 1)
        {
            CalculateCoOpMovement();  // Player 2 movement using IJKL
        }

        // Allow Player 1 to fire laser using Space
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && PlayerId == 0)
        {
            FireLaser(); 
        }

        // Allow Player 2 to fire laser using Right Shift
        if (Input.GetKeyDown(KeyCode.RightShift) && Time.time > _canFire && PlayerId == 1)
        {
            FireLaser();
        }
    }

    // Player 1 movement (WASD)
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(_speed * Time.deltaTime * direction);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    // Player 2 movement (IJKL)
    void CalculateCoOpMovement()
    {
        float horizontalInput = 0;
        float verticalInput = 0;

        // Use different keys for Player 2
        if (Input.GetKey(KeyCode.J)) horizontalInput = -1;
        if (Input.GetKey(KeyCode.L)) horizontalInput = 1;
        if (Input.GetKey(KeyCode.I)) verticalInput = 1;
        if (Input.GetKey(KeyCode.K)) verticalInput = -1;

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(_speed * Time.deltaTime * direction);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _firerate;

        if (_tripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(-0.53f, 0, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        _audioSource.Play();
    }

    public void Damage()
    {
        if (_shieldsActive)
        {
            _shieldsActive = false;
            _shieldOnPlayer.SetActive(false);
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.GameOverText();
            Destroy(gameObject);
        }
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _tripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _speedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _shieldsActive = true;
        _shieldOnPlayer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
