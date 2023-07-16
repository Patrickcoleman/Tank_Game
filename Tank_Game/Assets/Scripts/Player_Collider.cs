using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Collider : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        Debug.Log("HIT");
        if (other.tag == "Player"){
            Debug.Log("ENTER");
        }
    }

    void OnTriggerStay(Collider other){
        if (other.tag == "Player"){
            Debug.Log("STAY");
        }
    }

    void OnTriggerExit(Collider other){
        if (other.tag == "Player"){
            Debug.Log("EXIT");
        }
    }
}
