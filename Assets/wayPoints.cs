using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to Waypoints gameobject under Waypoint gameobject
public class wayPoints : MonoBehaviour
{
    public GameObject[] waypoints;
    void OnDrawGizmos()
    {
        DrawGizmo(false);
        //Debug.Log("DrawGizmo(false)");
    }

    private void OnDrawGizmosSelected()
    {
        DrawGizmo(true);
        //Debug.Log("DrawGizmo(true)");
    }

    void DrawGizmo(bool selected)
    {
        if (selected == false) return;

        if(waypoints.Length > 1)
        {
            Vector3 previous = waypoints[0].transform.position;
            for (int i = 1; i < waypoints.Length; i++)
            {
                Vector3 next = waypoints[i].transform.position;
                Gizmos.DrawLine(previous, next);
                previous = next;
            }
            Gizmos.DrawLine(previous, waypoints[0].transform.position);
            //Debug.Log("selected = " + selected);
        }
    }

}
