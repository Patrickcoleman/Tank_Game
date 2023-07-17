using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Camera_Mover : NetworkBehaviour
{
    private Camera _cam;
    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;
        Vector3 newPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);
        _cam.transform.position = newPos;
    }
}
