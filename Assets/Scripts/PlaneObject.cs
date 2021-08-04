using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " New Plane", menuName = "Plane")]
public class PlaneObject : ScriptableObject
{
    [Tooltip("Visual prefab of plane")]
    public GameObject prefab;
    public new string name;
    [Tooltip("Projectile prediction marker")]
    public GameObject marker;
    [Tooltip("Basic plane speed variables")]
    public float planeSpeed, strafeSpeed, hoverSpeed;
    [Tooltip("Basic plane acceleration variables")]
    public float forwardAcceleration, strafeAcceleration, hoverAcceleration;
    [Tooltip("Rotation speed")]
    public float lookRotateSpeed;
    [Tooltip("Barrel roll variable")]
    public float rollSpeed, rollAcceleartion;
    // Start is called before the first frame update

}
