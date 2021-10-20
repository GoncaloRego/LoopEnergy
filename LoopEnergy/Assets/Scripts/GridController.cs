using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Node emptyNode;
    [SerializeField] private Node lineNode;
    [SerializeField] private Node curvedLineNode;
    [SerializeField] private Node startNode;
    [SerializeField] private Node endNode;

    public List<Node> nodeList;

    private Material lineNodesMaterial;
    private Material endNodeMaterial;
    private Material backgroundMaterial;
    private float materialFade;

    private bool winAnimationHasEnded;

    public LevelManager levelManager;

    void Start()
    {
        CreateGrid();

        InitShaderGraphVariables();

        StopCoroutine("PlayWinAnimation");

        winAnimationHasEnded = false;
    }

    // Set Positions for all nodes depending on the level
    void CreateGrid()
    {
        switch (levelManager.currentLevel)
        {
            case 0:
                GenerateGridLevel1();
                break;
            case 1:
                GenerateGridLevel2();
                break;
            default:
                break;
        }
    }

    void InitShaderGraphVariables()
    {
        Color backgroundTopColor;
        Color backgroundBottomColor;

        materialFade = 0.0f;
        lineNodesMaterial = lineNode.GetComponentInChildren<SpriteRenderer>().sharedMaterial;
        lineNodesMaterial.SetColor("_Color", Color.white);
        lineNodesMaterial.SetFloat("_Fade", 1f);
        endNodeMaterial = endNode.transform.GetChild(0).GetChild(0).GetComponent<Image>().material;
        endNodeMaterial.SetInt("_PlayerWon", 0);

        backgroundMaterial = GameObject.Find("Background").GetComponent<SpriteRenderer>().sharedMaterial;

        //Background Color Control
        if (levelManager.currentLevel == 0)
        {
            backgroundTopColor = new Color(94, 224, 161);
            backgroundBottomColor = new Color(255, 170, 81);
            backgroundMaterial.SetColor("_TopColor", Color.yellow);
            backgroundMaterial.SetColor("_BottomColor", Color.green);
        }
        else if(levelManager.currentLevel == 1)
        {
            backgroundTopColor = new Color(94, 224, 161);
            backgroundBottomColor = new Color(255, 170, 81);
            backgroundMaterial.SetColor("_TopColor", Color.grey);
            backgroundMaterial.SetColor("_BottomColor", Color.black);
        }
    }

    void GenerateGridLevel1()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 2 && y == 8)
                {
                    InstantiateNode(startNode, NodeType.startNodeID, x, y);
                }
                else if (x == 6 && y == 8)
                {
                    InstantiateNode(endNode, NodeType.endNodeID, x, y);
                }
                else if (x == 2 && y == 10 || x == 6 && y == 9 || x == 4 && y == 9)
                {
                    InstantiateNode(lineNode, NodeType.lineNodeID, x, y);
                }
                else
                {
                    InstantiateNode(emptyNode, NodeType.emptyNodeID, x, y);
                }
            }
        }
    }

    void GenerateGridLevel2()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 2 && y == 9)
                {
                    InstantiateNode(startNode, NodeType.startNodeID, x, y);
                }
                else if (x == 6 && y == 8)
                {
                    InstantiateNode(endNode, NodeType.endNodeID, x, y);
                }
                else if (x == 3 && y == 5 || x == 6 && y == 9 || x == 4 && y == 8)
                {
                    InstantiateNode(lineNode, NodeType.lineNodeID, x, y);
                }
                else if (x == 5 && y == 6)
                {
                    InstantiateNode(curvedLineNode, NodeType.curvedLineNodeID, x, y);
                }
                else
                {
                    InstantiateNode(emptyNode, NodeType.emptyNodeID, x, y);
                }
            }
        }
    }

    void InstantiateNode(Node node, NodeType nodeID, int x, int y)
    {
        Node nodeGameObject = Instantiate(node, new Vector3(x - width / 2, y - height / 2), Quaternion.identity);
        nodeGameObject.nodeID = nodeID;
        nodeList.Add(nodeGameObject);
    }

    // function called when the player drops a line or curved line node on top of another node which isn't an empty node
    public void SetNodeToInitialPosition(Node node)
    {
        foreach (Node n in nodeList)
        {
            if (node != n) 
            {
                if (node.cachedTransform.position == n.cachedTransform.position && n.nodeID != NodeType.emptyNodeID)
                {
                    node.cachedTransform.position = node.initialNodePosition;
                }
            }
        }
    }

    public bool WinAnimationEnded()
    {
        return winAnimationHasEnded;
    }

    // Shader Graph Win Animation
    public IEnumerator PlayWinAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        materialFade += Time.deltaTime;

        if (materialFade >= 1.0f)
        {
            materialFade = 1.0f;
        }

        lineNodesMaterial.SetFloat("_Fade", materialFade);
        lineNodesMaterial.SetColor("_Color", Color.green);
        yield return new WaitForSeconds(0.5f);
        endNodeMaterial.SetInt("_PlayerWon", 1);
        yield return new WaitForSeconds(0.5f);
        winAnimationHasEnded = true;
    }
}
