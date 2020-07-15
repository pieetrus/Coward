using UnityEngine;

public class CoinRotation : MonoBehaviour
{

    public bool goUp;
    public AudioSource myAudio;
    public AudioClip coinCollection;
    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (goUp == true)
        {
            transform.Rotate(0, 0, 0);
            transform.Translate(0, 0.8f, 0);
        }
        else
        {
            transform.Rotate(0, 2f, 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            goUp = true;
            myAudio.PlayOneShot(coinCollection, 1);
           // Destroy(gameObject);
        }
    }
}
