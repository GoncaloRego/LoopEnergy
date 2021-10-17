using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    public struct Nodes
    {
        public int nodeID;
        public Vector2 position;
    };

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private GameObject emptyNode;
    [SerializeField] private GameObject lineNode;
    [SerializeField] private GameObject curvedLineNode;
    [SerializeField] private GameObject startNode;
    [SerializeField] private GameObject endNode;
    [HideInInspector] public GameObject[] lineNodes;

    [HideInInspector] public List<Nodes> nodeList;
    [HideInInspector] public int emptyNodeID;
    [HideInInspector] public int startNodeID;
    [HideInInspector] public int endNodeID;
    [HideInInspector] public int lineNodeID;
    [HideInInspector] public int curvedLineNodeID;

    private Material lineNodesMaterial;
    private Material endNodeMaterial;
    private float materialFade;

    void Start()
    {
        CreateGrid();

        InitShaderGraphVariables();

        StopCoroutine("PlayWinAnimation");
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
                    GameObject nodeGameObject = Instantiate(startNode, new Vector3(x - width / 2, y - height / 2), Quaternion.identity);
                    Nodes newNode = new Nodes();
                    newNode.nodeID = startNodeID;
                    newNode.position = nodeGameObject.transform.position;
                    nodeList.Add(newNode);
                }
                else if(x == 6 && y == 8)
                {
                    GameObject nodeGameObject = Instantiate(endNode, new Vector3(x - width / 2, y - height / 2), Quaternion.identity);
                    Nodes newNode = new Nodes();
                    newNode.nodeID = endNodeID;
                    newNode.position = nodeGameObject.transform.position;
                    nodeList.Add(newNode);
                }
                else if (x == 2 && y == 5 || x == 6 && y == 4 || x == 4 && y == 6)
                {
                    GameObject nodeGameObject = Instantiate(lineNode, new Vector3(x - width / 2, y - height / 2), Quaternion.identity);
                    Nodes newNode = new Nodes();
                    newNode.nodeID = lineNodeID;
                    newNode.position = nodeGameObject.transform.position;
                    nodeList.Add(newNode);
                }
                else
                {
                    GameObject nodeGameObject = Instantiate(emptyNode, new Vector3(x - width / 2, y - height / 2), Quaternion.identity);
                    Nodes newNode = new Nodes();
                    newNode.nodeID = emptyNodeID;
                    newNode.position = nodeGameObject.transform.position;
                    nodeList.Add(newNode);
                }
            }
        }

        lineNodes = GameObject.FindGameObjectsWithTag("LineNode");
    }

    void InitShaderGraphVariables()
    {
        materialFade = 0.0f;
        lineNodesMaterial = lineNode.GetComponentInChildren<SpriteRenderer>().sharedMaterial;
        lineNodesMaterial.SetColor("_Color", Color.white);
        lineNodesMaterial.SetFloat("_Fade", 1);

        endNodeMaterial = endNode.transform.GetChild(0).GetChild(0).GetComponent<Image>().material;
        endNodeMaterial.SetInt("_PlayerWon", 0);
    }

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
    }
}
