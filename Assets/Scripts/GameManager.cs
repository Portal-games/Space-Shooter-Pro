using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver = false;

    private void Update(){
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true) 
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            
            if (currentSceneIndex == 1)
            {
                SceneManager.LoadScene(1);
            }
            else if (currentSceneIndex == 2)
            {
                SceneManager.LoadScene(2);
            }
        }

        //if escape key is pressed
        //quit application

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void GameOver(){
        _isGameOver = true;
    }
}
