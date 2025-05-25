// SideScrollingCamera.cs
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SideScrollingCamera : MonoBehaviour
{
    public Transform trackedObject; // ��һ�������Ҫ��׷�ٵĶ���

    [Header("Camera Positioning")]
    public float yOffset = 2f;         // ����ڴ�ֱ�����������׷�ٶ����ƫ����
    public float xLookAhead = 2f;      // ����ѡ�������ˮƽ�����Ͽ������ǰ������ľ���
    public float yLookAhead = 1f;      // ����ѡ������ڴ�ֱ�����Ͽ��������Ծ/���䷽��ľ���

    [Header("Fixed Y Levels (for SetUnderground)")]
    public float normalLevelY = 6.5f;      // �� SetUnderground(false) ʱ�����Y��Ĺ̶��߶�
    public float undergroundLevelY = -9.5f;  // �� SetUnderground(true) ʱ�����Y��Ĺ̶��߶�

    [Header("Underground Settings")] // �������ҵ�������������
    public float undergroundThreshold = 0f; // <--- �������������ж���

    [Header("Follow Speed")]
    public float horizontalFollowSpeed = 5f; // ˮƽ�����ƽ���ٶ�
    public float verticalFollowSpeed = 3f;   // ��ֱ�����ƽ���ٶ�

    private Vector3 _currentTargetPosition;
    private Vector3 _currentVelocity;

    private void Start()
    {
        if (trackedObject == null)
        {
            Debug.LogError("SideScrollingCamera: Tracked Object is not set!");
            enabled = false;
            return;
        }

        _currentTargetPosition = new Vector3(
            trackedObject.position.x + xLookAhead,
            trackedObject.position.y + yOffset,
            transform.position.z
        );
        transform.position = _currentTargetPosition;
    }

    private void LateUpdate()
    {
        if (trackedObject == null) return;

        // 1. ����Ŀ��Xλ��
        float targetX = Mathf.Max(transform.position.x, trackedObject.position.x + xLookAhead);

        // 2. ����Ŀ��Yλ�� (�������Y�ᣬ������ƫ�ƺ�ǰհ)
        // Rigidbody2D�ļ����Ϊ�˱�����û��Rigidbody2Dʱ����
        float playerVerticalVelocityEffect = 0f;
        Rigidbody2D playerRb = trackedObject.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerVerticalVelocityEffect = playerRb.velocity.y * yLookAhead * Time.deltaTime;
        }
        float targetY = trackedObject.position.y + yOffset + playerVerticalVelocityEffect;

        _currentTargetPosition.x = targetX;
        _currentTargetPosition.y = targetY;
        _currentTargetPosition.z = transform.position.z;

        // ƽ���ƶ����
        Vector3 smoothedPosition = transform.position;
        // ʹ�� 1f / speed ��Ϊ����ʱ���ֱ�ۣ�speedֵԽ������ʱ��ԽС������ƶ�Խ��
        // ��� speed ֵ��С������С��1��������ʱ���������ƶ����������ƽ��
        // ȷ�� speed ��Ϊ0
        float hDampTime = (horizontalFollowSpeed > 0.01f) ? 1f / horizontalFollowSpeed : 0.01f;
        float vDampTime = (verticalFollowSpeed > 0.01f) ? 1f / verticalFollowSpeed : 0.01f;

        smoothedPosition.x = Mathf.SmoothDamp(transform.position.x, _currentTargetPosition.x, ref _currentVelocity.x, hDampTime);
        smoothedPosition.y = Mathf.SmoothDamp(transform.position.y, _currentTargetPosition.y, ref _currentVelocity.y, vDampTime);

        transform.position = smoothedPosition;
    }

    public void SetUnderground(bool isUnderground)
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = isUnderground ? undergroundLevelY : normalLevelY;
        transform.position = cameraPosition; // ��������Y��

        _currentTargetPosition.y = cameraPosition.y; // ����ƽ��Ŀ��
        _currentVelocity.y = 0f; // ���ô�ֱ�ٶȣ�������ֵ�ƽ������

        Debug.Log("SetUnderground called. Camera Y set to: " + cameraPosition.y + (isUnderground ? " (Underground)" : " (Normal)"));
    }
}