using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private float LANE_DISTANCE = 3.0f;

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
    private int desiredLane = 1; //0 = Left, 1 = Middle, 2 = Right

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
        //moveVector.x = Input.GetAxisRaw("Horizontal") * speedHorizonal;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            MoveLane(false);
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            MoveLane(true);

        //Calculate where we should be in future
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * LANE_DISTANCE;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * LANE_DISTANCE;
        }

        //Lets calculate our move delta
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;

        //Y - Up and Down
        moveVector.y = verticalVelocity; 

        //Z - Forward and Backward
        moveVector.z = speed;

        controller.Move(moveVector * Time.deltaTime);

    }

    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
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
