using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;  // Reference to the FieldOfView Script

        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.right, 360, fow.radius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);
        // Draw the lines that clamp the angle of the FOV
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.radius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.radius);

        Handles.color = Color.blue;
        // Draw a cube around the selected closestTarget
        if (fow.ClosestTarget)
            Handles.DrawWireCube(fow.ClosestTarget.position, Vector3.one);

        Handles.color = Color.red;
        // Draw a line in the direction of the enemy in the view
        foreach (Transform visibleTarget in fow.VisibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }
    }

}
