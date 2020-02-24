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
    public float jumpVelocity = 4.5f;

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

    public bool SmashSide = false;

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
        //Debug.Log("current tile: " + GetTargetTile(gameObject));
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;

        //Debug.Log(side);

        if (Physics.Raycast(target.transform.position, -transform.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        /*if(side == "Side1Spaces" && !turn)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        */

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

        Debug.Log(tile.transform.parent.parent.name);// side at end of turn
    }

    public string GetSide(Tile tile)
    {
        string x;

        x = tile.transform.parent.parent.name;
        side = x;

        return x;
    }

    public void FindRotation()
    {
        if(side == "Side2Spaces")
            transform.Rotate(0, 0, 90, Space.Self);
    }

    public void Move()
    {
        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            //calculate unit position on tile
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

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

            Debug.Log("Target: " + target + "  More moves: " + RollScript.EndRoll);
        }
        else
        {
            RemoveSelectableTiles();
            RollScript.EndRoll--;

            if(RollScript.EndRoll <= 0)
            {
                moving = false;
            }

            if (side != GetSide(SideTile))
            {
                Debug.Log("SideChanged"); //rotate & move player in script
                FindRotation();
            }

            if(SmashSide == true)
            {
                Debug.Log("Smash a Side");
            }

            if (moving == false && SmashSide == false) //multiple moves//////////////////////
                TurnManager.EndTurn();
            else if(moving == true && SmashSide == false)
                Move();
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
