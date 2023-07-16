using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Collider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Missile" && other.gameObject.GetComponent<Bullet_Mover>()._creator != this.gameObject){
            Debug.Log("ENTER");
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if (other.tag == "Missile" && other.gameObject.GetComponent<Bullet_Mover>()._creator != this.gameObject){
            Debug.Log("STAY");
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.tag == "Missile" && other.gameObject.GetComponent<Bullet_Mover>()._creator != this.gameObject){
            Debug.Log("EXIT");
        }
    }
}
