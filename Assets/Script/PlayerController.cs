using System;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    
    [Header("Stats")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float turningSpeed = 3.5f;
    [SerializeField] private float drift = 0.95f;
    
    private float accelerationInput;
    private float turningInput;
    private float rotationAngle;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        AccelerateCar();
        DampDrift();
        TurnCar();
    }

    

    private void AccelerateCar()
    {
        if(accelerationInput == 0)
        {
            rb.linearDamping = Mathf.Lerp(rb.linearDamping, 3f, Time.fixedDeltaTime * 3f);
        }
        else rb.linearDamping = 0;
        Vector3 force = transform.forward * (accelerationInput * acceleration);
        rb.AddForce(force, ForceMode.Force);
        
        
        if (rb.linearVelocity.magnitude > maxSpeed) //Limit the speed of the car after applying the force
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
    
    private void TurnCar()
    {
        float allowTurnThreshold = rb.linearVelocity.magnitude / 8;
        allowTurnThreshold = Mathf.Clamp01(allowTurnThreshold); //if the car is not moving, we should not allow turning
        
        rotationAngle += turningInput * turningSpeed * allowTurnThreshold;
        rb.MoveRotation(Quaternion.Euler(0, rotationAngle, 0));
    }
    
    private void DampDrift()
    {
        Vector3 forwardVelocity = Vector3.Dot(rb.linearVelocity, transform.forward) * transform.forward;
        Vector3 rightVelocity = Vector3.Dot(rb.linearVelocity, transform.right) * transform.right;
        rb.linearVelocity = forwardVelocity + rightVelocity * drift;
    }

    private float GetLateralVelocity()
    {
        return Vector3.Dot( transform.right,rb.linearVelocity);
    }

    public bool IsDrifting(out float lateralVelocity)
    {
        lateralVelocity = GetLateralVelocity();
        
        return Mathf.Abs(lateralVelocity) > 4f;
    }
    
    public void SetInput(Vector2 input)
    {
        accelerationInput = input.y;
        turningInput = input.x;
    }
}
