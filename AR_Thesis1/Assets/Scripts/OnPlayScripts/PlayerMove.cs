using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : TacticsMove
{
    public RollDie rollScript;
    public GameObject PausePanel;

    public GameObject EndScreen;
    public Text Winner;
    public Text ElvesLeft, WizardsLeft;

	// Start is called before the first frame update
	void Start()
    {
        Init();

        EndScreen.SetActive(false);
        PausePanel.SetActive(false);
        rollScript = GameObject.Find("Canvas").GetComponentInChildren<RollDie>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, transform.forward);
        GameObject[] Others, Enemies;
        Others = GameObject.FindGameObjectsWithTag("ElfPlayer");
        Enemies = GameObject.FindGameObjectsWithTag("WizardPlayer");

        if (!turn)
        {
            return;
        }

        //label Turn
        if (turn && tag == "WizardPlayer" && rollScript.Rolled == false)
        {
            rollScript.PlayerRoll.text = "Wizard's Turn";
            rollScript.PlayerRoll.color = Color.magenta;
            PausePanel.SetActive(true);
            rollScript.RollButton.SetActive(true);
        }
        else if (turn && tag == "ElfPlayer" && rollScript.Rolled == false)
        {
            rollScript.PlayerRoll.text = "Elf's Turn";
            rollScript.PlayerRoll.color = Color.green;
            PausePanel.SetActive(true);
            rollScript.RollButton.SetActive(true);
        }
        else
        {
            rollScript.PlayerRoll.text = "";
            PausePanel.SetActive(false);
            rollScript.RollButton.SetActive(false);
        }


        if (!moving)
        {
            FindSelectableTiles2();
            CheckClick();
        }
        else
        {
            //if(PausePanel.activeInHierarchy == false) //needed?
                Move();

            foreach (GameObject other in Others)
            {
                if (tag == "WizardPlayer" && transform.position == other.transform.position)
                {
                    Debug.Log("Player Squashed!!");
                    Destroy(other);
                }
            }

            foreach (GameObject enemy in Enemies)
            {
                if (tag == "ElfPlayer" && transform.position == enemy.transform.position)
                {
                    Debug.Log("Player Squashed!!");
                    Destroy(enemy);
                }
            }
        }

        if(Others == null || Others.Length == 0)
        {
            Debug.Log("The Wizards have won!");
            turn = false;

            EndScreen.SetActive(true);
            Winner.text = "Wizards";
            ElvesLeft.text = "0";
            WizardsLeft.text = Enemies.Length.ToString();
        }
        if (Enemies == null || Enemies.Length == 0)
        {
            Debug.Log("The Elves have won!");
            turn = false;

            EndScreen.SetActive(true);
            Winner.text = "Elves";
            ElvesLeft.text = Others.Length.ToString();
            WizardsLeft.text = "0";
        }
    }

    void CheckClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if (t.selectable)
                    {
                        MoveToTile(t);
                    }

                    SideTile = t;
                    //if(side != GetSide(t))
                    {
                        //Debug.Log("SideChanged"); //rotate & move player in script
                        //FindRotation();
                    }
                }
            }
        }
    }
}
