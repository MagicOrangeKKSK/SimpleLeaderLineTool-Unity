using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderLineManager1 : MonoBehaviour
{
    List<LeaderLineTarget> targets;
    Vector3 center;
    
    public void Start()
    {
        targets = FindObjectsByType<LeaderLineTarget>(FindObjectsSortMode.None).ToList();
    }

    private void Update()
    {
        center = Vector3.zero;
        targets.ForEach(x => center += x.transform.position);
        center /= targets.Count;

        targets.ForEach(x => Debug.DrawLine(center, x.transform.position));

    }

    private void OnDrawGizmos()
    {
        if (targets.Count == 0) return;

        //To2D
        List<Vector2> screenPoints = new List<Vector2>();
        targets.ForEach(x => screenPoints.Add(Camera.main.WorldToScreenPoint(x.transform.position)));
        Vector2 screenCenterPoint = Vector2.zero;
        screenPoints.ForEach(x => screenCenterPoint += x);
        screenCenterPoint /= targets.Count;
        Gizmos.color = Color.yellow;
        float maxDistance = 0;
        screenPoints.ForEach(x =>
        {
            Gizmos.DrawLine(x, screenCenterPoint);
            float distance = Vector2.Distance(x, screenCenterPoint);
            if(distance > maxDistance)
                maxDistance = distance;
        });

        

        Gizmos.DrawWireSphere(Camera.main.WorldToScreenPoint(transform.position), 10f);
        Gizmos.DrawWireSphere(screenCenterPoint, maxDistance);

    }
}