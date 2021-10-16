using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool LevelBeforeIsComplete(int levelID)
    {
        if(levelID == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
