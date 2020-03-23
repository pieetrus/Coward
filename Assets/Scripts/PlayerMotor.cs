using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private static float LANE_DISTANCE = 3.0f;

    private CharacterController controller;
    [SerializeField]
    private Animator playerAnim;
    
    private Vector3 moveVector;
    [SerializeField]
    public float startSpeed = 10.0f;
    private float speed;
    [SerializeField]
    private float jumpPower = 7.0f;
    [SerializeField]
    private float verticalVelocity = 0.0f;
    private static float gravity = 12.0f;
    private static float animationDuration = 3.0f; //COPY THIS VALUE FROM CAMERA MOTOR SCRIPT
    private float startTime;
    private int desiredLane = 1; //0 = Left, 1 = Middle, 2 = Right

    private bool isDead = false;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerAnim = GetComponent<Animator>();

        speed = startSpeed;
        startTime = Time.time;
    }

    void Update()
    {
        if (isDead)
            return;

        Movement();
        
        //ControllAnimation();

    }

    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    private void Movement()
    {
        //start animation
        if (Time.time - startTime < animationDuration)
        {
            controller.Move(Vector3.forward * speed * Time.deltaTime);
            return;
        }

        moveVector = Vector3.zero; //reset value

        //Y - Jumping
        if (controller.isGrounded)
        {
            verticalVelocity = Input.GetAxisRaw("Jump") * jumpPower;
        }
        else
            verticalVelocity -= gravity * Time.deltaTime;

        //X - Left and Right
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            MoveLane(false);
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            MoveLane(true);

        //Calculate where we should be in future
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (desiredLane == 0)
            targetPosition += Vector3.left * LANE_DISTANCE;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * LANE_DISTANCE;

        //Lets calculate our move delta
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;

        //Y - Up and Down
        moveVector.y = verticalVelocity;

        //Z - Forward and Backward
        moveVector.z = speed;

        controller.Move(moveVector * Time.deltaTime);
    }


    /// <summary>
    /// Used in Score Script
    /// </summary>
    /// <param name="modifier"></param>
    public void SetSpeed(int modifier)
    {
        speed = startSpeed + modifier;
    }

    public void OnCollisionEnter(Collision collision)
    {
        //When hit obcject with tag Obstacle then die
        if (collision.collider.CompareTag("Obstacle"))
            Death();
    }


    private void Death()
    {
        isDead = true;
        GetComponent<Score>().OnDeath();
        //OnDeathAnimation();
    }

    private void ControllAnimation()
    {
        playerAnim.SetBool("walking", true);
        if (verticalVelocity > 1)
        {
            playerAnim.SetBool("jumping", true);
            playerAnim.SetBool("walking", false);

        }
        else if (verticalVelocity <= 0)
        {
            playerAnim.SetBool("jumping", false);
            playerAnim.SetBool("walking", true);
        }
    }

    private void OnDeathAnimation()
    {
        playerAnim.SetBool("walking", false);
        playerAnim.SetBool("jumping", false);
        playerAnim.SetBool("isDead", true);
    }

}
