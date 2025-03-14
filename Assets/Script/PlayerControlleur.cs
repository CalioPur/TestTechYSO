using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerControlleur : MonoBehaviour
{
    
    [SerializeField] private Joystick playerInput; //I use the joystick from the joystick pack because i had issues with the new input system
    //Using the Joystick instead of the FloatingJoystick so i could change the type of joystick in the future in settings, eventually
    
    [Header("Stats")]
    [SerializeField] private float speed = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Move(playerInput.Direction);
    }
    
    void Move(Vector2 dir)
    {
        // Move the player
        transform.position += new Vector3(dir.x, 0, dir.y) * (Time.deltaTime * speed);
    }
    
}
