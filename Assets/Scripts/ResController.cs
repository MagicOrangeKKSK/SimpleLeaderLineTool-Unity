using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResController : MonoBehaviour
{
    public static ResController Instance;

    public Camera UICamera;
    public Canvas UICanvas;

    public void Awake()
    {
        Instance = this;    
    }
}
