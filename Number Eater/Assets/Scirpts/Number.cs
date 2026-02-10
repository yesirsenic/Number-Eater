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
        rb.linearVelocity = transform.forward * GameManager.Instance.numberSpeed;
    }

    public void NumberUp()
    {
        GameManager.Instance.getNumberSum += num;
        GameManager.Instance.NumberUserChange();
        Debug.Log(GameManager.Instance.getNumberSum);
    }
}
