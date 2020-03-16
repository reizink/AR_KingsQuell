using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "ElfPlayer" || other.gameObject.tag == "WizardPlayer")
        {
            Destroy(other.gameObject);
        }
    }
}
