using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    struct NodesToFill
    {
        public int levelID;
        public Vector2 position;
        public bool isFilled; 
    };

    [SerializeField] GridController gridController;
    private NodesToFill[] nodesToFill;
    private int numberOfNodesToFill;
    private Vector2[,] solutionNodesByLevel;
    private int solutionNodeCounter = 0;
    public GameSaveManager gameSaveManager;

    public LevelManager levelManager;

    void Start()
    {
        solutionNodeCounter = 0;
        InitializeAllSolutionNodes();
        SetLevelSolutionNodes(levelManager.currentLevel);
    }

    void Update()
    {
        // Player Completed Level
        if (levelManager.levelComplete[levelManager.currentLevel] == true)
        {
            CancelInvoke();
            gridController.StartCoroutine("PlayWinAnimation");
            solutionNodeCounter = 0;
        }

        // Load New Level Scene
        if (gridController.WinAnimationEnded() == true)
        {
            levelManager.lastLevel = levelManager.currentLevel;
            StartCoroutine("LoadNewLevel");
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

        solutionNodesByLevel = new Vector2[levelManager.numberOfLevels, numberOfNodesToFill];

        // Level 1
        if (levelManager.currentLevel == levelManager.levelOne)
        {
            solutionNodesByLevel[0, 0] = new Vector2(-1, 0);
            solutionNodesByLevel[0, 1] = new Vector2(0, 0);
            solutionNodesByLevel[0, 2] = new Vector2(1, 0);
        }
        else if (levelManager.currentLevel == levelManager.levelTwo)
        {
            solutionNodesByLevel[1, 0] = new Vector2(-1, 1);
            solutionNodesByLevel[1, 1] = new Vector2(0, 1);
            solutionNodesByLevel[1, 2] = new Vector2(1, 1);
            solutionNodesByLevel[1, 3] = new Vector2(2, 1);
        }
    }

    void SetLevelSolutionNodes(int level)
    {
        nodesToFill = new NodesToFill[numberOfNodesToFill];

        for (int levelIdentifier = 0; levelIdentifier < levelManager.numberOfLevels; levelIdentifier++)
        {
            for (int i = 0; i < numberOfNodesToFill; i++)
            {
                NodesToFill solutionNode = new NodesToFill();
                solutionNode.levelID = levelIdentifier;

                if (levelIdentifier == levelManager.levelOne)
                {
                    solutionNode.position = solutionNodesByLevel[level, i];
                }

                nodesToFill[i] = solutionNode;
                nodesToFill[i].isFilled = false;
            }
        }
    }

    public bool LevelComplete(int _currentLevel)
    {
        for (int i = 0; i < numberOfNodesToFill; i++)
        {
            foreach (Node n in gridController.nodeList)
            {
                if ((Vector2)n.transform.position == (Vector2)solutionNodesByLevel[_currentLevel, i] && nodesToFill[i].isFilled == false && n.nodeWasPickedUp == false)
                {
                    nodesToFill[i].isFilled = true;
                    solutionNodeCounter++;
                    if (solutionNodeCounter == numberOfNodesToFill)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    IEnumerator LoadNewLevel()
    {
        yield return new WaitForSeconds(0.5f);
        levelManager.IncrementLevel();
        int level = levelManager.currentLevel + 1;
        if (level <= levelManager.numberOfLevels)
        {
            SceneManager.LoadScene("Level" + level);
        }
    }
}
