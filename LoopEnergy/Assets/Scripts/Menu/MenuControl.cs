using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private GameObject levelsMenu;

    LevelManager levelManager;

    private void Start()
    {
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
            Debug.Log("aqui");
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
}
