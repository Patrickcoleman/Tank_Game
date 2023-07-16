using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Mover : MonoBehaviour
{
    public Vector3 dir;
    public GameObject _creator;
    private float movespeed = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(Vector3 InputDir, GameObject creator) {
        dir = InputDir;
        _creator = creator;
    }

    // Update is called once per frame
    void Update()
    {   
        this.transform.position = this.transform.position - dir * Time.deltaTime * movespeed;
        // this.transform.position = this.transform.position + new Vector3(Mathf.Sin(dir.z * Mathf.PI/180) * Time.deltaTime * movespeed,-Mathf.Cos(dir.z * Mathf.PI/180) * Time.deltaTime * movespeed,0);

    }
}
