using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    private CharacterController controller;
    private Vector3 moveVector;

    [SerializeField]
    private float speed = 10.0f;
    [SerializeField]
    private float speedHorizonal = 5.0f;
    [SerializeField]
    private float jumpPower = 7.0f;
    private float verticalVelocity = 0.0f;
    private float gravity = 12.0f;
    private float animationDuration = 3.0f; //COPY THIS VALUE FROM CAMERA MOTOR SCRIPT

    private bool isDead = false;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (isDead)
            return;

        //start animation
        if(Time.time < animationDuration)
        {
            controller.Move(Vector3.forward * speed * Time.deltaTime);
            return;
        }

        moveVector = Vector3.zero; //reset value

        if(controller.isGrounded)
        {
            verticalVelocity = Input.GetAxisRaw("Jump") * jumpPower;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        //X - Left and Right
        moveVector.x = Input.GetAxisRaw("Horizontal") * speedHorizonal;

        //Y - Up and Down
        moveVector.y = verticalVelocity; 


        //Z - Forward amd Backward
        moveVector.z = speed;

        controller.Move(moveVector * Time.deltaTime);
    }

    /// <summary>
    /// Used in Score Script
    /// </summary>
    /// <param name="modifier"></param>
    public void SetSpeed(int modifier)
    {
        speed = 5 + modifier;
    }

    public void OnCollisionEnter(Collision collision)
    {
        //When hit obcject with tag Obstacle then die
        if (collision.collider.CompareTag("Obstacle"))
        {
            Death();
        }
    }


    private void Death()
    {
        isDead = true;
        GetComponent<Score>().OnDeath();
    }

}
