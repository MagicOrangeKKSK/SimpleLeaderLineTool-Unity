using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Marisa {
    public class MouseHover : MonoBehaviour, IMouseEventSystem
    {
        public MeshRenderer renderer;
        public List<Material> materials;
        public Material outline;

        public bool IsHovering = false;

        public void Awake()
        {
            renderer = GetComponent<MeshRenderer>();
            renderer.GetMaterials(materials);
            outline = materials[1];
        }

        public void OnMouseEnterHover()
        {
            outline.SetFloat("_Scale", 1.03f);
            IsHovering = true;
        }

        public void OnMouseLeaveHover()
        {
            outline.SetFloat("_Scale", 1f);
            IsHovering = false;
        }

        public void OnMouseLeftButtonClicked()
        {
            FindObjectOfType<CameraController>().lookAtTarget = transform;
        }

        public void OnMouseRightButtonClicked()
        {
        }
    }
}