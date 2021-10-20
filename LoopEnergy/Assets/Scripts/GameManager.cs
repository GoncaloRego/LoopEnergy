using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    struct NodesToFill
    {
        public Vector2 position;
        public bool isFilled;
    };

    [SerializeField] GridController gridController;
    private int numberOfNodesToFill;
    private NodesToFill[,] solutionNodesByLevel;
    private int solutionNodeCounter = 0;

    public LevelManager levelManager;

    AudioSource audioSource;
    public SoundSystem soundSystem;
    bool stopSoundRepeating;

    public SaveSystem saveSystem;
    SaveData saveData = new SaveData();

    void Start()
    {
        // Load Level

        audioSource = GetComponent<AudioSource>();
        solutionNodeCounter = 0;
        stopSoundRepeating = false;

        InitializeAllSolutionNodes();

        soundSystem.PlaySound(levelManager.currentLevel + 1, audioSource);

        //SaveGame();
    }

    void Update()
    {
        // Player Completed Level
        if (levelManager.levelComplete[levelManager.currentLevel] == true)
        {
            if(stopSoundRepeating == false)
            {
                soundSystem.PlaySound(3, audioSource);
                stopSoundRepeating = true;
            }
            gridController.StartCoroutine("PlayWinAnimation");
            solutionNodeCounter = 0;
        }

        // Load New Level Scene
        if (gridController.WinAnimationEnded() == true)
        {
            levelManager.lastLevel = levelManager.currentLevel;

            if (levelManager.currentLevel < levelManager.numberOfLevels - 1)
            {
                StartCoroutine("LoadNewLevel");
            }
        }
    }

    void InitializeAllSolutionNodes()
    {
        if (levelManager.currentLevel == levelManager.levelOne)
        {
            numberOfNodesToFill = 3;
        }
        else if (levelManager.currentLevel == levelManager.levelTwo)
        {
            numberOfNodesToFill = 4;
        }

        solutionNodesByLevel = new NodesToFill[levelManager.numberOfLevels, numberOfNodesToFill];

        // Level 1
        if (levelManager.currentLevel == levelManager.levelOne)
        {
            solutionNodesByLevel[0, 0].position = new Vector2(-1, 0);
            solutionNodesByLevel[0, 1].position = new Vector2(0, 0);
            solutionNodesByLevel[0, 2].position = new Vector2(1, 0);
        }
        else if (levelManager.currentLevel == levelManager.levelTwo)
        {
            solutionNodesByLevel[1, 0].position = new Vector2(-1, 1);
            solutionNodesByLevel[1, 1].position = new Vector2(0, 1);
            solutionNodesByLevel[1, 2].position = new Vector2(1, 1);
            solutionNodesByLevel[1, 3].position = new Vector2(2, 1);
        }

        for(int i = 0; i < levelManager.numberOfLevels; i++)
        {
            for(int j = 0; j < numberOfNodesToFill; j++)
            {
                solutionNodesByLevel[i, j].isFilled = false;
            }
        }
    }

    public bool LevelComplete(int _currentLevel)
    {
        for (int i = 0; i < numberOfNodesToFill; i++)
        {
            foreach (Node n in gridController.nodeList)
            {
                if (_currentLevel == 0)
                {
                    if ((Vector2)n.cachedTransform.position == (Vector2)solutionNodesByLevel[_currentLevel, i].position && solutionNodesByLevel[_currentLevel, i].isFilled == false && n.nodeID == NodeType.lineNodeID)
                    {
                        solutionNodesByLevel[_currentLevel, i].isFilled = true;
                        solutionNodeCounter++;
                    }
                }
                else if (_currentLevel == 1)
                {
                    if (n.nodeID == NodeType.lineNodeID)
                    {
                        if (i < numberOfNodesToFill - 1)
                        {
                            if ((Vector2)n.cachedTransform.position == solutionNodesByLevel[_currentLevel, i].position && solutionNodesByLevel[_currentLevel, i].isFilled == false)
                            {
                                solutionNodesByLevel[_currentLevel, i].isFilled = true;
                                solutionNodeCounter++;
                            }
                        }
                    }
                    else if (n.nodeID == NodeType.curvedLineNodeID)
                    {
                        if ((Vector2)n.cachedTransform.position == solutionNodesByLevel[_currentLevel, numberOfNodesToFill - 1].position && solutionNodesByLevel[_currentLevel, numberOfNodesToFill - 1].isFilled == false)
                        {
                            solutionNodesByLevel[_currentLevel, numberOfNodesToFill - 1].isFilled = true;
                            solutionNodeCounter++;
                        }
                    }
                }
            }

            if (solutionNodeCounter == numberOfNodesToFill)
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator LoadNewLevel()
    {
        yield return new WaitForSeconds(0.5f);

        // Save Level
        levelManager.IncrementLevel();

        int level = levelManager.currentLevel + 1;
        SceneManager.LoadScene("Level" + level);
    }

    void SaveGame()
    {
        saveData.currentLevel = levelManager.currentLevel;
        saveSystem.SaveGame();
    }
}
