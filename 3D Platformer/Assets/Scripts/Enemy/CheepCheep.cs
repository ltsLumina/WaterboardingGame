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
    [SerializeField] int impactDamage;

    // Always appears at the bottom due to editor scripting. This is done to make the inspector more readable.
    // These fields are used to determine whether the rotation point or rotation vector should be used.
    [SerializeField, HideInInspector] GameObject rotationPoint;
    [SerializeField, HideInInspector] Vector3 rotationVector;

    // Cached References
    PlayerController player;

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

    void Start() => player = FindObjectOfType<PlayerController>();

    void Update()
    {
        if (isSwimming) Swim();
    }


    /// <summary>
    /// Flips the swim speed if the rotation direction is changed to make the fish move in the inverse direction.
    /// </summary>
    void OnValidate() //TODO: swimming broken
    {
        switch (rotationDirection)
        {
            case RotationDirection.Clockwise:
                swimSpeed = Mathf.Abs(swimSpeed);

                // Flip localscale depending on the rotaion direction
                transform.localScale = new Vector3(1, 1, 1);

                break;

            case RotationDirection.CounterClockwise:
                swimSpeed = -Mathf.Abs(swimSpeed);
                // Flip localscale depending on the rotaion direction
                transform.localScale = new Vector3(-1, 1, 1);

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// Rotates the GameObject around a point (GameObject) or a Vector3.
    /// </summary>
    void Swim()
    {
        if (RotationPoint == null) return;

        Vector3 axis;

        switch (rotationAxis)
        {
            case RotationAxis.X:
                axis = Vector3.right;

                transform.localRotation = Quaternion.Euler(transform.eulerAngles.x, 90, transform.eulerAngles.z);
                break;

            case RotationAxis.Y:
                axis = Vector3.up;
                break;

            case RotationAxis.Z:
                axis = Vector3.forward;

                transform.localRotation = Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

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

        //-TODO: do something to indicate player takes damage here

        // damage the player
        player.CurrentHealth -= impactDamage;
        StartCoroutine(player.HurtOverlay());
        Debug.Log($"Player took {impactDamage} damage!");
    }
}