using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //Will allow us to set the ball speed.
    [SerializeField]
    private float ballSpeed;

    //I am still unsure as why we need to declare this here
    PlayerControls controls;

    // Start is called before the first frame update
    void Awake()
    {
        //Allows us to access the PlayerControls class (?) 
        controls = new PlayerControls();

        //When the player presses the attributed button, the ball is shot
        controls.Gameplay.ShootBall.performed += ctx => Shoot();
    }


    void Update()
    {
        //When the ball no longer is the child of the bar, it is shot in the direction it is facing
        //Well technically on its right, but it serves the same purpose here
        if (transform.parent == null)
        {
            transform.Translate(transform.right * Time.deltaTime * ballSpeed, Space.World);
        }
    }

    //When shot, the ball is no longer a child of the playerBar
    void Shoot()
    {
        transform.parent = null;
    }

    //Is called once when a collision starts
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If the ball is independant, when it hits an object (or multiple objects), it will return a sum of all the normals from the different collision,
        //which will then be used in an attempt to best emulate how it should bounce against obstacles
        if (transform.parent == null)
        {
            Vector2 finalNormal = Vector2.zero;
            for (int i = 0; i < collision.contacts.Length; i++)
            {
                finalNormal += collision.GetContact(i).normal;
            }
            //The Vector3.reflect will give us the direction the ball should be going after the collision
            transform.right = Vector3.Reflect(transform.right, finalNormal);
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
