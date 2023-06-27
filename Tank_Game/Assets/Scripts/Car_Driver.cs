using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Car_Driver : NetworkBehaviour
{
    public GameObject Tank_Skin;
    public GameObject Gun_Skin;
    public float currentSpeed = 0;
    private Plane _background = new(Vector3.back, Vector3.zero);
    private Camera _cam;

    private Vector3 mouse_pos;
    private Vector3 object_pos;
    private float angle;

    private void Awake() {
        _cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    public override void OnNetworkSpawn(){
    }

    // Update is called once per frame
    void Update()
    {
        // Don't change anything for other tanks
        if(!IsOwner) return;


        //Update gun direction
        mouse_pos = Input.mousePosition;
            // mouse_pos.z = -20;
            //Debug.Log(Input.mousePosition);
            object_pos = Camera.main.WorldToScreenPoint(transform.position);
            mouse_pos.x = object_pos.x - mouse_pos.x;
            mouse_pos.y = object_pos.y - mouse_pos.y;
            angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg - 90;
            Gun_Skin.transform.rotation = Quaternion.Euler(0, 0, angle);


        // // Update rotation of Gun
        // var ray = _cam.ScreenPointToRay(Input.mousePosition);
        // if (_background.Raycast(ray, out var enter)) {
        //     var hitPoint = ray.GetPoint(enter);

        //     var dir = hitPoint - transform.position;
        //     var rot = Quaternion.LookRotation(dir);

        //     Gun_Skin.transform.rotation = Quaternion.RotateTowards(Gun_Skin.transform.rotation, rot, 450 * Time.deltaTime);
        // }

        // Debug.Log(Input.mousePosition);
        //
        //currentSpeed = Mathf.Round((currentSpeed * 50000 * Time.deltaTime) * 0.95f)/100;
        currentSpeed = 20;
        var rotationVector = Tank_Skin.transform.rotation.eulerAngles;
        this.transform.position = this.transform.position + new Vector3(Mathf.Sin(rotationVector.z * Mathf.PI/180) * Time.deltaTime * currentSpeed * 0.05f,-Mathf.Cos(rotationVector.z * Mathf.PI/180) * Time.deltaTime * currentSpeed * 0.05f,0);
        
        if(Input.GetKey(KeyCode.UpArrow)){
            currentSpeed += 10;
        }

        if(Input.GetKey(KeyCode.DownArrow)){
            currentSpeed -= 10;
        }

        if(Input.GetKey(KeyCode.LeftArrow)){
            rotationVector.z += Time.deltaTime * 100;
            Tank_Skin.transform.rotation = Quaternion.Euler(rotationVector);
        }

        if(Input.GetKey(KeyCode.RightArrow)){
            rotationVector.z -= Time.deltaTime * 100;
            Tank_Skin.transform.rotation = Quaternion.Euler(rotationVector);
        }
    }
}
