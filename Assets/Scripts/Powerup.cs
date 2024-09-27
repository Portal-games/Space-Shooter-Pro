
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    //ID for powerups
    //0 = trippleshot
    //1 = speed
    //2 = shield
    [SerializeField]
    private int powerupID;
   [SerializeField]private AudioClip _clip;
    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);

        if (transform.position.y < -6)
        {
            Destroy(this.gameObject);
        }
    }

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip,transform.position);
            

            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();                      
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    default:
                        Debug.Log("Defualt Value");
                        break;
                }
               

            }

            Destroy(this.gameObject);
        }

    }
}