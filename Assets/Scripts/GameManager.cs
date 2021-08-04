using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text text;
    public BallObject ballObject;
    public PlaneObject planeObject;
    public GoalObject goalObject;
    public int blueScore;
    public int redScore;
    public List<GameObject> planes;
    // Start is called before the first frame update

    void Awake()
    {
        blueScore = 0;
        redScore = 0;
        InstantiatePlane(new Vector3(0, 109, 0));
        InstantiateBall(new Vector3(11,150,180));
        
    }
    public void InstantiatePlane(Vector3 spawnpoint)
    {
        GameObject newPlane = Instantiate(planeObject.prefab, spawnpoint, Quaternion.identity);
        Plane np = newPlane.AddComponent<Plane>();
        np.planeObject = planeObject;
        np.setMaxPowerAndAccel(ballObject.maxBallPower, ballObject.ballPowerAccel);
        planes.Add(newPlane);
    }
   
    public void InstantiateBall(Vector3 spawnPoint)
    {
        GameObject gameBall = Instantiate(ballObject.prefab, spawnPoint, Quaternion.identity);
        ball gb = gameBall.AddComponent<ball>();
        gameBall.GetComponent<Rigidbody>().useGravity = false;
        gb.ballObject = ballObject;
        foreach (GameObject plane in planes)
        {
            Plane p = plane.GetComponent<Plane>();
            p.ball = gameBall;
        }
    }
    public void InstantiateGoal(Vector3 spawnPoint)
    {

    }

    public void Respawn(GameObject prevPlane, Vector3 spawn)
    {
        Destroy(prevPlane);
        planes.Remove(prevPlane);
        InstantiatePlane(spawn);
    }
    public void AddScore(int x)
    {
        blueScore += x;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "SCORE: " + blueScore;
    }
}
