﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   

    public void LoadGame()
    {

        SceneManager.LoadScene(1);

    }

    public void LoadInstructions()
    {

        SceneManager.LoadScene(2);

    }
    
    public void LoadMainMenu()
    {

        SceneManager.LoadScene(0);
    }
}
