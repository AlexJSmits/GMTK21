using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuButtons : MonoBehaviour
{

    IsometricCharacterController charController;

    // Start is called before the first frame update
    void Start()
    {
        charController = FindObjectOfType<IsometricCharacterController>();
    }

    public void QuitButton()
    {
        Application.Quit();
    }


}
