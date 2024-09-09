using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame(){
        SceneManager.LoadScene(1); //game scene
    }
}
