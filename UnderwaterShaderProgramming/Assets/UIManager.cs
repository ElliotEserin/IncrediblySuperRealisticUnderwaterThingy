using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject tutorial;
    public GameObject pickups;

    public Slider volume;
    public AudioMixer mixer;

    public static bool paused = true;

    private void Start()
    {
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        tutorial.SetActive(false);
        pickups.SetActive(false);

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
        pickups.SetActive(true);

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
        pickups.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        paused = true;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Update()
    {
        mixer.SetFloat("Vol", volume.value);
    }
}
