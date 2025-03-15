using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    
    [SerializeField] private PlayerController playerController;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            //both repel each other but the player is not damaged and the AI is not killed
            //since both of them are triggers, they would both call this function
            //so we only need to apply the logic on the owner of the script

            playerController.Repel(other.transform.position); 
            return; //if we don't return, the other object will also execute the switch and one of them will die
        }

        switch (other.tag)
        {
            case "Wall":
                playerController.WallHit();
                break;
            case "AI":
                other.GetComponent<PlayerController>().DieAI(transform.position);
                break;
            case "Player":
                other.GetComponent<PlayerController>().DamagePlayer(transform.position);
                break;
        }
    }
}
