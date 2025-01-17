using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ToPlay : MonoBehaviour
{
    // Called when we click the "Play" button.
    public void OnHomeButton()
    {
        SceneManager.LoadScene(0);
    }
    // Called when we click the "Quit" button.
   
}