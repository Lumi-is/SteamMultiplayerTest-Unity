using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

namespace Player
{
    public class PlayerMovementHandler : NetworkBehaviour
    {
        [SerializeField] Rigidbody rb; 
        [SerializeField] Transform orientation;
        [SerializeField] Transform head;

        public float GlobalSensitivity;

        float SensitivityX;
        float SensitivityY;

        float MousePosX;
        float MousePosY;

        float xRotation;
        float yRotation;

        private Vector3 moveDirection;

        public float WalkSpeed;
        public float RunSpeed;

        private float HorizontalInput;
        private float VerticalInput;

        public bool Crouching;
        public bool isRagdoll;

        void Start()
        {
            if(isLocalPlayer)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        void Update()
        {
            if(isLocalPlayer)
            {

                //Get all input information from client
                GetInputs();

                if(!isRagdoll)
                {
                    //Moves the player in regard to transform rotation "orientation" 
                    MoveAction();
                }
                
            }
        }

        void GetInputs()
        {
            SensitivityX = GlobalSensitivity;
            SensitivityY = GlobalSensitivity;
        
            MousePosX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * SensitivityX;
            MousePosY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * SensitivityY;

            yRotation += MousePosX;
            xRotation -= MousePosY;
            xRotation = Mathf.Clamp(xRotation,-72f,72f);

            //Values ranging from -1 to 1
            HorizontalInput = Input.GetAxis("Horizontal");
            VerticalInput = Input.GetAxis("Vertical");
        

            if(Input.GetKey(KeyCode.LeftControl))
            {
                Crouching = true;
            } else {
                Crouching = false;
            }

            if(Input.GetKey(KeyCode.Space))
            {
                isRagdoll = true;
                rb.AddExplosionForce(5,new(),1);
                rb.freezeRotation = false;
            } else
            {
                isRagdoll = false;
                transform.rotation = new();
                rb.freezeRotation = true;
            }
        }

        void MoveAction()
        {

            head.rotation = Quaternion.Euler(xRotation,yRotation,0);
            orientation.rotation = Quaternion.Euler(0,yRotation,0);

            moveDirection = orientation.forward * VerticalInput + orientation.right * HorizontalInput;

            if (Input.GetKey(KeyCode.LeftShift) && !Crouching)
            {
                if (Input.GetAxisRaw("Vertical") == -1 && Input.GetAxisRaw("Horizontal") == 0)
                {
                    rb.linearVelocity = new Vector3(moveDirection.x * RunSpeed * 0.8f, rb.linearVelocity.y, moveDirection.z * RunSpeed * 0.8f);
                }
                else if (Math.Abs(Input.GetAxisRaw("Horizontal")) != Math.Abs(Input.GetAxisRaw("Vertical")))
                {
                    rb.linearVelocity = new Vector3(moveDirection.x * RunSpeed, rb.linearVelocity.y, moveDirection.z * RunSpeed);
                }
                else
                {
                    rb.linearVelocity = new Vector3(moveDirection.x * RunSpeed * 0.5f, rb.linearVelocity.y, moveDirection.z * RunSpeed * 0.5f);
                }


            }
            else
            {
                if (Input.GetAxisRaw("Horizontal") != Input.GetAxisRaw("Vertical") && Input.GetAxisRaw("Horizontal") != -1 * Input.GetAxisRaw("Vertical"))
                {
                    rb.linearVelocity = new Vector3(moveDirection.x * WalkSpeed, rb.linearVelocity.y, moveDirection.z * WalkSpeed);
                }
                else
                {
                    rb.linearVelocity = new Vector3(moveDirection.x * WalkSpeed * 0.7f, rb.linearVelocity.y, moveDirection.z * WalkSpeed * 0.7f);
                }
            }
        }
    }
}