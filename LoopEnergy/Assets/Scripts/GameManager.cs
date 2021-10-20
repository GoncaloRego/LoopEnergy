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
        LoadGame();

        audioSource = GetComponent<AudioSource>();
        solutionNodeCounter = 0;
        stopSoundRepeating = false;

        // Each level there are nodes that have to be filled, this function initializes them
        InitializeAllSolutionNodes();

        soundSystem.PlaySound(levelManager.currentLevel + 1, audioSource);
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
                if (_currentLevel == 0) // level 1
                {
                    //check if all solution nodes are occupied by line nodes
                    if ((Vector2)n.cachedTransform.position == (Vector2)solutionNodesByLevel[_currentLevel, i].position && solutionNodesByLevel[_currentLevel, i].isFilled == false && n.nodeID == NodeType.lineNodeID)
                    {
                        solutionNodesByLevel[_currentLevel, i].isFilled = true;
                        solutionNodeCounter++;
                    }
                }
                else if (_currentLevel == 1) // level 2
                {
                    if (n.nodeID == NodeType.lineNodeID)
                    {
                        //check if all solution nodes are occupied by line nodes except the last one which has to be occupied by a curved line node
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
                        //check if last solution nodes is occupied by a curved line node
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

        // Increment and Save Level ID
        levelManager.IncrementLevel();
        SaveGame();

        int level = levelManager.currentLevel + 1;
        SceneManager.LoadScene("Level" + level);
    }

    void SaveGame()
    {
        saveData.currentLevel = levelManager.currentLevel;
        saveSystem.SaveGame();
    }

    void LoadGame()
    {
        saveSystem.LoadGame();
    }
}
