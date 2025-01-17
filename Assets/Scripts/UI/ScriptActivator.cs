using System.Collections;
using UnityEngine;

public class ScriptActivator : MonoBehaviour
{
    public GameManager gameManager; 
    public MonoBehaviour[] scripts; 
    public GameObject[] UIs;
    public GameObject except;
    public GameObject except2;



    private void Start()
    {
        except.SetActive(true);
        except2.SetActive(true);



        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
        foreach (GameObject UI in UIs)
        {
            UI.SetActive( false);
        }

        StartCoroutine(WaitForMazeReady());
    }

    private IEnumerator WaitForMazeReady()
    {
        
        while (!gameManager.MazeFinsihed)
        {
            yield return null;
        }

        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = true;
        }
        foreach (GameObject UI in UIs)
        {
            UI.SetActive(true);
        }
        except.SetActive(false);
        except2.SetActive(false);


        Debug.Log("Scripts activated after maze generation!");
    }
}
