using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    static Dictionary<string, List<TacticsMove>> units = new Dictionary<string, List<TacticsMove>>();
    static Queue<string> turnKey = new Queue<string>(); //who's turn it is
    static Queue<TacticsMove> turnTeam = new Queue<TacticsMove>();

    // Update is called once per frame
    void Update()
    {
        if(turnTeam.Count == 0)
        {
            InitTeamTurnQueue();
        }
    }

    static void InitTeamTurnQueue()
    {
        List<TacticsMove> teamList = units[turnKey.Peek()];

        foreach (TacticsMove unit in teamList)
        {
            turnTeam.Enqueue(unit);
        }

        StartTurn();
    }

    public static void SelectCharacter()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if ((hit.collider.tag == "ElfPlayer" && turnKey.Peek() == "ElfPlayer") || (hit.collider.tag == "WizardPlayer" && turnKey.Peek() == "WizardPlayer"))
                {
                    TacticsMove g = hit.collider.GetComponent<TacticsMove>();
                    Debug.Log("G = " + g);

                    turnTeam.Clear();
                    turnTeam.Enqueue(g);
                }
            }
        }
    }

    /*public static void IsKing()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Rotator")
                {
                    EndTurn();
                }
            }
        }
    }*/

    public static void StartTurn()
    {
        SelectCharacter();

        if(turnTeam.Count > 0)
        {
            turnTeam.Peek().BeginTurn();
        }
        //else team empty

        //Debug.Log("Turnkey: " + turnKey.Peek());
        Debug.Log("TurnTeam: " + turnTeam.Peek());
    }

    public static void EndTurn()
    {
        TacticsMove unit = turnTeam.Dequeue(); //remove item
        unit.EndTurn();

        if(turnTeam.Count > 0)
        {
            StartTurn();
        }
        else
        {
            string team = turnKey.Dequeue(); //next Team
            turnKey.Enqueue(team); //add team to end of queue
            InitTeamTurnQueue();
        }
    }

    public static void AddUnit(TacticsMove unit)
    {
        List<TacticsMove> list;

        if (!units.ContainsKey(unit.tag))
        {
            list = new List<TacticsMove>();
            units[unit.tag] = list;

            if (!turnKey.Contains(unit.tag))
            {
                turnKey.Enqueue(unit.tag);
            }
        }
        else
        {
            list = units[unit.tag];
        }

        list.Add(unit);
    }
}
