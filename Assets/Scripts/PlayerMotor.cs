using TMPro;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private static float LANE_DISTANCE = 3.0f;

    [SerializeField]
    private float TURN_SPEED = 0.05f;

    [SerializeField]
    private TextMeshProUGUI caneText = null;

    private CharacterController controller;
    [SerializeField]
    private GameObject scoreContainer;
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
    private int canes = 0;

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
        IsGrounded();

        if (isDead)
            return;

        Movement();

        ControllAnimation();

    }

    private void MoveLane(bool goingRight)
    {
        if (goingRight == true)
        {
            desiredLane += 1;
        }
        else
        {
            desiredLane += -1;
        }
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
        scoreContainer.active = true;

        moveVector = Vector3.zero; //reset value

        //Y - Jumping
        if (IsGrounded())
        {
            verticalVelocity = -0.1f;
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                FindObjectOfType<AudioManager>().Play("PlayerJump");
                verticalVelocity = jumpPower;
                playerAnim.SetBool("jumping", true);
                
            }
        }
        else
        {
            

            playerAnim.SetBool("jumping", false);

            verticalVelocity -= (gravity * Time.deltaTime);

            //Fast Falling mechanic
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                verticalVelocity = -jumpPower / 2;
            }
        }

        //X - Left and Right
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            MoveLane(false);
            FindObjectOfType<AudioManager>().Play("PlayerTurn");
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            MoveLane(true);
            FindObjectOfType<AudioManager>().Play("PlayerTurn");
        }

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

        // Rotate player to where he is going
        Vector3 direction = controller.velocity;
        direction.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, direction, TURN_SPEED);



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
        FindObjectOfType<AudioManager>().Play("PlayerDeath");
        isDead = true;
        GetComponent<Score>().OnDeath();
        OnDeathAnimation();
    }

    private void ControllAnimation()
    {
        playerAnim.SetBool("grounded", IsGrounded());

        //playerAnim.SetBool("walking", true);
        //if (verticalVelocity > 1)
        //{
        //    playerAnim.SetBool("jumping", true);
        //    playerAnim.SetBool("walking", false);

        //}
        //else if (verticalVelocity <= 0)
        //{
        //    playerAnim.SetBool("jumping", false);
        //    playerAnim.SetBool("walking", true);
        //}
    }

    private void OnDeathAnimation()
    {
        //playerAnim.SetBool("jumping", false);
        playerAnim.SetBool("isDead", true);
    }




    private bool IsGrounded()
    {
        Ray groundRay = new Ray(
            new Vector3(
                controller.bounds.center.x,
                (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f,
                controller.bounds.center.z),
            Vector3.down);

        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan, 20.0f);

        return (Physics.Raycast(groundRay, 0.2f + 0.1f));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject, 0.5f);
            canes++;
            caneText.text = ((int)canes).ToString();
        }
    }


}
