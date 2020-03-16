using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    static Dictionary<string, List<TacticsMove>> units = new Dictionary<string, List<TacticsMove>>();
    static Queue<string> turnKey = new Queue<string>(); //who's turn it is
    static Queue<TacticsMove> turnTeam = new Queue<TacticsMove>();

    static TacticsMove selectedTurn = null; //

    void Update()
    {
        if(turnTeam.Count == 0)
        {
            InitTeamTurnQueue();
        }
        /*if(selectedTurn == null)
        {
            SelectCharacter();
        }

        Debug.Log(selectedTurn);*/

        /*if(selectedTurn != null)
        {
            turnTeam.Clear();
            turnTeam.Enqueue(selectedTurn);
            //StartTurn();
        }*/
    }

    static void InitTeamTurnQueue()
    {
        List<TacticsMove> teamList = units[turnKey.Peek()];
        //Debug.Log("teamList output: " + turnKey.Peek());

        foreach (TacticsMove unit in teamList)
        {
            turnTeam.Enqueue(unit);
        }
        //SelectCharacter(); //

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
                //if (turnTeam.Contains(hit.collider.GetComponent<TacticsMove>())) //team contains player
                {
                    //selectedTurn = hit.collider.GetComponent<TacticsMove>();
                    //Debug.Log("G = " + selectedTurn);
                }

                if ((hit.collider.tag == "ElfPlayer" && turnKey.Peek() == "ElfPlayer") || (hit.collider.tag == "WizardPlayer" && turnKey.Peek() == "WizardPlayer"))
                {
                    selectedTurn = hit.collider.GetComponent<TacticsMove>();
                    //Debug.Log("G = " + selectedTurn);

                    //turnTeam.Clear();
                    //turnTeam.Enqueue(selectedTurn);
                }
            }
        }

        //selectedTurn.BeginTurn();
    }

    public static void StartTurn()
    {
        if(turnTeam.Count > 0)
        {
            turnTeam.Peek().BeginTurn();
            //selectedTurn.BeginTurn();
        }
        //else team empty

        //Debug.Log("Turnkey: " + turnKey.Peek());
        //Debug.Log("TurnTeam: " + turnTeam.Peek());
    }

    public static void EndTurn()
    {
        TacticsMove unit = turnTeam.Dequeue(); //remove item
        unit.EndTurn();
        //selectedTurn.EndTurn();
        //selectedTurn = null;

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

    public static void AddUnit(TacticsMove unit) // make teams list: elf, Wizard
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
