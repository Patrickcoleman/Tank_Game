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

    private Rigidbody2D _tankrigidbody;
    private float movespeed = 1;
    private float turnspeed = 70;
    private Plane _background = new(Vector3.back, Vector3.zero);
    private Camera _cam;

    private Vector3 mouse_pos;
    private Vector3 object_pos;
    private float angle;
    private bool lastDirFwd = true;

    private void Awake() {
        _cam = Camera.main;
        _tankrigidbody = GetComponent<Rigidbody2D>();
    }

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

    private bool AmIMovingForward(){
        bool xsame = _tankrigidbody.velocity.x * gameObject.transform.up.x > 0 ? true : false;
        bool ysame = _tankrigidbody.velocity.y * gameObject.transform.up.y > 0 ? true : false;
        if (xsame & ysame){
            lastDirFwd = true;
        }else if (!xsame & !ysame){
            lastDirFwd = false;
        }
        return lastDirFwd;
    }

    void Update()
    {
        // Don't change anything for other tanks
        if(!IsOwner) return;

        //Update gun direction toward mouse position
        mouse_pos = Input.mousePosition;
        object_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.x = object_pos.x - mouse_pos.x;
        mouse_pos.y = object_pos.y - mouse_pos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg - 90;
        Gun_Skin.transform.rotation = Quaternion.RotateTowards(Gun_Skin.transform.rotation, Quaternion.Euler(0, 0, angle), 3);;

        //On click, shoot a bullet
        if (Input.GetMouseButtonDown(0)){
            var dir = Gun_Skin.transform.up;
            RequestShootServerRpc(dir);
            Shoot(dir);
        }

        if(Input.GetKey(KeyCode.UpArrow) | Input.GetKey(KeyCode.W)){
            _tankrigidbody.AddForce(transform.up * 10f);
            //currentSpeed += movespeed * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.DownArrow) | Input.GetKey(KeyCode.S)){
            _tankrigidbody.AddForce(-transform.up * 10f);
            //currentSpeed -= movespeed * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.LeftArrow) | Input.GetKey(KeyCode.A)){
            Vector3 velocity = _tankrigidbody.velocity;
            transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * turnspeed, Space.World);
            _tankrigidbody.velocity = transform.up * velocity.magnitude * (AmIMovingForward() ? 1 : -1);
        }

        if(Input.GetKey(KeyCode.RightArrow) | Input.GetKey(KeyCode.D)){
            Vector3 velocity = _tankrigidbody.velocity;
            transform.Rotate(new Vector3(0, 0, -1) * Time.deltaTime * turnspeed, Space.World);
            _tankrigidbody.velocity = transform.up * velocity.magnitude * (AmIMovingForward() ? 1 : -1);
        }
    }
}
