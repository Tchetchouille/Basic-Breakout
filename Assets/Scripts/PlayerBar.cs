using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBar : MonoBehaviour
{
    //This creates an object of the class PlayerControls, which contains all the actions we'll be using. Or not. I am not sure about this.
    PlayerControls controls;

    //Variables to store the context returned by the controller input.
    Vector2 move;
    Vector2 rotate;

    //patRadius is the size of the circle on which the player moves.
    [SerializeField]
    private float pathRadius;
    //playerSpeed is the speed at which the player moves on the circle.
    [SerializeField]
    private float playerSpeed;
    //sensitivity will allow us to define how sensitive the4 stick is gonna be. A small value is more sensitive.
    //A value of 0.6 or smaller will cause problems as the stick going back in place when released will be registered as an input in the opposite direction.
    [SerializeField]
    private float sensitivity;

    //Declaring those here instead of in the Update allows us to give them a value of 0 at the start, and then only update it when a new input is made
    float inputCos = 0;
    float inputSin = 0;
    float inputAngle = 0;


    void Awake()
    {
        //THIS creates an object. 
        controls = new PlayerControls();


        //ROTATION AND MOVEMENT
        //Makes it so that when the Rotate action is performed, it stores the input values in the rotate variable.
        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;

        //Will allow me to access the position of the left stick
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;

        //Sets the position and rotation of the player at the start.
        inputAngle = 270;
        transform.rotation = Quaternion.Euler(0, 0, 90);

    }


    void Update()
    {
        //ROTATION
        //Storing the right stick orientation in a variable
        Vector3 r = new Vector3(rotate.x, rotate.y, 0);
        //The condition will allow us to avoid having NaN, and will allow us to only use the input if the stick is obviously tilted, and ignore it if it is only tilted a little
        if (rotate.x > sensitivity | rotate.x < -sensitivity | rotate.y > sensitivity | rotate.y < -sensitivity)
        {
            //If the variable is not (0, 0, 0), the the object is rotated. This allows to not reset the rotation everytime there is no input.
            if (r != new Vector3(0, 0, 0))
            {
                transform.right = r;
            }
        }


        //MOVEMENT AROUND THE CENTER
        //This will get us the cos, sin and angle of the stick input
        //The condition will allow us to avoid having NaN, and will allow us to only use the input if the stick is obviously tilted, and ignore it if it is only tilted a little
        if (move.x > sensitivity | move.x < -sensitivity | move.y > sensitivity | move.y < -sensitivity)
        {
            inputCos = getCos(move.x, move.y);
            inputSin = getSin(move.x, move.y);
            inputAngle = getAngle(inputCos, inputSin);
        }

        //Same but for the player position
        float playerCos = getCos(transform.position.x, transform.position.y);
        float playerSin = getSin(transform.position.x, transform.position.y);
        float playerAngle = getAngle(playerCos, playerSin);

        //This variable will be used to store a positive or negative speed
        float speed = 0;

        /* Basically, if the distance between the greater angle and the smaller angle is > than 
         * the addition between the distance between the biger angle and 360, and the distance between the smaller angle and 0, 
         * it means that the shorter path is by going clockwise. So we adjust the speed accordingly */
        // the 0.5 here is a margin that will make it so the player don't go back and forth when their speed is > than the distance between its position and the input position
        if(playerAngle > inputAngle + 0.5)
        {
            if(playerAngle - inputAngle > (360 - playerAngle) + inputAngle)
            {
                speed = playerSpeed;
            }
            else
            {
                speed = -playerSpeed;
            }
        }
        else if(playerAngle < inputAngle - 0.5)
        {
            if(inputAngle - playerAngle > (360 - inputAngle) + playerAngle)
            {
                speed = -playerSpeed;
            }
            else
            {
                speed = playerSpeed;
            }
        }
        else
        {
            speed = 0;
        }

        //Then we simply store the rotation to save it, and rotate the player around the origin, before giving it its old rotation again.
        var playerRotation = transform.rotation;
        transform.RotateAround(new Vector3(0, 0, 0), Vector3.forward, speed * Time.deltaTime);
        transform.rotation = playerRotation;


        //A small function that allows us to get a cos from a point
        float getCos(float x, float y)
        {
            return x * (1 / Mathf.Sqrt(x * x + y * y));
        }

        //A small function that allows us to get a sin from a point
        float getSin(float x, float y)
        {
            return y * (1 / Mathf.Sqrt(x * x + y * y));
        }

        //Allows us to calculate the angle at which the stick is pushed.
        float getAngle(float cos, float sin)
        {
            //If sin is positive, it means the stick is pushed forward, so the angle is between 0 and 180
            if(sin > 0)
            {
                //We then use Mathf.Acos to calculate the angle, and convert it to degrees using * 180 / Mathf.PI
                return Mathf.Acos(cos) * 180 / Mathf.PI;
            }
            //If the stick is pushed backwards, it means that we have to substract the angle returned by Mathf.Acos(inputCos) to 360. 
            //The way it is written is based on the way I figured it uot, so it will allow me to understand it better if I stumble on it after I have forgotten how it works.
            else
            {
                return 180 + (180 - (Mathf.Acos(cos) * 180 / Mathf.PI));
            }
        }
    }


    //Pretty self explanatory
    void OnEnable()
    {
        controls.Gameplay.Enable();
    }


    //Pretty self explanatory
    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
