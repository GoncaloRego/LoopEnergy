using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private GameObject levelsMenu;

    public LevelManager levelManager;
    AudioSource audioSource;
    public SoundSystem soundSystem;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        levelsMenu.SetActive(false);
    }

    public void ShowLevelsMenu()
    {
        levelsMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void LoadLevel(int levelID)
    {
        if (levelID > 1)
        {
            if (levelManager.LevelBeforeIsComplete() == true)
            {
                SceneManager.LoadScene("Level" + levelID);
            }
        }
        else
        {
            SceneManager.LoadScene("Level" + levelID);
        }
    }

    public void PlaySound(int soundID)
    {
        soundSystem.PlaySound(soundID, audioSource);
    }
}
