using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

/// <summary>
/// Movements controller for the current drone transform.
/// Only works if only one drone instance is currently existing.
/// </summary>
public class DroneController : MonoBehaviour
{
    /// <summary>
    /// Public unique script instance for easy drone access.
    /// </summary>
    public static DroneController drone;

    /// <summary>
    /// Rigidbody of the current drone.
    /// </summary>
    private Rigidbody rigidbody;
    /// <summary>
    /// Velocity of the current drone.
    /// </summary>
    private Vector3 currentLocalVelocity;
    /// <summary>
    /// Torque of the current drone.
    /// </summary>
    private Vector3 currentLocalTorque;
    /// <summary>
    /// Switch if the drone is 'activated' or not.
    /// </summary>
    private bool isRunning = false;

    /// <summary>
    /// Getter to know if the drone is active or not.
    /// </summary>
    public bool running { get { return isRunning; } }

    private const float
        dynamicFriction = 5f,
        maxSpeed = 5f,
        maxTorque = Mathf.PI,
        maxAngle = 45f;


    /// <summary>
    /// Awake Function, initialize propreties.
    /// </summary>
    void Awake()
    {
        drone = this;

        rigidbody = GetComponentInChildren<Rigidbody>();
        currentLocalVelocity = Vector3.zero;
        turnOn(isRunning);
    }

    /// <summary>
    /// FixedUpdate Function, update very forces & clamp every limited rotation.
    /// </summary>
    void FixedUpdate()
    {
        // Velocity Update
        rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, computeToLocal(currentLocalVelocity), dynamicFriction * Time.fixedDeltaTime);
        rigidbody.angularVelocity = Vector3.Lerp(rigidbody.angularVelocity, computeToLocal(currentLocalTorque), dynamicFriction * Time.fixedDeltaTime);

        // Pitch Clamping
        if (Mathf.Abs(UnsignedEuler_toSigned(rigidbody.transform.rotation.eulerAngles.x)) > maxAngle)
            rigidbody.transform.rotation = Quaternion.Euler(maxAngle * Mathf.Sign(rigidbody.transform.rotation.eulerAngles.x),
                rigidbody.transform.rotation.eulerAngles.y, rigidbody.transform.rotation.eulerAngles.z);

        // Roll Clamping
        if (Mathf.Abs(UnsignedEuler_toSigned(rigidbody.transform.rotation.eulerAngles.z)) > maxAngle)
            rigidbody.transform.rotation = Quaternion.Euler(rigidbody.transform.rotation.eulerAngles.x,
                rigidbody.transform.rotation.eulerAngles.y, maxAngle * Mathf.Sign(rigidbody.transform.rotation.eulerAngles.z));
    }

    /// <summary>
    /// Euler angle converter.
    /// Change a specific value from [0, 360[ to [-180, 180[.
    /// </summary>
    /// <param name="val">Specific value</param>
    /// <returns>Result of the computation</returns>
    private float UnsignedEuler_toSigned(float val)
    {
        return (val > 180) ? val - 360 : val;
    }


    /// <summary>
    /// Enumeration of every axis on which to apply force(s) on.
    /// </summary>
    public enum Axis { X,Y,Z }
    /// <summary>
    /// Change the current content of a vec3 with a specific value depending on the axis.
    /// </summary>
    /// <param name="vec">Reference of the vec3</param>
    /// <param name="val">Specific value</param>
    /// <param name="axis">Axis on which to set the value</param>
    private static void set(ref Vector3 vec, float val, Axis axis)
    {
        switch (axis)
        {
            case Axis.X: vec.x = val; break;
            case Axis.Y: vec.y = val; break;
            case Axis.Z: vec.z = val; break;
            default: break;
        }
    }

    /// <summary>
    /// Set the current drone velocity through a new force on specific(s) axis/axes.
    /// </summary>
    /// <param name="force">New specific force</param>
    /// <param name="axes">Table of every modified axis/axes</param>
    public void setVelocity(float force, params Axis[] axes)
    {
        if (Mathf.Abs(force) > maxSpeed)
            force = Mathf.Sign(force) * maxSpeed;
        foreach (Axis axis in axes)
            set(ref currentLocalVelocity, force, axis);
    }

    /// <summary>
    /// Set the current drone angular velocity through a new force on specific(s) axis/axes.
    /// </summary>
    /// <param name="force">New specific force</param>
    /// <param name="axes">Table of every modified axis/axes</param>
    public void setTorque(float torque, params Axis[] axes)
    {
        if (Mathf.Abs(torque) > maxTorque)
            torque = Mathf.Sign(torque) * maxTorque;
        foreach (Axis axis in axes)
            set(ref currentLocalTorque, torque, axis);
    }

    /// <summary>
    /// 'Activate' the current drone, allow for landing/takeoff.
    /// </summary>
    /// <param name="check">New drone state of activation</param>
    public void turnOn(bool check)
    {
        isRunning = check;
        if (rigidbody != null)
            rigidbody.useGravity = !isRunning;
    }

    /// <summary>
    /// Modify a specified vec3 to apply its values on a local transformation.
    /// </summary>
    /// <param name="vec">Specified vec3</param>
    /// <returns>Transformed vec3</returns>
    private Vector3 computeToLocal(Vector3 vec)
    {
        return (transform.right * vec.x
            + transform.up * vec.y
            + transform.right * vec.z).normalized;
    }
}
