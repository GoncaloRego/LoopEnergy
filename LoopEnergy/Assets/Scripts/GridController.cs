using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public struct Nodes
    {
        public int nodeID;
        public Vector2 position;
    };

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Node emptyNode;
    [SerializeField] private Node lineNode;
    [SerializeField] private Node curvedLineNode;
    [SerializeField] private Node startNode;
    [SerializeField] private Node endNode;
    [HideInInspector] public GameObject[] lineNodes;

    [HideInInspector] public List<Nodes> nodeList;
    [HideInInspector] public int emptyNodeID;
    [HideInInspector] public int startNodeID;
    [HideInInspector] public int endNodeID;
    [HideInInspector] public int lineNodeID;
    [HideInInspector] public int curvedLineNodeID;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        nodeList = new List<Nodes>();
        emptyNodeID = 0;
        startNodeID = 1;
        endNodeID = 2;
        lineNodeID = 3;
        curvedLineNodeID = 4;

        // Level 1
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 2 && y == 8)
                {
                    var nodeGameObject = Instantiate(startNode, new Vector3(x - width / 2, y - height / 2), Quaternion.identity);
                    Nodes newNode = new Nodes();
                    newNode.nodeID = startNodeID;
                    newNode.position = nodeGameObject.transform.position;
                    nodeList.Add(newNode);
                }
                else if(x == 6 && y == 8)
                {
                    var nodeGameObject = Instantiate(endNode, new Vector3(x - width / 2, y - height / 2), Quaternion.identity);
                    Nodes newNode = new Nodes();
                    newNode.nodeID = endNodeID;
                    newNode.position = nodeGameObject.transform.position;
                    nodeList.Add(newNode);
                }
                else if (x == 2 && y == 5 || x == 6 && y == 4 || x == 4 && y == 6)
                {
                    var nodeGameObject = Instantiate(lineNode, new Vector3(x - width / 2, y - height / 2), Quaternion.identity);
                    Nodes newNode = new Nodes();
                    newNode.nodeID = lineNodeID;
                    newNode.position = nodeGameObject.transform.position;
                    nodeList.Add(newNode);
                }
                else
                {
                    var nodeGameObject = Instantiate(emptyNode, new Vector3(x - width / 2, y - height / 2), Quaternion.identity);
                    Nodes newNode = new Nodes();
                    newNode.nodeID = emptyNodeID;
                    newNode.position = nodeGameObject.transform.position;
                    nodeList.Add(newNode);
                }
            }
        }

        lineNodes = GameObject.FindGameObjectsWithTag("LineNode");
    }
}
