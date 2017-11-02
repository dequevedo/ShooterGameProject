using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    private static FieldOfView fieldOfViewInstance;
    public static FieldOfView FieldOfViewInstance { get { return fieldOfViewInstance; } }

    [Header("FOV Properties")]
    public float radius;
    [Range(0f, 360f)]
    public float viewAngle;

    [Header("FOV Layer Masks")]
    public LayerMask targetLayer;
    public LayerMask obstaclesLayer;
    private List<Transform> visibleTargets = new List<Transform>();
    private Transform closestTarget;
    private bool isClosestTargetInSight = false;
    private bool isTargetLockedOn = false;

    public List<Transform> VisibleTargets
    {
        get { return visibleTargets; }
    }
    public Transform ClosestTarget
    {
        get { return closestTarget; }
    }
    public bool IsClosestTargetInSight
    {
        get { return isClosestTargetInSight; }
    }
    public bool IsTargetLockedOn
    {
        get { return isTargetLockedOn; }
        set { isTargetLockedOn = value; }
    }

    private void Awake()
    {
        fieldOfViewInstance = this;
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine("FindTargets", 0.2f);
    }

    void FindAllVisibleTargets()
    {
        //Debug.Log("Searching for visible targets!");
        visibleTargets.Clear(); // Reset the list of VisibleTargets to prevent miscounting
        Collider2D[] allTargetsInRange = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer); // Array of all objects that overlap the FOV radius
        for (int i = 0; i < allTargetsInRange.Length; i++)
        {
            Transform currentTarget = allTargetsInRange[i].transform;
            Vector3 targetDirection = (currentTarget.position - transform.position).normalized; // Vector that points from the player to the target
            float angleToTarget = Vector3.Angle(transform.right, targetDirection);
            // Checks if the Target is within the FOV positive/negative angle
            if (angleToTarget <= (viewAngle / 2))
            {
                float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
                // Checks if the Ray does not collider with any obstacle defined by the ObstacleLayerMask
                if (!Physics2D.Raycast(transform.position, targetDirection, distanceToTarget, obstaclesLayer))
                {
                    visibleTargets.Add(currentTarget);
                    // If there isn't a target locked on, it's allowed to change the ClosestTarget
                    if (!isTargetLockedOn)
                    {
                        // Checks if there is a ClosestTarget or if the current Target`s distance is less than the prev. ClosestTarget`s distance
                        if (!closestTarget || distanceToTarget <= Vector3.Distance(transform.position, closestTarget.position))
                        {
                            isClosestTargetInSight = true;
                            closestTarget = currentTarget;
                        }
                        else
                        {
                            if (!isClosestTargetInSight)
                            {
                                // Lambda expression to sort the List of VisibleTargets by the shoter distance to the player position
                                visibleTargets.Sort((t1, t2) => Vector3.Distance(transform.position, t1.position).ToString().CompareTo(
                                                                Vector3.Distance(transform.position, t2.position).ToString()));
                                closestTarget = visibleTargets[0];  // Change the closestTarget to the first Target of the sorted list (shorter distance)
                                isClosestTargetInSight = true;
                            }
                        }
                    }
                }
            }
            if (closestTarget)
            {
                if (Vector3.Angle(transform.right, (closestTarget.position - transform.position).normalized) > (viewAngle / 2))
                    isClosestTargetInSight = false;
            }
            // Sets the ClosestTarget to NULL if the List of VisibleTargets is NULL
            if (visibleTargets.Count <= 0 || !closestTarget.gameObject.activeInHierarchy)
            {
                closestTarget = null;
                isTargetLockedOn = false;
            }
        }
    }

    IEnumerator FindTargets(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindAllVisibleTargets();
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.z;
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0f);
    }
}
