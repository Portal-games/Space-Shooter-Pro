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
            SceneManager.LoadScene(1);
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
