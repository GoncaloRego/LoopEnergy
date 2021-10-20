using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

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

    Touch t;

    private void Start()
    {
        cachedTransform = transform;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gridController = GameObject.Find("GridController").GetComponent<GridController>();
    }

    void Update()
    {
        if (Application.isMobilePlatform)
        {
            MobileTouchMove();
            if (nodeWasPickedUp == true)
            {
                cachedTransform.position = initialNodePosition + (Vector2)Camera.main.ScreenToWorldPoint(t.position) - initialMousePosition;
                cachedTransform.position = new Vector2(Mathf.RoundToInt(cachedTransform.position.x), Mathf.RoundToInt(cachedTransform.position.y));
            }
        }
        else if (Application.isEditor)
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
            t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(t.position);
                RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit2D.collider.transform == this.transform)
                {
                    if (this.nodeID == NodeType.lineNodeID || this.nodeID == NodeType.curvedLineNodeID)
                    {
                        nodeWasPickedUp = true;
                        initialMousePosition = Camera.main.ScreenToWorldPoint(t.position);
                        initialNodePosition = cachedTransform.position;
                    }
                }
            }
            else if (t.phase == TouchPhase.Ended)
            {
                Ray ray = Camera.main.ScreenPointToRay(t.position);
                RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit2D.collider.transform == this.transform)
                {
                    if (this.nodeID == NodeType.lineNodeID || this.nodeID == NodeType.curvedLineNodeID)
                    {
                        // Reset node position
                        gridController.SetNodeToInitialPosition(this);
                        nodeWasPickedUp = false;

                        // Check if Level is complete
                        levelManager.levelComplete[levelManager.currentLevel] = gameManager.LevelComplete(levelManager.currentLevel);
                    }
                }
            }
        }
    }
}
