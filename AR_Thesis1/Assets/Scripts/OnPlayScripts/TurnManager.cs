using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : TacticsMove
{
    static Dictionary<string, List<TacticsMove>> units = new Dictionary<string, List<TacticsMove>>();
    static Queue<string> turnKey = new Queue<string>(); //who's turn it is
    static Queue<TacticsMove> turnTeam = new Queue<TacticsMove>();

    //static TacticsMove selectedTurn = null; //

    void Update()
    {
        if(turnTeam.Count == 0)
        {
            InitTeamTurnQueue();
        }
        //if (SelectedPiece != null && turnTeam.Count > 1)
        {
            //StartTurn(SelectedPiece);
        }
    }

    static void InitTeamTurnQueue() //static
    {
        List<TacticsMove> teamList = units[turnKey.Peek()];
        //Debug.Log("teamList output: " + turnKey.Peek());

        foreach (TacticsMove unit in teamList)
        {
            turnTeam.Enqueue(unit);
        }

        //if(SelectedPiece != null)
        {
            //StartTurn(SelectedPiece);
        }
        //SelectCharacter(); //
        //else
            StartTurn();
    }

    public void SelectCharacter()
    {
        //(hit.collider.tag == "ElfPlayer" && turnKey.Peek() == "ElfPlayer") || (hit.collider.tag == "WizardPlayer" && turnKey.Peek() == "WizardPlayer"))

        TacticsMove x = SelectedPiece.GetComponent<TacticsMove>();

        //turnTeam.Clear();
        //turnTeam.Enqueue(x);
        //.BeginTurn();
    }

    public static void StartTurn() //static
    {
        if(turnTeam.Count > 0)
        {
            turnTeam.Peek().BeginTurn();
            //SelectCharacter();

            Debug.Log(" turnTeam list: " + turnTeam.Peek() + ", " + turnTeam.Count);

        }
        //SelectCharacter();
        //else team empty

        //Debug.Log("Turnkey: " + turnKey.Peek());
        //Debug.Log("TurnTeam: " + turnTeam.Peek());
    }

    public static void StartTurn(GameObject x)
    {
        TacticsMove y = x.GetComponent<TacticsMove>();
        Debug.Log("StartTurn for piece: " + x);

        if (turnTeam.Count > 0)
        {
            //turnTeam.Clear();
            //turnTeam.Enqueue(y);

            turnTeam.Peek().BeginTurn();
            //y.BeginTurn();
        }
        //Debug.Log("Turnkey: " + turnKey.Peek());
        //Debug.Log("TurnTeam: " + turnTeam.Peek());
    }

    public static void EndTurn() //static
    {
        TacticsMove unit = turnTeam.Dequeue(); //remove item
        unit.EndTurn();
        //SelectedPiece = null;

        if (turnTeam.Count > 0) //other team members turn
        {
            StartTurn();
        }
        else
        {//switch teams each time
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

    //
}
