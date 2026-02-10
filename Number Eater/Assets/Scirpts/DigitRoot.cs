using UnityEngine;
using System.Collections;

public class DigitRoot : MonoBehaviour
{
    private bool locked = false;

    public void OnHit(Collider other)
    {
        if (locked) return;


        locked = true;

        Debug.Log("Number 기준 충돌 처리");


        if(other.gameObject.GetComponent<Number>())
        {
            other.gameObject.GetComponent<Number>().NumberUp();
        }

        Destroy(other.gameObject);


        StartCoroutine(UnlockNextPhysicsStep());
    }

    IEnumerator UnlockNextPhysicsStep()
    {
        // 다음 물리 스텝까지 대기
        yield return new WaitForFixedUpdate();
        locked = false;
    }
}
