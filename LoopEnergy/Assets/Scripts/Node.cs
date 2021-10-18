using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    emptyNodeID,
    startNodeID,
    endNodeID,
    lineNodeID,
    curvedLineNodeID
}

public class Node : MonoBehaviour
{
    private Vector2 initialMousePosition;
    [HideInInspector] public Vector2 initialNodePosition;
    public Vector2 nodeCurrentPosition { get; private set; }
    [HideInInspector] public bool nodeWasPickedUp;
    public NodeType nodeID;
    [HideInInspector] public Transform cachedTransform;

    public GameManager gameManager;
    public LevelManager levelManager;
    public GridController gridController;

    private void Start()
    {
        cachedTransform = transform;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gridController = GameObject.Find("GridController").GetComponent<GridController>();
    }

    void Update()
    {
        if (nodeWasPickedUp == true)
        {
            cachedTransform.position = initialNodePosition + (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - initialMousePosition;
            cachedTransform.position = new Vector2(Mathf.RoundToInt(cachedTransform.position.x), Mathf.RoundToInt(cachedTransform.position.y));
            nodeCurrentPosition = cachedTransform.position;
        }

        Debug.Log(gridController.nodeList.Count);
    }

    private void OnMouseDown()
    {
        if(this.nodeID == NodeType.lineNodeID || this.nodeID == NodeType.curvedLineNodeID)
        {
            nodeWasPickedUp = true;
            initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            initialNodePosition = cachedTransform.position;
        }
    }

    private void OnMouseUp()
    {
        switch (this.nodeID)
        {
            case NodeType.lineNodeID:
                gridController.SetNodeToInitialPosition(this);
                nodeWasPickedUp = false;
                break;

            case NodeType.curvedLineNodeID:
                gridController.SetNodeToInitialPosition(this);
                nodeWasPickedUp = false;
                break;
            default: 
                break;
        }

        levelManager.levelComplete[levelManager.currentLevel] = gameManager.LevelComplete(levelManager.currentLevel);
    }
}
