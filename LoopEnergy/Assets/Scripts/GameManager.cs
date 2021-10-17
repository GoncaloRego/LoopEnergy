using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    struct NodesToFill
    {
        public int levelID;
        public Vector2 position;
        public bool isFilled; 
    };

    [SerializeField] GameObject gridManagerGO;
    private GridController gridController;
    private bool levelIsComplete;
    private int currentLevel;
    private NodesToFill[] nodesToFill;
    private int numberOfLevels = 1;
    private int levelOneID = 1;
    private int numberOfNodesToFill;
    private Vector2[,] solutionNodesByLevel;
    private int solutionNodeCounter = 0;

    void Start()
    {
        gridController = gridManagerGO.GetComponent<GridController>();
        currentLevel = levelOneID;
        InitializeAllSolutionNodes();
        SetLevelSolutionNodes(currentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if(levelIsComplete == false)
        {
            levelIsComplete = LevelComplete(currentLevel);
        }

        if(levelIsComplete == true)
        {
            gridController.StartCoroutine("PlayWinAnimation");
            gridController.PlayWinAnimation();
            solutionNodeCounter = 0;
        }
    }

    void InitializeAllSolutionNodes()
    {
        if (currentLevel == levelOneID)
        {
            numberOfNodesToFill = 3;
        }

        solutionNodesByLevel = new Vector2[numberOfLevels, numberOfNodesToFill];

        // Level 1
        solutionNodesByLevel[0, 0] = new Vector2(-1, 0);
        solutionNodesByLevel[0, 1] = new Vector2(0, 0);
        solutionNodesByLevel[0, 2] = new Vector2(1, 0);
    }

    void SetLevelSolutionNodes(int level)
    {
        nodesToFill = new NodesToFill[numberOfNodesToFill];

        for (int levelIdentifier = 0; levelIdentifier < numberOfLevels; levelIdentifier++)
        {
            for (int i = 0; i < numberOfNodesToFill; i++)
            {
                NodesToFill solutionNode = new NodesToFill();
                solutionNode.levelID = levelIdentifier;

                if (levelIdentifier == levelOneID)
                {
                    solutionNode.position = solutionNodesByLevel[level - 1, i];
                }

                nodesToFill[i] = solutionNode;
                nodesToFill[i].isFilled = false;
            }
        }
    }

    bool LevelComplete(int _currentLevel)
    {
        for (int i = 0; i < numberOfNodesToFill; i++)
        {
            foreach (GameObject n in gridController.lineNodes)
            {
                if ((Vector2)n.transform.position == (Vector2)solutionNodesByLevel[_currentLevel - 1, i] && nodesToFill[i].isFilled == false && n.GetComponent<Node>().nodeWasPickedUp == false)
                {
                    nodesToFill[i].isFilled = true;
                    solutionNodeCounter++;
                    if (solutionNodeCounter == numberOfNodesToFill)
                    {
                        Debug.Log("Level Complete");
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
