using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DefaultGoal",menuName = "Goal")]
public class GoalObject : ScriptableObject
{
    public new string name;
    public GameObject prefab;
    public enum motionTypes // your custom enumeration
    {
        Default,
        Crazy1,
        Average
    };
    public motionTypes motion;
    

}
