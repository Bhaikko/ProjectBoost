using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using ProjectBoost.AI;

[CustomEditor (typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI() {
        FieldOfView fow = (FieldOfView)target;

        Handles.color = Color.white;
        Handles.DrawWireArc(
            fow.transform.position,
            -Vector3.forward,
            Vector3.up,
            360.0f,
            fow.GetViewRadius()
        );

        Vector3 viewAngleA = fow.DirectionFromAngle(-fow.GetViewAngle() / 2, false);
        Vector3 viewAngleB = fow.DirectionFromAngle(fow.GetViewAngle() / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.GetViewRadius());
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.GetViewRadius());
    }
}

