using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.18f;
    public float jumpHeight = 3f;

    Vector3 velocity;
    
    public Image crosshair;
    private RaycastHit hit;
    private EnemyHandler enemy;
    public Camera fpsCam;

    void Start() {
        speed = 12f;
    }
    void Update()
    {
        
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, 30f)){
            enemy = hit.transform.GetComponent<EnemyHandler>();   
        }
        else {
            enemy = null;
        }
        if(enemy != null) crosshair.GetComponent<Image>().color = Color.green;
        else crosshair.GetComponent<Image>().color = Color.red;
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}