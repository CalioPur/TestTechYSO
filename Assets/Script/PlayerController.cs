using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerController : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider bodyCollider;
    [SerializeField] private Collider damageCollider;
    
    [Header("Player Only")]
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private ParticleSystem speedLines;
    
    [Header("Stats")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float turningSpeed = 3.5f;
    [SerializeField] private float drift = 0.95f;
    
    private float accelerationInput;
    private float turningInput;
    private float rotationAngle;
    
    private bool isAlive = true;
    [HideInInspector] public bool isPlayer;
    
    
    private void Awake()
    {
        isPlayer = CompareTag("Player");
        
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAlive && transform.position.y < 0.55f) //cant accelerate if the car is in the air
        {
            AccelerateCar();
            DampDrift();
            TurnCar();
        }
        else ApplyGravity();
    }

    private void ApplyGravity()
    {
        rb.AddForce(Vector3.down * 9.81f, ForceMode.Acceleration);
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

    public bool IsDrifting()
    {
        if(RemainingEnemiesCount.Instance.gameEnded || !isAlive || transform.position.y> 0.55f) return false;
        return Mathf.Abs(GetLateralVelocity()) > 4f;
    }
    
    public void SetInput(Vector2 input)
    {
        accelerationInput = input.y;
        turningInput = input.x;
    }

    public IEnumerator BoostPostDrift(float boostDuration)
    {
        
        float realBoostDuration = Mathf.Min(boostDuration, 3f); // we don't want the boost to last more than 3 seconds
        maxSpeed = 15f;
        float time = 0;
        if (isPlayer)
        {
            CameraShakeHandler.Instance.ShakeCamera(1f, boostDuration);
            PostProcessManager.Instance.SetChromaticAberration(0.5f, Mathf.Clamp01(boostDuration/3));
            speedLines.Play();
        }
        
        while (time < realBoostDuration)
        {
            time += Time.deltaTime;

            if (isPlayer)
            {
                virtualCamera.Lens.FieldOfView = Mathf.Lerp(60, 70, Mathf.Clamp01(time / 0.5f));
            }
            yield return null;
            
        }
        StartCoroutine(EndBoost());
    }

    public IEnumerator EndBoost()
    {
        maxSpeed = 10f;
        float time = 0;
        PostProcessManager.Instance.SetChromaticAberration(0, 0.3f);
        while (time < 0.3f)
        {
            time += Time.deltaTime;
            if(isPlayer) virtualCamera.Lens.FieldOfView = Mathf.Lerp(70, 60, time/0.3f);
            
            yield return null;
        }

        if (isPlayer)
        {
            virtualCamera.Lens.FieldOfView = 60;
            speedLines.Stop();
        }

    }

    public void DieAI(Vector3 positionOfAttacker)
    {
        if(!isAlive) return; //We don't want to call this function twice
        isAlive = false;
        RemainingEnemiesCount.Instance.DecreaseRemainingEnemies(); //Actualize the remaining enemies count UI
        //bodyCollider.enabled = false;
        damageCollider.enabled = false;
        
        Vector3 direction = (transform.position - positionOfAttacker).normalized;
        
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(Vector3.up * Random.Range(10,20), ForceMode.Impulse);
        rb.AddForce(direction * Random.Range(15,30), ForceMode.Impulse);
        rb.AddTorque(Vector3.right * Random.Range(5,10) , ForceMode.Impulse);
        rb.AddTorque(Vector3.up * Random.Range(5,10), ForceMode.Impulse);
        Destroy(gameObject, 3f);
    }

    public void DamagePlayer(Vector3 positionOfAttacker)
    {
        //player doesn't die, but gets pushed back
        Repel(positionOfAttacker);
        ScoreManager.Instance.IncreaseScore(-200); //if the player hits another player, he loses 200 points
        CameraShakeHandler.Instance.ShakeCamera(2, 0.3f);
        //TODO : visual feedback on UI (like red glow or something)
        
    }

    public void WallHit()
    {
        //reset car velocity
        rb.linearVelocity = Vector3.zero;
        //the wall makes the car bounce back
        rb.AddForce(-transform.forward * 5, ForceMode.Impulse);
        rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        if(isPlayer) CameraShakeHandler.Instance.ShakeCamera(1.5f, 0.3f);
    }

    public void Repel(Vector3 otherPosition)
    {
        Vector3 direction = (transform.position - otherPosition).normalized;
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.up * Random.Range(3,7), ForceMode.Impulse);
        rb.AddForce(direction * Random.Range(2,5), ForceMode.Impulse);
        if(isPlayer) CameraShakeHandler.Instance.ShakeCamera(1.5f, 0.3f);
    }
}
