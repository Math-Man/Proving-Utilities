using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{

    public float verticalInputAcceleration = 1;
    public float horizontalInputAcceleration = 20;

    public float maxSpeed = 10;
    public float maxRotationSpeed = 100;

    public float velocityDrag = 1;
    public float rotationDrag = 1;

    public Vector3 Velocity { get; private set; }
    public float zRotationVelocity { get; private set; }


    public void ApplyForce(Vector3 force)
    {
        Vector3 acceleration = force * verticalInputAcceleration * 2;
        Velocity += acceleration;
    }

    public void ApplyRotationalForce(float turnAngleForce)
    {
        float zTurnAcceleration = -1 * turnAngleForce * horizontalInputAcceleration;
        zRotationVelocity += zTurnAcceleration * Time.deltaTime;
    }

    private void Update()
    {
        // apply velocity drag
        Velocity = Velocity * (1 - Time.deltaTime * velocityDrag);

        // clamp to maxSpeed
        Velocity = Vector3.ClampMagnitude(Velocity, maxSpeed);

        // apply rotation drag
        zRotationVelocity = zRotationVelocity * (1 - Time.deltaTime * rotationDrag);

        // clamp to maxRotationSpeed
        zRotationVelocity = Mathf.Clamp(zRotationVelocity, -maxRotationSpeed, maxRotationSpeed);

        // update transform
        transform.position += Velocity * Time.deltaTime;
        transform.Rotate(0, 0, zRotationVelocity * Time.deltaTime);
    }

}
