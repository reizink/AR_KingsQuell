using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMovement : MonoBehaviour
{

    [Header("Attributes")]
    public bool EvenDragon; //even turns
    [Range(0, 100)]
    public float Health = 100f;
    [Range(0, 100)]
    public int HitPercentage = 75;
    public float Speed = 300;
    public int GoldCapacity = 5;
    //public int DragonWorth = 6;

    [Header("Ranges")]
    [Range(0, 7)]
    public float RaidRange = 2;
    [Range(0, 7)]
    public float AttackRange = 3;
    [Range(0, 7)]
    public float MoveRange = 1.5f;

    //test variables for movement
    private Vector2 TouchOrigin = -Vector3.one;
    public GameObject DragonAttached;
    private Rigidbody rb;
    //private int TapCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = DragonAttached.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];

            if(myTouch.phase == TouchPhase.Began && myTouch.tapCount % 2 == 0 && EvenDragon == true)
            {
                TouchOrigin = myTouch.position;
                //TapCount++;
                //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - .1f);
                //return;
            }
            else if(myTouch.phase == TouchPhase.Began && myTouch.tapCount % 2 == 1 && EvenDragon == false)
            {
                TouchOrigin = myTouch.position;
                //TapCount++;
                //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + .1f);
            }
            else if (myTouch.phase == TouchPhase.Ended && TouchOrigin.x >= 0)
            {
                //Vector2 TouchEnd = myTouch.position;
                //end turn or ask for confirmation?
            }
        }
    }

    public void RunDragonHor(float horizonalInput) //left is negative input
    {
        //rb.AddForce(new Vector3(horizonalInput * Speed * Time.deltaTime, 0, 0));
        transform.position = new Vector3(transform.position.x + (horizonalInput * Speed * Time.deltaTime), transform.position.y, transform.position.z);
    }

    public void RunDragonVer(float verticalInput)
    {
        //rb.AddForce(new Vector3( 0, verticalInput * Speed * Time.deltaTime, 0));
        transform.position = new Vector3(transform.position.x, transform.position.y + (verticalInput * Speed * Time.deltaTime), transform.position.z);
    }

    public void RunDragonDepth(float depthInput)
    {
        //rb.AddForce(new Vector3( 0, 0, depthInput * Speed * Time.deltaTime));
        transform.position = new Vector3(transform.position.x, transform.position.y, (transform.position.z + depthInput * Speed * Time.deltaTime));
    }
}
