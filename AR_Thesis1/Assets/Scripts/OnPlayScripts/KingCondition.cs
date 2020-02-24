using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingCondition : TacticsMove
{
    public GameObject RotateOptions;
    public GameObject Board;

    // Start is called before the first frame update
    void Awake()
    {
        RotateOptions.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if ((KingAbleE == true && other.gameObject.tag == "ElfPlayer") || (KingAbleW == true && other.gameObject.tag == "WizardPlayer"))
        {
            Debug.Log("King: select side to smash.");
            //check that player didn't start in King

            RotateOptions.SetActive(true);
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

                        if (r.name == "RotateS2")
                        {
                            Board.transform.Rotate(90, 0, 0);
                            Vector3 CurPos = Board.transform.position;
                            Board.transform.position = Vector3.Lerp(CurPos, new Vector3(0f,0f,0f), Time.deltaTime);
                            //Board.transform.position = Vector3.Lerp(Board.transform.position, CurPos, Time.deltaTime);
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

                        SmashSide = false;
					}
                }

                RotateOptions.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Regain King ability");

		if (other.gameObject.tag == "ElfPlayer")
			KingAbleE = true;
		if (other.gameObject.tag == "WizardPlayer")
			KingAbleE = true;
		
        RotateOptions.SetActive(false);
    }
}
