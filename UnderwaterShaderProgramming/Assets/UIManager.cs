using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject tutorial;

    public static bool paused = true;

    private void Start()
    {
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        tutorial.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        paused = true;
    }

    public void Play()
    {
        Debug.Log("Playing");
        PostProcessAnimations.StartFade();
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        tutorial.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        paused = false;
    }

    public void Pause()
    {
        Debug.Log("Pausing");
        PostProcessAnimations.StartReverseFade();
        pauseMenu.SetActive(true);
        tutorial.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        paused = true;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
