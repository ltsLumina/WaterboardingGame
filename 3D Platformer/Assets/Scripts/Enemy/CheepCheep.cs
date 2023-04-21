#region
using System;
using Cysharp.Threading.Tasks;
using UnityEditor;
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

    public RotationType RotationTypeValue => rotationTypeValue;
    public RotationAxis RotationAxisValue
    {
        get => rotationAxis;
        set => rotationAxis = value;
    }
    public GameObject RotationPoint => rotationPoint;

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
        Vector3 axis = rotationAxis switch
        { RotationAxis.X => Vector3.right,
          RotationAxis.Y => Vector3.up,
          RotationAxis.Z => Vector3.forward, };

        // Determines whether the rotation is done around a GameObject or a Vector3
        switch (rotationTypeValue)
        {
            case RotationType.GameObject:
                if (RotationPoint == null) return;
                transform.RotateAround(RotationPoint.transform.position, axis, swimSpeed * Time.deltaTime);
                break;

            case RotationType.Vector3:
                transform.RotateAround(rotationVector, axis, swimSpeed * Time.deltaTime);
                break;
        }
    }
}

#if UNITY_EDITOR
/// <summary>
/// Shows the serialized field for the rotation point or the rotation vector depending on the value of the RotationTypeValue enum.
/// This editor script can safely be ignored.
/// </summary>
[CustomEditor(typeof(CheepCheep))]
public class CheepCheepEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var cheepCheep = (CheepCheep)target;

        // If the condition is true, show the serialized field.
        if (cheepCheep.RotationTypeValue == CheepCheep.RotationType.Vector3)
        {
            SerializedProperty myFieldProp = serializedObject.FindProperty("rotationVector");
            EditorGUILayout.PropertyField(myFieldProp);
        }
        else if (cheepCheep.RotationTypeValue == CheepCheep.RotationType.GameObject)
        {
            SerializedProperty myFieldProp = serializedObject.FindProperty("rotationPoint");
            EditorGUILayout.PropertyField(myFieldProp);
        }

        // Apply any changes made to the serialized object.
        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
        var cheepCheep = target as CheepCheep;
        Handles.color = Color.red;

        if (cheepCheep.RotationPoint == null) return;
        Vector3 rotationPoint = cheepCheep.RotationPoint.transform.position;
        float   radius        = Vector3.Distance(rotationPoint, cheepCheep.transform.position);

        Vector3 rotationAxis = cheepCheep.RotationAxisValue switch
        { CheepCheep.RotationAxis.X => Vector3.right,
          CheepCheep.RotationAxis.Y => Vector3.up,
          CheepCheep.RotationAxis.Z => Vector3.forward, };

        Handles.DrawWireDisc(rotationPoint + rotationAxis, rotationAxis, radius);
    }

    /// <summary>
    /// Allows you to create a Cheep Cheep GameObject from the GameObject menu.
    /// </summary>
    [MenuItem("GameObject/3D Object/Create Cheep Cheep", false, 10)]
    static void CreateCheepCheep()
    {
        var myPrefab = Resources.Load<GameObject>("PREFABS (Resources)/CheepCheep");
        if (myPrefab == null)
        {
            Debug.LogError("Failed to load prefab!");
            return;
        }

        GameObject newObject = Instantiate(myPrefab);
        // Optionally set the new object's position, rotation, and scale:
        newObject.transform.position   = Vector3.zero;
        newObject.transform.rotation   = Quaternion.identity;
        newObject.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Orients the CheepCheep GameObject to rotate in the axis that the Scene View camera is facing.
    /// </summary>
    [MenuItem("GameObject/3D Object/Align CheepCheep With View", false, 10)]
    static void OrientCheepCheepToCamera()
    {
        // Get the CheepCheep GameObject
        GameObject cheepCheep = Selection.activeGameObject;

        // Get the Scene View camera
        var sceneView = SceneView.lastActiveSceneView;

        if (sceneView == null)
        {
            Debug.LogError("No Scene View found! (Open the scene view window)");
            return;
        }
        Camera camera = sceneView.camera;

        // Get the direction the camera is facing
        Vector3 cameraDirection = camera.transform.forward;

        // Calculate the absolute values of the camera direction components
        var absCameraDir = new Vector3
            (Mathf.Abs(cameraDirection.x), Mathf.Abs(cameraDirection.y), Mathf.Abs(cameraDirection.z));

        // Set initial values for the maximum absolute direction and value
        var   maxAbsDir    = CheepCheep.RotationAxis.X;
        float maxAbsDirVal = Mathf.NegativeInfinity;

        // Iterate over the camera direction components to find the maximum absolute value
        for (int i = 0; i < 3; i++)
        {
            // If the current component's absolute value is greater than the maximum absolute value so far,
            // update the maximum absolute value and set the maximum absolute direction accordingly
            if (!(absCameraDir[i] > maxAbsDirVal)) continue;
            maxAbsDirVal = absCameraDir[i];
            maxAbsDir    = i switch
            { 0 => CheepCheep.RotationAxis.X,
              1 => CheepCheep.RotationAxis.Y,
              _ => CheepCheep.RotationAxis.Z };
        }

        // Set the CheepCheep's RotationAxisValue to the maximum absolute direction
        cheepCheep.GetComponent<CheepCheep>().RotationAxisValue = maxAbsDir;
    }
}
#endif