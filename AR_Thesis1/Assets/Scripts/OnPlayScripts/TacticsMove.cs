using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacticsMove : MonoBehaviour
{
    public bool turn = false;

    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;


    public bool moving = false;
    public float jumpHeight = 2;
    public float moveSpeed = .3f; //2
    public float jumpVelocity = 1.5f;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0;

    bool fallingDown = false;
    bool jumpingHigh = false;
    bool movingEdge = false;
    Vector3 jumpTarget;

    RollDie RollScript;
    public string side = "Side1Spaces";
    public bool KingAbleE = false, KingAbleW = false;
    public Tile SideTile;
    public GameObject CurTile; //, CurTileW;

    public bool SmashSide = false;
    public GameObject Board;

    private void Awake()
    {
        RollScript = GameObject.Find("RollDieButton").GetComponent<RollDie>();
    }

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;

        TurnManager.AddUnit(this);
    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;

        //Debug.Log(side);

        if (Physics.Raycast(target.transform.position, -transform.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();


            CurTile = tile.gameObject; //current tile gameObject
            //CurTileW = tile.gameObject;
        }

        //rotation check?
        //FindRotation(CurTile); // ///////////////////////////////////////////////

        return tile;
    }

    public void ComputeAdjacencyLists()
    {
        foreach(GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.AddAdjacent(t); //all 4 tiles added to adjacencylist
        }
    }

    public void FindSelectableTiles2()
    {
        ComputeAdjacencyLists();
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        //set move limit here?
        process.Enqueue(currentTile.returnAdj1());
        process.Enqueue(currentTile.returnAdj2());
        process.Enqueue(currentTile.returnAdj3());
        process.Enqueue(currentTile.returnAdj4());

        selectableTiles.Add(currentTile.returnAdj1());
        currentTile.returnAdj1().selectable = true;

        selectableTiles.Add(currentTile.returnAdj2());
        currentTile.returnAdj2().selectable = true;

        selectableTiles.Add(currentTile.returnAdj3());
        currentTile.returnAdj3().selectable = true;

        selectableTiles.Add(currentTile.returnAdj4());
        currentTile.returnAdj4().selectable = true;

    }

    public void MoveToTile(Tile tile)
    {
        path.Clear();
        tile.target = true;
        moving = true;

        Tile next = tile;
        while(next != null)
        {
            path.Push(next);
            next = next.parent;
        }

        //.Log(tile.transform.parent.parent.name);// side at end of turn
        RollScript.EndRoll--; //# of moves used
    }

    public string GetSide(Tile tile)
    {
        string x;

        x = tile.transform.parent.parent.name;
        side = x;

        return x;
    }

    public void FindRotation(GameObject other) //adjust by board rotation too?
    {
        Debug.Log("Current tile: " + CurTile);

        if (CurTile.transform.parent.parent.tag == "Side1")
            other.transform.eulerAngles = new Vector3(0 + Board.transform.position.x, 0 + Board.transform.position.y, 0 + Board.transform.position.z);
        else if (CurTile.transform.parent.parent.tag == "Side2")
            other.transform.eulerAngles = new Vector3(90 + Board.transform.position.x, 0 + Board.transform.position.y, 0 + Board.transform.position.z);
        else if (CurTile.transform.parent.parent.tag == "Side3")
            other.transform.eulerAngles = new Vector3(-180 + Board.transform.position.x, 0 + Board.transform.position.y, 0 + Board.transform.position.z); //bottom
        else if (CurTile.transform.parent.parent.tag == "Side4")
            other.transform.eulerAngles = new Vector3(-90 + Board.transform.position.x, 0 + Board.transform.position.y, 0 + Board.transform.position.z);
        else if (CurTile.transform.parent.parent.tag == "Side5")
            other.transform.eulerAngles = new Vector3(0 + Board.transform.position.x, 0 + Board.transform.position.y, -90 + Board.transform.position.z);
        else if (CurTile.transform.parent.parent.tag == "Side6")
            other.transform.eulerAngles = new Vector3(0 + Board.transform.position.x, 0 + Board.transform.position.y, 90 + Board.transform.position.z);

    }

    public void Move()
    {
        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            CurTile = t.gameObject;
            //calculate unit position on tile
            Debug.Log("Side for Editing: " + GetSide(t));

            if (GetSide(t) == "Side1Spaces")
            {
                target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;
            }
            else if (GetSide(t) == "Side2Spaces")
            {
                target.z += halfHeight + t.GetComponent<Collider>().bounds.extents.z;
                target.y -= .01f;
            }
            else if (GetSide(t) == "Side3Spaces")
            {
                target.y -= halfHeight + t.GetComponent<Collider>().bounds.extents.y;
            }
            else if (GetSide(t) == "Side4Spaces")
            {
                target.z -= halfHeight + t.GetComponent<Collider>().bounds.extents.z;
                target.y -= .01f;
            }
            else if (GetSide(t) == "Side5Spaces")
            {
                target.x += halfHeight + t.GetComponent<Collider>().bounds.extents.x;
                target.y -= .01f;
            }
            else if (GetSide(t) == "Side6Spaces")
            {
                target.x -= halfHeight + t.GetComponent<Collider>().bounds.extents.x;
                target.y -= .01f;
            }

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                bool jump = transform.position.y != target.y;

                if (jump)
                {
                    Jump(target);
                }
                else
                {
                    CalculateHeading(target);
                    SetHorizontalVelocity();
                }

                //locomotion & Animations
                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                transform.position = target;
                path.Pop();
            }

            //Debug.Log("Target: " + target + "  More moves: " + RollScript.EndRoll);
        }
        else
        {
            RemoveSelectableTiles();
            
            moving = false;

            if(SmashSide == true)
            {
                Debug.Log("Smash a Side");
            }

            //StartCoroutine(WaitForClick());

            if (RollScript.EndRoll <= 0) //end turn if roll over
                TurnManager.EndTurn(); //IsKing to EndTurn()
        }
    }

    protected void RemoveSelectableTiles()
    {
        if(currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }

        foreach(Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        selectableTiles.Clear();
    }

    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    void Jump(Vector3 target)
    {
        if (fallingDown)
        {
            FallDownward(target);
        } else if (jumpingHigh)
        {
            JumpUpward(target);
        }else if (movingEdge)
        {
            MoveToEdge();
        }
        else
        {
            PrepareJump(target);
        }
    }

    void PrepareJump(Vector3 target)
    {
        float targety = target.y;
        target.y = transform.position.y;

        CalculateHeading(target);

        if(transform.position.y > targety)
        {
            fallingDown = false;
            jumpingHigh = false;
            movingEdge = true;

            jumpTarget = transform.position + (target - transform.position) / 2.0f;
        }
        else
        {
            fallingDown = false;
            jumpingHigh = true;
            movingEdge = false;

            velocity = heading * moveSpeed / 3.0f;

            float difference = targety - transform.position.y;

            velocity.y = jumpVelocity * (0.5f + difference / 2.0f);
        }
    } 

    void FallDownward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if(transform.position.y <= target.y)
        {
            fallingDown = false;
            jumpingHigh = false;
            movingEdge = false;

            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;

            velocity = new Vector3();
        }
    }

    void JumpUpward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if(transform.position.y > target.y)
        {
            jumpingHigh = false;
            fallingDown = true;
        }
    }

    void MoveToEdge()
    {
        if(Vector3.Distance(transform.position, jumpTarget) >= 0.05f)
        {
            SetHorizontalVelocity();
        }
        else
        {
            movingEdge = false;
            fallingDown = true;

            velocity /= 5.0f;
            velocity.y = 1.5f;
        }
    }

    public void BeginTurn()
    {
        turn = true;
        RollScript.Rolled = false;
    }

    public void EndTurn()
    {
        turn = false;
    }
}
