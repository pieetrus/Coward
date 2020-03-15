using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script made camera follow the player
/// And add some fancy animation on start of the game
/// </summary>
public class CameraMotor : MonoBehaviour
{
    [SerializeField]
    private Transform target = null; // position of object we want to follow

    private Vector3 cameraOffset;

    [SerializeField]
    [Range(0.01f, 1.0f)]
    private static float smoothFactor = 0.5f;

    private float transition = 0.0f;
    private static float animationDuration = 3.0f; //IF YOU CHANGE THAT VALUE CHANGE IT ALSO IN PLAYERMOTOR SCRIPT

    private Vector3 animationOffset = new Vector3(0, 5, 5);



    void Start()
    {
        cameraOffset = transform.position - target.position;
    }

    /// <summary>
    /// Its better to use LateUpdate instead of update, because sometimes we change position in Update method
    /// and Camera fight with other part of code who go first, this is why its better to make cammera stuff after those things
    /// using LateUpdate which will execute after Update method
    /// </summary>
    void FixedUpdate()
    {
        if(target == null)
        {
            Debug.LogWarning("Missing target ref !");

            return;
        }

        Vector3 desiredPosition = target.position + cameraOffset;


        //X
        desiredPosition.x = 0;

        //Y
        desiredPosition.y = Mathf.Clamp(desiredPosition.y,3,5); //in y axis camera can only move in range (3,5)

        if(transition > 1.0f)
        {
            transform.position = Vector3.Slerp(transform.position, desiredPosition, smoothFactor);
        }
        else
        {
            //Animation at the start of the game
            transform.position = Vector3.Lerp(desiredPosition + animationOffset, desiredPosition, transition);
            transition += Time.fixedDeltaTime * 1 / animationDuration;

            transform.LookAt(target.position + Vector3.up);
        }



    }
}
