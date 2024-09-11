using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
public class Asteroid_behavior : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3.0f;
    [SerializeField ]private GameObject _explosionPrefab;


    void Update()
    {
        transform.Rotate(_rotateSpeed * Time.deltaTime * Vector3.forward);
    }
    //check for laser colision (Trigger)
    //instiate explosion at position of asteroid (us)
    //destroy the explosion after 3 seconds
    
    private void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Laser") {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.25f);
        }
    }
}
