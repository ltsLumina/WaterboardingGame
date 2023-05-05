using UnityEngine;

public class SeagullSpin : MonoBehaviour
{
    [SerializeField] float flySpeed =1f;

    void Update()
    {
        transform.Rotate(new Vector3(0,flySpeed,0)* Time.deltaTime);
    }
}