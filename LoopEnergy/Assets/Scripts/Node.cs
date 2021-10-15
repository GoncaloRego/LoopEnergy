using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private GameObject highlight;
    private int nodeID;

    private void OnMouseDrag()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
        highlight.SetActive(true);
    }

    private void OnMouseUp()
    {
        highlight.SetActive(false);
    }
}
