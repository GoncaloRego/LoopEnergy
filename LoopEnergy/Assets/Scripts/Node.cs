using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private Vector2 initialMousePosition;
    private Vector2 initialNodePosition;
    private Vector2 nodeCurrentPosition;
    [HideInInspector] public bool nodeWasPickedUp;
    GridController gridController;

    private void Start()
    {
        gridController = GameObject.Find("GridManager").GetComponent<GridController>();
    }

    void Update()
    {
        if (nodeWasPickedUp == true)
        {
            transform.position = initialNodePosition + (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - initialMousePosition;
            transform.position = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            nodeCurrentPosition = transform.position;
        }
    }

    private void OnMouseDown()
    {
        foreach (GameObject node in gridController.lineNodes)
        {
            if(this.gameObject == node)
            {
                nodeWasPickedUp = true;
                initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                initialNodePosition = transform.position;
            }
        }
    }

    private void OnMouseUp()
    {
        foreach (GameObject node in gridController.lineNodes)
        {
            if (this.gameObject == node)
            {
                foreach (var n in gridController.nodeList)
                {
                    if (nodeCurrentPosition == n.position && n.nodeID != gridController.emptyNodeID || nodeCurrentPosition == n.position && n.nodeID == gridController.lineNodeID)
                    {
                        transform.position = initialNodePosition;
                    }
                    nodeWasPickedUp = false;
                }
            }
        }
    }
}
