using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 centerPosition = new Vector3(0, 0, 0); // 建筑物中心位置

    private const float Y_ANGLE_MAX = 180f;
    private const float Y_ANGLE_MIN = -180f;

    [Space]
    public bool canRotate = true;  
    public float rotateY = 0f; // 摄像机Y轴转角
    public float rotateX = 0f; // 摄像机X轴转角
    public float rotateSpeed = 360f;

    [Space]
    public Vector3 movement = Vector3.zero;
    public float movementSpeed = 10.0f;

    [Space]
    public float minDistance = 5f;
    public float maxDistance = 120f;
    private float distance = 120f;


    private Vector3 originPosition;
    private Vector3 originEuler; // 初始角度
    private float sensitivity;
    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        originPosition = cameraTransform.position;
        originEuler = cameraTransform.eulerAngles;
        distance = (cameraTransform.position - centerPosition).magnitude;
        Reset();
    }

    // 重置摄像机位置和角度
    public void Reset()
    {
        movement = Vector3.zero;
        rotateX = 0f;
        rotateY = 0f;
        cameraTransform.rotation = Quaternion.Euler(originEuler);
        cameraTransform.position = originPosition;
        distance = (cameraTransform.position - centerPosition).magnitude;
        centerPosition = Vector3.zero;
    }

    void Update()
    {
        HandleMouseInput();
        ZoomCamera();
    }

    // 处理鼠标输入
    private void HandleMouseInput()
    {
        if (Input.GetMouseButton(1)) // 右键平移
        {
            MovementCamera();
        }

        if (Input.GetMouseButton(0)) // 左键旋转
        {
            RotateCamera();
        }
    }

    private void RotateCamera()
    {
        if (canRotate)
        {
            rotateY += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            rotateX -= Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
            rotateX = Mathf.Clamp(rotateX, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }
    }

    private void MovementCamera()
    {
        float movementX = -Input.GetAxis("Mouse X");
        float movementY = -Input.GetAxis("Mouse Y");
        movement = new Vector2(movementX, movementY) * movementSpeed * Time.deltaTime;
    }

    // 缩放摄像机
    private void ZoomCamera()
    {
        sensitivity = distance * 0.5f;
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        distance -= scrollWheel * sensitivity;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    void LateUpdate()
    {
        UpdateCameraPosition();
    }

    // 更新摄像机位置和旋转
    private void UpdateCameraPosition()
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(rotateX, rotateY, 0);
        cameraTransform.position = centerPosition + rotation * direction + rotation * movement;
        cameraTransform.rotation = rotation;

        centerPosition = cameraTransform.position + cameraTransform.forward * distance;
        movement = Vector3.zero;
    }
}
