using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ball : MonoBehaviour
{
    public BallObject ballObject;
    public GameObject owner;
    GameManager gameManager;
    [HideInInspector]
    public int maxPower;
    [HideInInspector]
    public int ballAccel;
    SphereCollider col;
    public ball(BallObject ballObject)
    {
        this.ballObject = ballObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        maxPower = ballObject.maxBallPower;
        ballAccel = ballObject.ballPowerAccel;
        owner = this.gameObject;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        col = GetComponent<SphereCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (owner != null)
        {
            if (collision.gameObject != owner)
            {
                owner = null; 
            }
        }
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "goal")
        {
            gameManager.AddScore(1);
            Destroy(this.gameObject);
            gameManager.InstantiateBall(new Vector3(11, 150, 180));
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            col.isTrigger = true;
            transform.position = transform.parent.position;
        }
        else
        {
            col.isTrigger = false;
            
        }
    }
}
