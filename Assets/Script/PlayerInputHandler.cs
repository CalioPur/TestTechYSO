using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController carController;
    [SerializeField] private Joystick playerInput; //I use the joystick from the joystick pack because i had issues with the new input system
                                                   //Using the Joystick instead of the FloatingJoystick so i could change the type of joystick
                                                   //in the future in settings, eventually
                                                   
    private float accelerationInput = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 CarInput = Vector2.zero;
        //Get the input from the joystick
        Vector2 input = playerInput.Direction;
        Vector3 AimDirection = new Vector3(input.x, 0, input.y);
        Vector3 TargetPosition = transform.position + AimDirection*10; //we multiply by 5 to get a point 5 units in front of the player
                                                                       //By doing this, the car will move toward a point which position is controlled by the joystick
        
        //Because of the camera angle, I might need to tweak some value in the future
        //^ instead of tweaking the value, I just changed the angle of the terrain, it was easier and now the camera is right behind the car
        
        CarInput.x = TurnTowardTarget(TargetPosition);
        CarInput.y = accelerationInput; //We always want the car to move forward
        
        carController.SetInput(CarInput);
    }
    
    public void startPlayerInput()
    {
        accelerationInput = 1f;
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
