using UnityEngine;

public class Number : MonoBehaviour
{

    private Rigidbody rb;

    [SerializeField]
    private int num;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        Vector3 nextPos =
        rb.position + transform.forward *
        GameManager.Instance.numberSpeed *
        Time.fixedDeltaTime;

        rb.MovePosition(nextPos);
    }

    public void NumberUp()
    {
        GameManager.Instance.getNumberSum += num;
        GameManager.Instance.NumberUserChange();
        Debug.Log(GameManager.Instance.getNumberSum);
    }
}
