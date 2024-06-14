using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderLineTarget : MonoBehaviour
{
    public string Name;
    public bool CanSee = false;


    private Camera camera;
    private Collider collider;

    private void Awake()
    {
        camera = Camera.main;
        collider = GetComponent<Collider>();
    }

    private void Update()
    {
        CanSee = false;
        Vector3 screenPoint = camera.WorldToViewportPoint(transform.position);
        if (screenPoint.z > 0 &&
               screenPoint.x > 0 && screenPoint.x < 1 &&
               screenPoint.y > 0 && screenPoint.y < 1)
        {
            Ray ray = camera.ScreenPointToRay(camera.WorldToScreenPoint(transform.position));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                if (hit.collider == collider)
                    CanSee = true;
        }
    }
}