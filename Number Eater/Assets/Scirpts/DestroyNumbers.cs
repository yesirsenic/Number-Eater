using UnityEngine;

public class DestroyNumbers : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnNumber"))
        {
            Destroy(other.gameObject);
        }
    }
}
