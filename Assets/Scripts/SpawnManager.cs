using System.Collections;
using UnityEngine;
public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;


    private bool _stopSpawing = false;

    public void StartSpawning() {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutien());
    }

    // Coroutine to spawn game objects every 5 seconds
    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawing == false)
        {
            Vector3 EnemyPosToSpawn = new(Random.Range(-8f, 8f), 7, 0);

            GameObject newEnemy = Instantiate(_enemy,EnemyPosToSpawn, Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(3.0f);
        }

    }

    IEnumerator SpawnPowerupRoutien()
    {
        //every 3-7 seconds spawn a powerup
        while(_stopSpawing == false)
        {
            Vector3 PowerupPosToSpawn = new(Random.Range(-8f, 8f), 7, 0);
            Instantiate(powerups[Random.Range(0,3)], PowerupPosToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(5, 8));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawing = true;
    } 
    

}
 