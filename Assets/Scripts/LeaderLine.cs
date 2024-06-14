using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderLine : MonoBehaviour
{
    [SerializeField]
    private Transform leftPoint;
    [SerializeField]
    private Transform rightPoint;
    [SerializeField]
    private TextMeshProUGUI nameText;

    public LeaderLineManager manager;

    public RectTransform rect;

    public Transform target { get; private set; }
    public LineRenderer line { get; private set; }

    public void Setup(LeaderLineManager manager, Transform target, LineRenderer line)
    {
        this.target = target;
        this.line = line;
    }


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (target == null)
            return;

        Vector2 point2 = Camera.main.WorldToScreenPoint(target.position);
        Vector2 point1 = Vector2.zero;
        if (point2.x < Screen.width / 2f)
        {
            point1 = ResController.Instance.UICamera.WorldToScreenPoint(rightPoint.position);
            if (point2.x < point1.x)
                point1 = ResController.Instance.UICamera.WorldToScreenPoint(leftPoint.position);
        }
        else
        {
            point1 = ResController.Instance.UICamera.WorldToScreenPoint(leftPoint.position);
            if (point2.x > point1.x)
                point1 = ResController.Instance.UICamera.WorldToScreenPoint(rightPoint.position);
        }

        line.positionCount = 2;
        line.SetPosition(0, point1);
        line.SetPosition(1, point2);
    }



}
