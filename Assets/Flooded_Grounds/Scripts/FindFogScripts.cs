using UnityEngine;
using System.Collections;

public class FindFogScripts : MonoBehaviour
{
    [ContextMenu("FIND FOG SCRIPTS NOW!")]  // Right-click script → Chạy luôn không Play!
    public void FindAllFogScripts()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        int fogCount = 0;

        foreach (GameObject go in allObjects)
        {
            if (go.name.ToLower().Contains("Fog"))
            {
                fogCount++;
                Debug.Log($"<color=yellow>🔍 FOG #{fogCount}: {go.name}</color>", go);  // Click log → Select object!

                Component[] components = go.GetComponents<Component>();
                foreach (Component comp in components)
                {
                    Debug.Log($"   📜 SCRIPT: <b>{comp.GetType().Name}</b>");  // Tên script chính xác!
                }
                Debug.Log("   ---");  // Phân cách
            }
        }

        Debug.Log($"<color=green>✅ Tìm thấy <b>{fogCount}</b> Fog objects!</color>");
    }
}