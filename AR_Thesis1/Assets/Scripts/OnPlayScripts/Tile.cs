using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;

    public List<Tile> adjacencyList = new List<Tile>();

    //BFS
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (current) {
            GetComponent<Renderer>().material.color = Color.magenta;
        } else if(target) {
            GetComponent<Renderer>().material.color = Color.green;
        } else if (selectable) {
            GetComponent<Renderer>().material.color = Color.blue;
        } else {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void Reset()
    {
        walkable = true;
        current = false;
        target = false;
        selectable = false;

        adjacencyList = new List<Tile>();

        visited = false;
        parent = null;
        distance = 0;
    }

    public void FindNeighbors(float jumpHeight)
    {
        Reset();

        CheckTile(Vector3.forward, jumpHeight);
        CheckTile(-Vector3.forward, jumpHeight);
        CheckTile(Vector3.right, jumpHeight);
        CheckTile(-Vector3.right, jumpHeight);
    }

    public void CheckTile(Vector3 direction, float jumpHeight)
    {
        Vector3 halfExtents = new Vector3(2.5f, (1 + jumpHeight) / 2.0f, 2.5f);//////(02.5f, (10 + jumpHeight) / 2.0f, 02.5f)//.65 cut off
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach(Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if(tile != null && tile.walkable)
            {
                RaycastHit hit;

                if(!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
                {
                    adjacencyList.Add(tile);
                    //Debug.Log("Adjacent: " + tile);
                    //Debug.DrawRay(tile.transform.position, Vector3.up, Color.red);
                }
            }
        }
    }
}
