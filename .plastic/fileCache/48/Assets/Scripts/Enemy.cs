using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4;
    private Player _player;
    private Animator _anim;
    private float _animSeconds = 2.8f;
    //handle to animator componot


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>(); 

        if (_player == null){
            //null check the player
            Debug.LogError("PLAYER IS NULL, Look Likes us devs have to fix it... 💀");
        }
        _anim = GetComponent<Animator>();   

        if (_anim == null){ 
            Debug.LogError("Anim is null");
        }
        //assign the component to anim
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(_speed * Time.deltaTime * Vector3.down);

        if (transform.position.y < -5f)
        {
            transform.position = new Vector3(Random.Range(-9.0f,9.0f), 8, 0);
        }  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            //trigger anim
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(this.gameObject, _animSeconds);
        }

       
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if(_player != null){
                _player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(this.gameObject, _animSeconds);
        }


    }


}
