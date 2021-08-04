using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(PredictionManager))]
[RequireComponent(typeof(BoxCollider))]
public class Plane : MonoBehaviour
{
    public PlaneObject planeObject;

    private float        planeSpeed, strafeSpeed, hoverSpeed;
    private float       activePlaneSpeed, activeStrafeSpeed, activeHoverSpeed;
    private float       forwardAcceleration = 2.5f, strafeAcceleration = 2f, hoverAcceleration = 2f;
    private float        lookRotateSpeed;
    private float       rollInput;
    private float        rollSpeed = 90f, rollAcceleartion = 3.5f;
    public float        ballPower;
    public float          maxBallPower;

    private Vector2     lookInput, screenCenter,mouseDistance;

    Rigidbody           rigid;
    [HideInInspector]
    public bool         hasBall;
    [HideInInspector]
    public Transform    ballHolderTransform;
    public float          maxBallAccel;
    public GameObject   ball, marker, markerPrefab;
    PredictionManager   prediction;
    private Vector3 initialPos;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        //Set up miscellaneous scene objects
        screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        ballHolderTransform = transform.GetChild(0);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ball = GameObject.FindGameObjectWithTag("ball");

        //Setup Rigidbody
        SetRigidBody(RigidbodyConstraints.FreezeRotation,false);

        //Setup PredictionManager
        LayerMask layerMask = ~(1 << LayerMask.NameToLayer("Ball") | 1 << LayerMask.NameToLayer("Plane"));
        Material lineMat = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        SetPredictionManager(lineMat,layerMask,100,0.05f);

        //Get data from plane scriptable object
        GetSetPlaneObjectData();

    }
    public void GetSetPlaneObjectData()
    {
        planeSpeed = planeObject.planeSpeed;
        strafeSpeed = planeObject.strafeSpeed;
        hoverSpeed = planeObject.hoverSpeed;
        rollSpeed = planeObject.rollSpeed;
        rollAcceleartion = planeObject.rollAcceleartion;
        lookRotateSpeed = planeObject.lookRotateSpeed;
        markerPrefab = planeObject.marker;
    }
    public void SetRigidBody(RigidbodyConstraints constraint, bool gravity)
    {
        initialPos = transform.position;
        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = gravity;
        rigid.constraints = constraint;
    }
    /// <summary>
    /// Variables in order: Material for the line Render line, layers to avoid collision detection, number of points prediction manager will use to estimate, time between each predicition estimation
    /// </summary>
    public void SetPredictionManager(Material lineRenderMaterial,LayerMask avoidLayers, int numberPoints, float timeBetweenPoints)
    {
        prediction = GetComponent<PredictionManager>();
        prediction.lineRenderer = GetComponent<LineRenderer>();
        prediction.lineRenderer.material =lineRenderMaterial;
        prediction.lineRenderer.startColor = Color.red;
        prediction.lineRenderer.endColor = Color.red;
        prediction.lineRenderer.useWorldSpace = true;
        prediction.CollidableLayers = avoidLayers;
        prediction.numPoints = numberPoints;
        prediction.timeBetweenPoints = timeBetweenPoints;
    }
    public void setMaxPowerAndAccel(float power,float accel)
    {
        this.maxBallPower = power;
        this.maxBallAccel = accel;
    }

    public void acquireBall()
    {
        ball.transform.SetParent(ballHolderTransform);
        ball.transform.localPosition = Vector3.zero;
        ball.GetComponent<Rigidbody>().useGravity = false;
        ball.GetComponent<ball>().owner = this.gameObject;
        hasBall = true;
    }
    public void fly()
    {
        lookInput = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);
        rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Horizontal"), rollAcceleartion * Time.deltaTime);
        transform.Rotate(-mouseDistance.y * lookRotateSpeed * Time.deltaTime, mouseDistance.x * lookRotateSpeed * Time.deltaTime, -rollInput * rollSpeed * Time.deltaTime, Space.Self);

        activePlaneSpeed = Mathf.Lerp(activePlaneSpeed, Input.GetAxisRaw("Vertical") * planeSpeed, forwardAcceleration * Time.deltaTime);
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, Input.GetAxisRaw("Strafe") * strafeSpeed, strafeAcceleration * Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, Input.GetAxisRaw("Hover") * hoverSpeed, hoverAcceleration * Time.deltaTime);
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            transform.position += transform.forward * activePlaneSpeed * Time.deltaTime;
            
        }
        else
        {
            transform.position += transform.forward * (planeSpeed/3) * Time.deltaTime;
            transform.position += -transform.up * (planeSpeed / 10) * Time.deltaTime;
        }
        
        transform.position += transform.right * activeStrafeSpeed * Time.deltaTime;
        transform.position += transform.up * activeHoverSpeed * Time.deltaTime;
    }

    public void shoot(float power)
    {
        ball.transform.SetParent(null);       
        ball.GetComponent<Rigidbody>().velocity = rigid.velocity + (ballHolderTransform.transform.up * power);
        ball.GetComponent<Rigidbody>().useGravity = true;
        hasBall = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ball")
        {
            if (collision.gameObject.GetComponent<ball>().owner != this.gameObject)
            {
                acquireBall();
            }
            else
            {
                Physics.IgnoreCollision(this.gameObject.GetComponent<BoxCollider>(),collision.collider);   
            }
            
        }
    }
    public void DestroyMarker()
    {
        if (marker != null)
        {
            Destroy(marker);
        }
    }
    public void Respawn()
    {
        ball.transform.SetParent(null);
        DestroyMarker();
        gameManager.Respawn(this.gameObject, initialPos);
    }
    void Update()
    {
        fly();
        
        if (hasBall)
        {
            if (marker)
                marker.transform.position = prediction.endPoint;
            if (Input.GetMouseButtonDown(0))
            {
                marker = Instantiate(markerPrefab);
            }
            if (Input.GetMouseButton(0))
            {
                //if marker exists check
                
                if (ballPower < maxBallPower)
                    ballPower += Time.deltaTime * maxBallAccel;
                else
                {
                    Respawn();
                }
                prediction.predict = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                DestroyMarker();
                shoot(ballPower);
                prediction.predict = false;
            }
        }
        else
        {
            ballPower = 0;
        }

    }
}
