using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDEventSystem : MonoBehaviour
{
    public IMouseEventSystem CurrentSelectObject;

    public void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            IMouseEventSystem mouseEventSystem = hit.collider.GetComponent<IMouseEventSystem>();
            if (mouseEventSystem != null)
            {
                if (CurrentSelectObject != mouseEventSystem)
                {
                    if (CurrentSelectObject != null)
                        CurrentSelectObject.OnMouseLeaveHover();

                    CurrentSelectObject = mouseEventSystem;
                    CurrentSelectObject.OnMouseEnterHover();
                }
                
            }
        }
        else
        {
            if (CurrentSelectObject != null)
            {
                CurrentSelectObject.OnMouseLeaveHover();
                CurrentSelectObject = null;
            }
        }
        if (CurrentSelectObject != null)
        {
            if (Input.GetMouseButtonDown(0))
                CurrentSelectObject.OnMouseLeftButtonClicked();
            if (Input.GetMouseButtonDown(1))
                CurrentSelectObject.OnMouseRightButtonClicked();
        }
    }
}


public interface IMouseEventSystem
{
    public void OnMouseEnterHover();
    public void OnMouseLeaveHover();
    public void OnMouseLeftButtonClicked();
    public void OnMouseRightButtonClicked();
}