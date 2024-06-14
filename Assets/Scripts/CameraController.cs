using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 centerPosition = new Vector3(0, 0, 0); // ����������λ��

    private const float Y_ANGLE_MAX = 180f;
    private const float Y_ANGLE_MIN = -180f;

    [Space]
    public bool canRotate = true;  
    public float rotateY = 0f; // �����Y��ת��
    public float rotateX = 0f; // �����X��ת��
    public float rotateSpeed = 360f;

    [Space]
    public Vector3 movement = Vector3.zero;
    public float movementSpeed = 10.0f;

    [Space]
    public float minDistance = 5f;
    public float maxDistance = 120f;
    private float distance = 120f;


    private Vector3 originPosition;
    private Vector3 originEuler; // ��ʼ�Ƕ�
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

    // ���������λ�úͽǶ�
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

    // �����������
    private void HandleMouseInput()
    {
        if (Input.GetMouseButton(1)) // �Ҽ�ƽ��
        {
            MovementCamera();
        }

        if (Input.GetMouseButton(0)) // �����ת
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

    // ���������
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

    // ���������λ�ú���ת
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
