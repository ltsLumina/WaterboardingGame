using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class MakePrefabsLastInHeirarchy : EditorWindow
{
    private List<Object> _list = new();

    [MenuItem("Prefabs In Hierarchy / Put the at the bottom of hierarchy")]
    static void Open()
    {
        GetWindow<MakePrefabsLastInHeirarchy>();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneView;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneView;
    }

    void OnSceneView(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.type == EventType.DragExited)
        {
            Debug.Log("Drag Exit OVER SCENE VIEW");
            var draggedObjs = DragAndDrop.objectReferences;

            bool objsArePrefabs = true;
            foreach (Object draggedObj in draggedObjs)
            {
                if (draggedObj is GameObject == false)
                {
                    objsArePrefabs = false;
                    _list.Clear();
                    break;
                }
                else
                {
                    _list.Add(draggedObj);
                }
            }

            if (objsArePrefabs)
            {
                var roots = SceneManager.GetActiveScene().GetRootGameObjects();
                foreach (GameObject root in roots)
                {
                    int a = 0;
                    for (int i = 0; i < _list.Count; i++)
                    {
                        var draggedObj = _list[a];
                        var source = PrefabUtility.GetCorrespondingObjectFromSource(root);
                        if (source == draggedObj)
                        {
                            _list.RemoveAt(a);
                            root.transform.SetSiblingIndex(roots.Length - 1);
                        }
                        else a++;
                    }
                }

                _list.Clear();
            }
        }
    }
}
