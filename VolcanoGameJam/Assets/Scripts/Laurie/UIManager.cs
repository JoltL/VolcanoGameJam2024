using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _panel;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }
    public void PauseGame()
    {


        if (Time.timeScale > 0f)
        {
            Time.timeScale = 0f;
            _panel.SetActive(true);
           
        }
        else
        {
            Time.timeScale = 1f;
            _panel.SetActive(false);

        }

    }

    public void ChangeScene(int scene)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

}
