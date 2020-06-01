using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowMotor : MonoBehaviour
{


    [SerializeField]
    private Transform target = null; // position of object we want to follow

    private Vector3 cowOffset;

    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float smoothFactor = 0.5f;

    private float transition = 0.0f;
    private static float animationDuration = 3.0f; //IF YOU CHANGE THAT VALUE CHANGE IT ALSO IN PLAYERMOTOR SCRIPT

    private Vector3 animationOffset = new Vector3(0, 1, 1);
    // Start is called before the first frame update
    void Start()
    {
        cowOffset = transform.position - target.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Missing target ref !");

            return;
        }

        Vector3 desiredPosition = target.position + cowOffset;


        //X
        desiredPosition.x = 0;

        //Y
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, 1, 5); //in y axis camera can only move in range (3,5)

        
        
            transform.position = Vector3.Slerp(transform.position, desiredPosition, smoothFactor);
            //transition += Time.fixedDeltaTime * 1 / animationDuration;

            //transform.LookAt(target.position + Vector3.up);
        

    }
}
