using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " New Ball", menuName = "Ball")]
public class BallObject : ScriptableObject
{
    public new string name;
    public string description;
    public GameObject prefab;
    public int maxBallPower;
    public int ballPowerAccel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
