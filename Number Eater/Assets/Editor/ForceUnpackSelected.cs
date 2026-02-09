#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ForceUnpackSelected
{
    [MenuItem("Tools/Force Unpack Selected (Completely)")]
    public static void UnpackCompletely()
    {
        var go = Selection.activeGameObject;
        if (!go)
        {
            Debug.LogError("Hierarchy에서 최상위(루트) 오브젝트를 선택하세요.");
            return;
        }

        PrefabUtility.UnpackPrefabInstance(
            go,
            PrefabUnpackMode.Completely,
            InteractionMode.UserAction
        );

        Debug.Log($"Unpack Completely 완료: {go.name}");
    }
}
#endif