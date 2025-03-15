using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("AI"))
        {
            other.GetComponent<PlayerController>().DieAI(transform.position);
        }
    }
}
