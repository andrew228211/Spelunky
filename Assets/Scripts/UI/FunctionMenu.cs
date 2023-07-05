using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FunctionMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        PlayerPrefs.SetInt("level", 0);
        PlayerPrefs.SetInt("sum", 0);
        SceneManager.LoadScene(1);
    }
    public void Setting()
    {

    }
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    public void Pause()
    {
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        Time.timeScale = 1f;
    }
}
