using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashedPlayers : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ElfPlayer" || other.gameObject.tag == "WizardPlayer")
            Destroy(other);
    }
}