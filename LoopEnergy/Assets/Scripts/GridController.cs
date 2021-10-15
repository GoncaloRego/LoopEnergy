using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Node emptyNode;
    [SerializeField] private Node lineNode;
    [SerializeField] private Node curvedLineNode;
    [SerializeField] private Node startNode;
    [SerializeField] private Node endNode;
    private List<Node> nodes;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        nodes = new List<Node>();

        // Level 1
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 3 && y == 8)
                {
                    var nodeGameObject = Instantiate(startNode, new Vector3(x - (float)width / 2, y - (float)height / 2), Quaternion.identity);
                    nodes.Add(startNode);
                }
                else if(x == 6 && y == 8)
                {
                    var nodeGameObject = Instantiate(endNode, new Vector3(x - (float)width / 2, y - (float)height / 2), Quaternion.identity);
                    nodes.Add(endNode);
                }
                else if ((x == 3 || x == 6) && y == 5)
                {
                    var nodeGameObject = Instantiate(lineNode, new Vector3(x - (float)width / 2, y - (float)height / 2), Quaternion.identity);
                    nodes.Add(lineNode);
                }
                else
                {
                    var nodeGameObject = Instantiate(emptyNode, new Vector3(x - (float)width / 2, y - (float)height / 2), Quaternion.identity);
                    nodes.Add(emptyNode);
                }
            }
        }
    }
}
