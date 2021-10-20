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
        if(Application.isMobilePlatform)
        {
            MobileTouchMove();
        }
        else if(Application.isEditor)
        {
            if (nodeWasPickedUp == true)
            {
                cachedTransform.position = initialNodePosition + (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - initialMousePosition;
                cachedTransform.position = new Vector2(Mathf.RoundToInt(cachedTransform.position.x), Mathf.RoundToInt(cachedTransform.position.y));
            }
        }
    }

    private void OnMouseDown()
    {
        if (this.nodeID == NodeType.lineNodeID || this.nodeID == NodeType.curvedLineNodeID)
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

                // Check if Level is complete
                levelManager.levelComplete[levelManager.currentLevel] = gameManager.LevelComplete(levelManager.currentLevel);
                break;

            case NodeType.curvedLineNodeID:
                gridController.SetNodeToInitialPosition(this);
                nodeWasPickedUp = false;

                // Check if Level is complete
                levelManager.levelComplete[levelManager.currentLevel] = gameManager.LevelComplete(levelManager.currentLevel);
                break;
            default:
                break;
        }
    }

    private void MobileTouchMove()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                if (touchPosition == cachedTransform.position && (this.nodeID == NodeType.lineNodeID || this.nodeID == NodeType.curvedLineNodeID))
                {
                    nodeWasPickedUp = true;
                    initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    initialNodePosition = cachedTransform.position;
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (touchPosition == cachedTransform.position && (this.nodeID == NodeType.lineNodeID || this.nodeID == NodeType.curvedLineNodeID))
                {
                    cachedTransform.position = initialNodePosition + (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - initialMousePosition;
                    cachedTransform.position = new Vector2(Mathf.RoundToInt(cachedTransform.position.x), Mathf.RoundToInt(cachedTransform.position.y));
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (touchPosition == cachedTransform.position && (this.nodeID == NodeType.lineNodeID || this.nodeID == NodeType.curvedLineNodeID))
                {
                    switch (this.nodeID)
                    {
                        case NodeType.lineNodeID:
                            gridController.SetNodeToInitialPosition(this);
                            nodeWasPickedUp = false;

                            // Check if Level is complete
                            levelManager.levelComplete[levelManager.currentLevel] = gameManager.LevelComplete(levelManager.currentLevel);
                            break;

                        case NodeType.curvedLineNodeID:
                            gridController.SetNodeToInitialPosition(this);
                            nodeWasPickedUp = false;

                            // Check if Level is complete
                            levelManager.levelComplete[levelManager.currentLevel] = gameManager.LevelComplete(levelManager.currentLevel);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
