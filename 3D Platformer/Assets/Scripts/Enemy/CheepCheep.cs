#region
using System;
using UnityEngine;
#endregion

public class CheepCheep : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] RotationType rotationTypeValue;
    [SerializeField] RotationAxis rotationAxis;
    [SerializeField] RotationDirection rotationDirection;

    [Header("Configurable Parameters")]
    [SerializeField] bool isSwimming = true;
    [SerializeField] float swimSpeed = 20f;

    // Always appears at the bottom due to editor scripting. This is done to make the inspector more readable.
    // These fields are used to determine whether the rotation point or rotation vector should be used.
    [SerializeField, HideInInspector] GameObject rotationPoint;
    [SerializeField, HideInInspector] Vector3 rotationVector;

    #region Enums (RotationType, RotationAxis, RotationDirection)
    public enum RotationType
    {
        GameObject,
        Vector3,
    }

    public enum RotationAxis
    {
        X,
        Y,
        Z,
    }

    enum RotationDirection
    {
        Clockwise,
        CounterClockwise,
    }
    #endregion

    #region Properties
    public RotationType RotationTypeValue => rotationTypeValue;
    public RotationAxis RotationAxisValue
    {
        get => rotationAxis;
        set => rotationAxis = value;
    }
    public GameObject RotationPoint => rotationPoint;
    #endregion

    void Update()
    {
        if (isSwimming) Swim();
    }

    /// <summary>
    /// Flips the swim speed if the rotation direction is changed to make the fish move in the inverse direction.
    /// </summary>
    void OnValidate() =>
        swimSpeed = rotationDirection switch
        { RotationDirection.Clockwise        => Mathf.Abs(swimSpeed),
          RotationDirection.CounterClockwise => -Mathf.Abs(swimSpeed),
          _                                  => swimSpeed };

    /// <summary>
    /// Rotates the GameObject around a point (GameObject) or a Vector3.
    /// </summary>
    void Swim()
    {
        if (RotationPoint == null) return;

        Vector3 axis = rotationAxis switch
        { RotationAxis.X => Vector3.right,
          RotationAxis.Y => Vector3.up,
          RotationAxis.Z => Vector3.forward,
          _              => throw new ArgumentOutOfRangeException()
        };

        // Determines whether the rotation is done around a GameObject or a Vector3
        switch (rotationTypeValue)
        {
            case RotationType.GameObject:
                transform.RotateAround(RotationPoint.transform.position, axis, swimSpeed * Time.deltaTime);
                break;

            case RotationType.Vector3:
                transform.RotateAround(rotationVector, axis, swimSpeed * Time.deltaTime);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        const int damage = 10; // TODO: Make this a variable and cache the player controller/health.
        other.gameObject.GetComponent<PlayerController>().CurrentHealth -= damage;
        Debug.Log($"Player took {damage} damage!");
    }
}