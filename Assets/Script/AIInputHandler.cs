using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIInputHandler : MonoBehaviour
{
    
    [SerializeField] private PlayerController carController;
    
    List<Transform> otherPlayers = new List<Transform>();
    Transform target;
    Transform lastTarget;
    private float timeOffset;
    private float timeToChangeTarget;

    private float accelerationInput = 0f;
    

    private void Awake()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("AI"); //We get all the AI in the scene
        foreach (GameObject player in players)
        {
            otherPlayers.Add(player.transform);
        }
        //remove self from the list
        otherPlayers.Remove(transform);
        //then we add the player
        otherPlayers.Add(GameObject.FindGameObjectWithTag("Player").transform);
        
        timeOffset = Random.Range(0, 1f); //We add a random offset to the AI so they don't change target at the same time
        timeToChangeTarget = Random.Range(0, 10f); //The AI will change target if it have the same since 10 seconds,
                                                   //but we add a random offset to avoid all the AI to change target at the same time
    }
    
    public void StartCarsInput()
    {
        accelerationInput = 1f;
    }
    
    void FixedUpdate()
    {
        Vector2 CarInput = Vector2.zero;
        //Get the input from the joystick
        if(!target) target = GetClosestPlayer(); //We get the closest player to the AI, once a target is set, it will
                                                 //following it to avoid the AI to change target every frame
        
        CarInput.x = TurnTowardTarget(target.position);
        CarInput.y = accelerationInput; //We always want the car to move forward
        
        carController.SetInput(CarInput);
        
        timeOffset -= Time.fixedDeltaTime; //every frame we add the time passed since the last frame
        if (timeOffset < 0f) //every second, we check for a new target
        {
            target = GetClosestPlayer();
            timeOffset = 1f;
        }
        if(lastTarget != target) //if the target changed, we reset the timer
        {
            timeToChangeTarget = 10f;
            lastTarget = target;
        }
        else
        {
            timeToChangeTarget -= Time.fixedDeltaTime;
            if (timeToChangeTarget < 0f) //if the target didn't change for 10 seconds, we get a new target
            {
                target = GetRandomTarget();
                timeToChangeTarget = 20f;
                timeOffset = 10f;
            }
        }
    }

    private Transform GetRandomTarget()
    {
        //remove all null references because the player might have been destroyed
        otherPlayers.RemoveAll(player => player == null);
        
        return otherPlayers[Random.Range(0, otherPlayers.Count)];
    }

    Transform GetClosestPlayer()
    {
        float minDistance = Mathf.Infinity;
        Transform closestPlayer = null;
        foreach (Transform player in otherPlayers)
        {
            //check if the referenced player is destroyed
            if(!player) continue;
            
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }
    
    float TurnTowardTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0; //We don't want the car to go up or down
        direction.Normalize();
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        
        
        float steer = angle / 45;
        steer = Mathf.Clamp(steer, -1, 1);
        
        return steer;
    }
}
