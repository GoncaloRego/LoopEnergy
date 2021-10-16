using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private GameObject levelsMenu;
    [SerializeField] private GameObject levelsManagerGO;
    LevelManager levelManager;

    private void Start()
    {
        levelsMenu.SetActive(false);
        levelManager = levelsManagerGO.GetComponent<LevelManager>();
    }

    public void ShowLevelsMenu()
    {
        levelsMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void LoadLevel(int levelID)
    {
        if(levelManager.LevelBeforeIsComplete(levelID) == true)
        {
            SceneManager.LoadScene("Level" + levelID);
        }
    }
}
