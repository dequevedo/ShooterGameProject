using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour {
    public Transform target;
    public int smoothTime;
    public float horizontalDistance;
    public float verticalTargetPos;

    public float delayExample;

    void Update()
    {
        if (delayExample <= 0) {
            delayExample = 2;
        } else {
            delayExample -= Time.deltaTime;
        }

        //verify and fix the vertical position of camera
        if(target.position.y > 7.95F - (horizontalDistance - 5))
        {
            verticalTargetPos = 7.95F - (horizontalDistance - 5);
        }
        else if(target.position.y < horizontalDistance + .1F)
        {
            verticalTargetPos = horizontalDistance + .1F;
        }
        else
        {
            verticalTargetPos = target.position.y;
        }

        //transform.LookAt(target);
        Vector3 standingPosition = new Vector3(transform.position.x, verticalTargetPos, target.position.z - horizontalDistance);
        transform.position = Vector3.Lerp(transform.position, standingPosition, Time.deltaTime * smoothTime);
    }
}
