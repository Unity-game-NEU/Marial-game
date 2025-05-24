// SideScrollingCamera.cs
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SideScrollingCamera : MonoBehaviour
{
    public Transform trackedObject; // 玩家或其他需要被追踪的对象

    [Header("Camera Positioning")]
    public float yOffset = 2f;         // 相机在垂直方向上相对于追踪对象的偏移量
    public float xLookAhead = 2f;      // （可选）相机在水平方向上看向玩家前进方向的距离
    public float yLookAhead = 1f;      // （可选）相机在垂直方向上看向玩家跳跃/下落方向的距离

    [Header("Fixed Y Levels (for SetUnderground)")]
    public float normalLevelY = 6.5f;      // 当 SetUnderground(false) 时，相机Y轴的固定高度
    public float undergroundLevelY = -9.5f;  // 当 SetUnderground(true) 时，相机Y轴的固定高度

    [Header("Underground Settings")] // 新增或找到合适区域添加
    public float undergroundThreshold = 0f; // <--- 在这里添加这行定义

    [Header("Follow Speed")]
    public float horizontalFollowSpeed = 5f; // 水平跟随的平滑速度
    public float verticalFollowSpeed = 3f;   // 垂直跟随的平滑速度

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

        // 1. 计算目标X位置
        float targetX = Mathf.Max(transform.position.x, trackedObject.position.x + xLookAhead);

        // 2. 计算目标Y位置 (跟随玩家Y轴，并加上偏移和前瞻)
        // Rigidbody2D的检查是为了避免在没有Rigidbody2D时出错
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

        // 平滑移动相机
        Vector3 smoothedPosition = transform.position;
        // 使用 1f / speed 作为阻尼时间更直观，speed值越大，阻尼时间越小，相机移动越快
        // 如果 speed 值很小（比如小于1），阻尼时间会变大，相机移动会更慢、更平滑
        // 确保 speed 不为0
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
        transform.position = cameraPosition; // 立即设置Y轴

        _currentTargetPosition.y = cameraPosition.y; // 更新平滑目标
        _currentVelocity.y = 0f; // 重置垂直速度，避免奇怪的平滑惯性

        Debug.Log("SetUnderground called. Camera Y set to: " + cameraPosition.y + (isUnderground ? " (Underground)" : " (Normal)"));
    }
}