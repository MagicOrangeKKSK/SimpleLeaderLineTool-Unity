using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Marisa
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;

        public Transform lookAtTarget;

        public float zoomSpeed = 10f;

        public float xMin = -80f;
        public float xMax = 80f;


        public float distance = 5;
        public float distanceMin = 2f;
        public float distanceMax = 30f;

        public bool isNeedDamping = true;
        public float damping = 8f;
        
        public Vector2 speed = new Vector2(300f, 300f);
        public Vector2 originAngle = new Vector2(30f, 0f);


        private void LateUpdate()
        {
            if (target == null)
                return;

            if (Input.GetMouseButton(0))
            {
                float xAxis = Input.GetAxis("Mouse X");
                float yAxis = Input.GetAxis("Mouse Y");

                originAngle += new Vector2(-yAxis * speed.y, xAxis * speed.x) * Time.deltaTime;
                originAngle.x = ClampAngle(originAngle.x,xMin,xMax);
            }

            distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            distance = Mathf.Clamp(distance,distanceMin,distanceMax);

            Quaternion rotation = Quaternion.Euler(originAngle.x, originAngle.y, 0f);
            Vector3 dis = new Vector3(0, 0, -distance);

            Vector3 targetPosition = target.position;
            if(lookAtTarget!=null)
                targetPosition = lookAtTarget.position;

            Vector3 position = rotation * dis + targetPosition;
            if (isNeedDamping)
            {
                transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * damping);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
            }
            else
            {
                transform.position = position;
                transform.rotation = rotation;
            }
        }


        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                lookAtTarget = null;
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
            {
                angle += 360;
            }
            if (angle > 360)
            {
                angle -= 360;

            }
            return Mathf.Clamp(angle, min, max);
        }


    }

}