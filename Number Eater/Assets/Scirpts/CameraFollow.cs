using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Follow Settings")]
    public float followSpeedX = 20f;   // 좌우 부드러움
    public float followSpeedZ = 999f; // 앞으로는 즉시 따라가게

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        Vector3 currentPos = transform.position;

        float targetX = target.position.x + offset.x;
        float targetZ = target.position.z + offset.z;

        float newX = Mathf.Lerp(currentPos.x, targetX, followSpeedX * Time.deltaTime);

        
        float newZ = Mathf.Lerp(currentPos.z, targetZ, followSpeedZ * Time.deltaTime);

        transform.position = new Vector3(
            newX,
            currentPos.y,
            newZ
        );
    }
}
