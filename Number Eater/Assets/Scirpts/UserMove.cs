using UnityEngine;
using UnityEngine.InputSystem;

public class UserMove : MonoBehaviour
{
    [Header("Move Range")]
    [SerializeField] private float maxX = 2.5f;

    [Header("Drag")]
    [SerializeField] private float dragRatio = 0.6f;   // ⭐ 핵심 (0.3~0.6)
    [SerializeField] private float moveSmooth = 0.06f; // SmoothDamp 시간

    private float targetX;
    private float velocityX;

    // Input
    private Vector2 pointerStartPos;
    private float startX;
    private bool isDragging;

    void Start()
    {
        targetX = transform.position.x;
    }

    void Update()
    {
        if (GameManager.Instance.state != GameState.MainGame)
            return;

        HandlePointerInput();
        ApplyMovement();
    }

    // =============================
    // Pointer Drag (Unity 6)
    // =============================
    void HandlePointerInput()
    {
        if (Pointer.current == null)
            return;

        if (Pointer.current.press.wasPressedThisFrame)
        {
            if(PlayerPrefs.GetInt("Tutorial") == 0)
            {
                GameManager.Instance.TutorialOff();
            }

            isDragging = true;
            pointerStartPos = Pointer.current.position.ReadValue();
            startX = targetX;
        }
        else if (Pointer.current.press.isPressed && isDragging)
        {
            Vector2 currentPos = Pointer.current.position.ReadValue();

            // 화면 비율
            float delta = currentPos.x - pointerStartPos.x;
            float deltaRatio = delta / 500f;

            // 월드 이동 범위 기준
            float moveRange = maxX * 2f;

            targetX = startX + deltaRatio * moveRange * dragRatio;
            targetX = Mathf.Clamp(targetX, -maxX, maxX);
        }
        else if (Pointer.current.press.wasReleasedThisFrame)
        {
            isDragging = false;
        }
    }

    // =============================
    // Movement
    // =============================
    void ApplyMovement()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.SmoothDamp(
            pos.x,
            targetX,
            ref velocityX,
            moveSmooth
        );

        transform.position = pos;
    }
}