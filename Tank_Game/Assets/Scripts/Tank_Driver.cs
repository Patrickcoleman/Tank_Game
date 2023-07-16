using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Tank_Driver : NetworkBehaviour
{
    [SerializeField] private GameObject Tank_Skin;
    [SerializeField] private GameObject Gun_Skin;
    [SerializeField] private GameObject BarrelTip;
    [SerializeField] private GameObject Bullet;
    public float currentSpeed = 0;
    private float movespeed = 1;
    private float turnspeed = 70;
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

    private void Shoot(Vector3 vector) {
        GameObject newBullet = Instantiate(Bullet,BarrelTip.transform.position,Quaternion.identity);
        newBullet.GetComponent<Bullet_Mover>().Init(vector,this.gameObject);
    }

    [ServerRpc]
    private void RequestShootServerRpc(Vector3 dir) {
        FireClientRpc(dir);
    }

    [ClientRpc]
    private void FireClientRpc(Vector3 dir) {
        if (!IsOwner){
            Shoot(dir);
        }
    }


    void Update()
    {
        // Don't change anything for other tanks
        if(!IsOwner) return;

        

        //Update gun direction
        mouse_pos = Input.mousePosition;
        object_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.x = object_pos.x - mouse_pos.x;
        mouse_pos.y = object_pos.y - mouse_pos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg - 90;
        Gun_Skin.transform.rotation = Quaternion.RotateTowards(Gun_Skin.transform.rotation, Quaternion.Euler(0, 0, angle), 3);;
        //Debug.Log(angle);
        
        


        if (Input.GetMouseButtonDown(0)){
            var dir = Gun_Skin.transform.up;
            RequestShootServerRpc(dir);
            Shoot(dir);
        }



        // slows down current speed by a factor of 0.4 every second
        currentSpeed -= currentSpeed * Time.deltaTime * 0.4f;

        //Moves the tank forward in the direction it is facing by its current speed
        var rotationVector = Tank_Skin.transform.rotation.eulerAngles;
        this.transform.position = this.transform.position + new Vector3(Mathf.Sin(rotationVector.z * Mathf.PI/180) * Time.deltaTime * currentSpeed,-Mathf.Cos(rotationVector.z * Mathf.PI/180) * Time.deltaTime * currentSpeed,0);
        //Debug.Log(rotationVector);

        if(Input.GetKey(KeyCode.UpArrow) | Input.GetKey(KeyCode.W)){
            currentSpeed += movespeed * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.DownArrow) | Input.GetKey(KeyCode.S)){
            currentSpeed -= movespeed * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.LeftArrow) | Input.GetKey(KeyCode.A)){
            rotationVector.z += Time.deltaTime * turnspeed;
            Tank_Skin.transform.rotation = Quaternion.Euler(rotationVector);
        }

        if(Input.GetKey(KeyCode.RightArrow) | Input.GetKey(KeyCode.D)){
            rotationVector.z -= Time.deltaTime * turnspeed;
            Tank_Skin.transform.rotation = Quaternion.Euler(rotationVector);
        }
    }
}
