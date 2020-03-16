using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingCondition : TacticsMove
{
    //public GameObject RotateOptions;
    //public GameObject Board;
    Animator anim;

    public GameObject SideColliders;
    public GameObject DestroyBase;
    private int ColliderCount = 0;

    // Start is called before the first frame update
    void Awake()
    {
        RotateOptions.SetActive(false);
        anim = Board.GetComponent<Animator>();
        SideColliders.SetActive(false);
        DestroyBase.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if not in trigger set true?
    }

    private void OnTriggerStay(Collider other)
    {
        if ((KingAbleE == true && other.gameObject.tag == "ElfPlayer") || (KingAbleW == true && other.gameObject.tag == "WizardPlayer"))
        {
            Debug.Log("King: select side to smash.");
            //check that player didn't start in King

            RotateOptions.SetActive(true);
            SideColliders.SetActive(true);
            SmashSide = true;
            //rotate cube

            //checkclick and rotate
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Rotator")
                    {

                        GameObject r = hit.collider.gameObject;
                        //SideColliders.SetActive(true); //set sides for when rotate

                        if (r.name == "RotateS2")
                        {
                            Board.transform.Rotate(90, 0, 0);
                        }
                        else if (r.name == "RotateS4")
                            Board.transform.Rotate(-90, 0, 0);
                        else if (r.name == "RotateS5")
                            Board.transform.Rotate(0, 0, -90);
                        else if (r.name == "RotateS6")
                            Board.transform.Rotate(0, 0, 90);

                        if(other.gameObject.tag == "ElfPlayer")
                            KingAbleE = false;
						if(other.gameObject.tag == "WizardPlayer")
							KingAbleW = false;

                        anim.Play("Squash1");
                        SmashSide = false;

                        //rename sides so bottom is tag Side3
                        SideColliders.SetActive(false);

                        //destroy
                        checkForDestroyed();
                        DestroyBase.SetActive(true);
                        //DestroyBase.SetActive(false);

                    } //end if collider is rotator
                }

                SideColliders.SetActive(false);
                RotateOptions.SetActive(false);
                DestroyBase.SetActive(false);

                //if piece starts as king, KingAble = false
                //isFalseK = true;
                if(ColliderCount == 0)
                {
                    KingAbleE = true;
                    KingAbleW = true;
                }
                else if (other.gameObject.tag == "ElfPlayer")
                {
                    KingAbleE = false;
                }
                else if (other.gameObject.tag == "WizardPlayer")
                {
                    KingAbleW = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Regain King ability");
        //isFalseK = false;
        ColliderCount--;


        if (other.gameObject.tag == "ElfPlayer")
			KingAbleE = true;
		if (other.gameObject.tag == "WizardPlayer")
			KingAbleW = true;
		
        RotateOptions.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        ColliderCount++;
    }

    private void checkForDestroyed()
    {
        //delete characters on bottom 
        GameObject[] Elves, Wizards;
        Elves = GameObject.FindGameObjectsWithTag("ElfPlayer");
        Wizards = GameObject.FindGameObjectsWithTag("WizardPlayer");

        Debug.Log(Elves[0] + "" + Elves[1] + "" + Elves[2] + "" + Elves[3]);

        for( int i = 0; i < Elves.Length; i++)
        {
            RaycastHit hit2;
            GameObject tile = null;

            Debug.DrawRay(transform.position, -Elves[i].transform.up);//

            if (Physics.Raycast(Elves[i].transform.position, -Elves[i].transform.up, out hit2, 1))
            {
                tile = hit2.collider.gameObject; // GetComponent<Tile>();

                Debug.Log("******* " + tile);
                Debug.Log("********* " + tile.transform.parent.parent.tag);
                if (tile.transform.parent.parent.tag == "Side3")
                {
                    Debug.Log(Elves[i] + " was destroyed.");
                    Destroy(Elves[i]);
                }
            }
        }
        for( int i = 0; i < Wizards.Length; i++)
        {
            RaycastHit hit2;
            GameObject tile = null;

            if (Physics.Raycast(Wizards[i].transform.position, -Wizards[i].transform.up, out hit2, 1))
            {
                tile = hit2.collider.gameObject; // GetComponent<Tile>();

                Debug.Log("******* " + tile);
                Debug.Log("********* " + tile.transform.parent.parent.tag);
                if (Wizards[i].transform.parent.parent.tag == "Side3")
                {
                    Debug.Log(Wizards[i] + " was destroyed.");
                    Destroy(Wizards[i]);
                }
            }
        }

        /*foreach (GameObject elf in Elves) //test if opponent squashed
        {
            RaycastHit hit2;
            GameObject tile = null;

            Debug.DrawRay(transform.position, -transform.up);//

            if (Physics.Raycast(elf.transform.position, -elf.transform.up, out hit2, 1))
            {
                tile = hit2.collider.gameObject; // GetComponent<Tile>();

                Debug.Log("******* " + tile);
                Debug.Log("********* " + tile.transform.parent.parent.tag);
                if (tile.transform.parent.parent.tag == "Side3")
                {
                    Destroy(elf);
                    Debug.Log(elf + " was destroyed.");
                }
            }
        }*/
        /*foreach (GameObject wizard in Wizards) //test if opponent squashed
        {
            RaycastHit hit2;
            GameObject tile = null;

            if (Physics.Raycast(wizard.transform.position, -wizard.transform.up, out hit2, 1))
            {
                tile = hit2.collider.gameObject; // GetComponent<Tile>();

                Debug.Log("******* " + tile);
                Debug.Log("********* " + tile.transform.parent.parent.tag);
                if (wizard.transform.parent.parent.tag == "Side3")
                {
                    Destroy(wizard);
                    Debug.Log(wizard + " was destroyed.");
                }
            }
        }*/
    }
}
