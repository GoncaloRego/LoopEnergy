using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelManager), menuName = "ScriptableObjects/" + nameof(LevelManager))]
public class LevelManager : ScriptableObject
{
    public int numberOfLevels = 2;
    public int currentLevel;
    public int lastLevel;

    public int levelOne = 0;
    public int levelTwo = 1;

    public bool[] levelComplete;

    private void OnEnable()
    {
        levelComplete = new bool[numberOfLevels];
        currentLevel = levelOne;
        lastLevel = currentLevel;
    }

    public void IncrementLevel()
    {
        currentLevel = lastLevel + 1;
    }

    public bool LevelBeforeIsComplete()
    {
        return levelComplete[lastLevel];
    }
}
