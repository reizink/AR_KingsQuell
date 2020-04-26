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

    public GameObject PToggles, GToggles; //for selected player

	// Start is called before the first frame update
	void Start()
    {
        Init();

        EndScreen.SetActive(false);
        PausePanel.SetActive(false);
        rollScript = GameObject.Find("Canvas").GetComponentInChildren<RollDie>();
    }

    public void ToggleIsClicked(GameObject x)
    {
        SelectedPiece = x;
        IsPieceSelected = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, transform.forward);
        GameObject[] Others, Enemies;
        Others = GameObject.FindGameObjectsWithTag("ElfPlayer");
        Enemies = GameObject.FindGameObjectsWithTag("WizardPlayer");

        if (!turn) //|| !IsPieceSelected)
        {
            return;
        }

        //label Turn
        if (turn && tag == "WizardPlayer" && (rollScript.Rolled == false))// || IsPieceSelected == false))
        {
            rollScript.PlayerRoll.text = "Wizard's Turn";
            rollScript.PlayerRoll.color = Color.magenta;
            PausePanel.SetActive(true);
            rollScript.RollButton.SetActive(true);
            PToggles.SetActive(true);
            GToggles.SetActive(false);
        }
        else if (turn && tag == "ElfPlayer" && (rollScript.Rolled == false))// !&&?, || IsPieceSelected == false))
        {
            rollScript.PlayerRoll.text = "Elf's Turn";
            rollScript.PlayerRoll.color = Color.green;
            PausePanel.SetActive(true);
            rollScript.RollButton.SetActive(true);
            GToggles.SetActive(true);
            PToggles.SetActive(false);
        }
        else
        {
            rollScript.PlayerRoll.text = "";
            PausePanel.SetActive(false);
            rollScript.RollButton.SetActive(false);
            //GToggles block toggles from click ////////
        }

        if (rollScript.Rolled == true)
        {
            rollScript.RollButton.SetActive(false);
        }

        if (rollScript.Rolled == true && RotateOptions.activeInHierarchy == false)// && IsPieceSelected == true)
        {
            if (!moving)
            {
                FindSelectableTiles2();
                CheckClick();
            }
            else
            {
                Move();
                //Debug.Log("Find Side Tag: " + CurTile.transform.parent.parent.tag);

                FindRotation(transform.gameObject);

                /////////////// Fix to find equivalent tile ///////////////////////////////////////////
                foreach (GameObject other in Others) //test if opponent squashed
                {
                    if (tag == "WizardPlayer" && transform.position == other.transform.position)
                    {
                        Debug.Log("Player Squashed!!");
                        Destroy(other);
                    }
                }

                foreach (GameObject enemy in Enemies) //test if opponent squashed
                {
                    if (tag == "ElfPlayer" && transform.position == enemy.transform.position)
                    {
                        Debug.Log("Player Squashed!!");
                        Destroy(enemy);
                    }
                }
            }

            if (Others == null || Others.Length == 0) //Wizard winner end screen
            {
                Debug.Log("The Wizards have won!");
                turn = false;

                EndScreen.SetActive(true);
                Winner.text = "Wizards";
                ElvesLeft.text = "0";
                WizardsLeft.text = Enemies.Length.ToString();
            }
            if (Enemies == null || Enemies.Length == 0) //Elf winner end screen
            {
                Debug.Log("The Elves have won!");
                turn = false;

                EndScreen.SetActive(true);
                Winner.text = "Elves";
                ElvesLeft.text = Others.Length.ToString();
                WizardsLeft.text = "0";
            }
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
                }
            }
        }
    }
}
