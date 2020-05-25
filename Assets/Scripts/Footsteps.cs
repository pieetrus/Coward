
using System.Collections;

using UnityEngine;

public class Footsteps : MonoBehaviour   
{
    CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cc.isGrounded == true && cc.velocity.magnitude> 2 && GetComponent<AudioSource>().isPlaying == false)
        {
            GetComponent<AudioSource>().volume = Random.Range(0.8f, 1);
            GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.1f);
            GetComponent<AudioSource>().Play();
        }
    }
}
