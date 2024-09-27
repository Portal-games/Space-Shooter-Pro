using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
  public void LoadSinglePlayerGame()
  {
    SceneManager.LoadScene(1);
  }
  public void LoadCoOpGame()
  {
    SceneManager.LoadScene(2);
  }
}
