using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScripts : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject Camera;
    [SerializeField] public GameObject Player;
    [SerializeField] public Scene CurrentScene;


    public void ResumeButton(){
        Player.GetComponent<PlayerController>().pauseGame();
    }

    public void RestartButton(){
        Time.timeScale = 1;
        CurrentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(CurrentScene.name);
    }
}
