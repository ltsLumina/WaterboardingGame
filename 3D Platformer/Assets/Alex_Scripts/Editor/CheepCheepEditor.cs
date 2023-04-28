using UnityEditor;
using UnityEngine;

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
        if (GUILayout.Button("Orient CheepCheep", GUILayout.Height(30)))
        {
            OrientCheepCheepToCamera();
        }

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

        if (cheepCheep != null && cheepCheep.RotationPoint == null) return;
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