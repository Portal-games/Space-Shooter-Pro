
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab; 
     [SerializeField] private GameObject _sheildOnPlayer;


    [SerializeField]
    private float _firerate = 0.5f;
    private float _canfire = -1f;

    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private UiManager _uiManager;
    
    private bool _trippleShotActive = false;
#pragma warning disable IDE0052 // Remove unread private members
    private bool _speedBoostActive = false;
#pragma warning restore IDE0052 // Remove unread private members
    private bool _sheildsActive = false;


    [SerializeField]
    private GameObject _trippleShotPrefab;
    private int _speedMultiplier = 2;
    [SerializeField] private int _score ;

    // Start is called before the first frame update
    void Start()
    { 
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL");
        }

        if (_uiManager == null){
            Debug.LogError("UiManager is NULL");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canfire)
        {
            FireLaser(); 
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput,0);

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
        _canfire = Time.time + _firerate;

        if (_trippleShotActive == true )
        {
            
            Instantiate(_trippleShotPrefab, transform.position + new Vector3(-0.53f, -0f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
    }

    public void Damage()
    {
        //if sheild is active
        //do nothing
        //deactivate sheilds
        if (_sheildsActive == true)
        {
            _sheildsActive = false;
            //disable sheild on player
            _sheildOnPlayer.SetActive (false);
            return;
        }
        
        
           _lives --;
        
        _uiManager.UpdateLives(_lives);
        
        

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.GameOverText();
            Destroy(this.gameObject);
        }
    }

    public void TrippleShotActive()
    {
        _trippleShotActive = true;
        StartCoroutine(TrippleShotPowerDownRoutien());
    
    }



    IEnumerator TrippleShotPowerDownRoutien()
    {
        yield return new WaitForSeconds(5f);
        _trippleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRuotien());
    }

    IEnumerator SpeedBoostPowerDownRuotien()
    {
        yield return new WaitForSeconds(5f);
        _speedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void SheildActive(){
      _sheildsActive = true;
      _sheildOnPlayer.SetActive(true);
      //enable the shield on player
    } 
    //method to add 10 to the score
    public void AddScore(int points){
        _score += 10;
        _uiManager.UpdateScore(_score);
    }
    //comuiciate with the uimanager to add 10 to the score
} 
