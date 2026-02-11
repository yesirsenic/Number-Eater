using UnityEngine;

public class PlayerDigit : MonoBehaviour
{
    private DigitRoot root;

    private void Awake()
    {
        root = GetComponentInParent<DigitRoot>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("SpawnNumber"))
        {
            root.OnHit(other);
        }
        
    }
}
