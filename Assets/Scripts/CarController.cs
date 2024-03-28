using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking, isNitroActive;
    private float nitroAmount = 100f; // Starting nitro amount

    // Settings
    public float motorForce, breakForce, maxSteerAngle;
    public float nitroBoostFactor = 1.5f; // Multiplier for speed when nitro is active
    public float nitroDepletionRate = 25f; // How fast nitro depletes
    public float nitroRechargeRate = 10f; // How fast nitro recharges

    public bool canMove = true;
    // Wheel Colliders
    public WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    public ParticleSystem nitroParticles;

    // Nitro UI
    public Slider nitroSlider;
    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -1f, 0);
        nitroSlider.maxValue = nitroAmount;
        nitroSlider.value = nitroAmount;
    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateNitro();
        }
    }

    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);

        isNitroActive = Input.GetKey(KeyCode.LeftShift) && nitroAmount > 0; // Use nitro with Left Shift
    }

    private void HandleMotor()
    {
        float currentMotorForce = verticalInput * motorForce;
        Vector3 force = transform.forward * currentMotorForce; // Calculate the force vector

        if (isNitroActive)
        {
            nitroParticles.Play();
            float nitroForce = currentMotorForce * nitroBoostFactor; // Calculate nitro force
            force = transform.forward * nitroForce; // Update force vector with nitro
            nitroAmount -= nitroDepletionRate * Time.fixedDeltaTime; // Deplete nitro
            nitroSlider.value = nitroAmount; // Update UI
        }
        else{
            nitroParticles.Stop();
        }
        

        // Apply motor torque as usual
        frontLeftWheelCollider.motorTorque = currentMotorForce;
        frontRightWheelCollider.motorTorque = currentMotorForce;

        // Apply nitro force directly to the Rigidbody for a more immediate effect
        if (isNitroActive)
        {
            GetComponent<Rigidbody>().AddForce(force, ForceMode.Force);
        }

        if (isBreaking)
        {
            ApplyBreaking();
        }
    }


    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateNitro()
    {
        if (!isNitroActive && nitroAmount < 100f) // Recharge nitro if not in use and not full
        {
            nitroAmount += nitroRechargeRate * Time.fixedDeltaTime;
            nitroSlider.value = nitroAmount; // Update UI
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "End"){
            FindObjectOfType<Timer>().FinishRace();
        }
    }
}