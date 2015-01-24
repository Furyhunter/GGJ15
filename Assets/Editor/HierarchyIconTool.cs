using UnityEngine;
using UnityEditor;
using System.Collections;
using System;


[InitializeOnLoad]
public class HierarchyIconTool
{
    static HierarchyIconTool singleton;
    public static HierarchyIconTool Singleton
    {
        get
        {
            return singleton;
        }
    }

    static HierarchyIconTool()
    {
        singleton = new HierarchyIconTool();
    }

    public delegate bool OnHierarchyDrawIcon(GameObject obj, Rect rect);

    public OnHierarchyDrawIcon onHierarchyCreateIcon;

    Color[] hierarchyDepthColors;

    Texture2D HighlightTexture = null;

    HierarchyIconTool()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyOnGui;
        onHierarchyCreateIcon += EnabledButton;
        onHierarchyCreateIcon += HasMeshFilter;
        onHierarchyCreateIcon += HasCamera;
        onHierarchyCreateIcon += HasLight;

        hierarchyDepthColors = new Color[5];
        hierarchyDepthColors[0] = new Color(0, 1, 0, 1f);
        hierarchyDepthColors[1] = new Color(1, 0, 0, 1f);
        hierarchyDepthColors[2] = new Color(0, 1, 1, 1f);
        hierarchyDepthColors[3] = new Color(1, 1, 0, 1f);
        hierarchyDepthColors[4] = new Color(1, 0, 1, 1f);
    }

    void HierarchyOnGui(int id, Rect rect)
    {
        var obj = EditorUtility.InstanceIDToObject(id) as GameObject;

        if (obj != null)
        {
            // Do background colors based on parents
            var transform = obj.transform;
            int depth = 0;
            while (transform != null)
            {
                depth++;
                transform = transform.parent;
            }

            if (HighlightTexture == null)
            {
                HighlightTexture = new Texture2D(1, 1);
                HighlightTexture.SetPixel(1, 1, new Color(1, 1, 1, .4f));
                HighlightTexture.Apply();
                HighlightTexture.hideFlags = HideFlags.DontSave;
            }
            for (int i = depth; i > 0; i--)
            {
                Rect depthRect = new Rect(rect.position.x - (i * 14), rect.position.y, 2, 16);
                var oldColor = GUI.color;
                GUI.color = hierarchyDepthColors[(depth - i) % hierarchyDepthColors.Length];
                GUI.DrawTexture(depthRect, HighlightTexture, ScaleMode.StretchToFill, true);
                GUI.color = oldColor;
            }

            float offset = 0;
            foreach (Delegate i in onHierarchyCreateIcon.GetInvocationList())
            {
                var iconRect = new Rect(rect.x + rect.width - 16f - offset, rect.y, 16f, 16f);
                bool res = (bool)i.DynamicInvoke(obj, iconRect);
                if (res == true)
                {
                    offset += 16f;
                }
            }
        }
    }

    bool EnabledButton(GameObject obj, Rect r)
    {
        var style = new GUIStyle(GUI.skin.toggle);
        style.fontSize = 8;
        var color = GUI.color;
        var parentDisabled = false;
        var enabled = false;
        if (obj.activeInHierarchy)
        {
            enabled = true;
        }
        else
        {
            enabled = false;
        }

        if (obj.transform.parent != null && !obj.transform.parent.gameObject.activeInHierarchy)
        {
            GUI.color = new Color(.5f, .5f, .5f);
            parentDisabled = true;
        }

        var changed = GUI.Toggle(r, enabled, "", style) != enabled;
        
        if (changed)
        {
            if (parentDisabled)
            {
                var par = obj.transform.parent;
                var lastPar = obj.transform.parent;
                while (par != null && !par.gameObject.activeInHierarchy)
                {
                    lastPar = par;
                    par = par.transform.parent;
                }
                lastPar.gameObject.SetActive(true);
            }
            else
            {
                obj.SetActive(!obj.activeInHierarchy);
            }
        }
        GUI.color = color;
        return true;
    }

    bool HasMeshFilter(GameObject obj, Rect r)
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        var exists = obj.GetComponent<MeshFilter>() != null || (obj.transform.parent == null && obj.GetComponentInChildren<MeshFilter>() != null);
        if (exists)
        {
            GUI.Box(r, "M", style);
        }
        return exists;
    }

    bool HasCamera(GameObject obj, Rect r)
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        var exists = obj.GetComponent<Camera>() != null || (obj.transform.parent == null && obj.GetComponentInChildren<Camera>() != null);

        if (exists)
        {
            GUI.Box(r, "C", style);
        }
        return exists;
    }

    bool HasLight(GameObject obj, Rect r)
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        var exists = obj.GetComponent<Light>() != null || (obj.transform.parent == null && obj.GetComponentInChildren<Light>() != null);

        if (exists)
        {
            GUI.Box(r, "L", style);
        }
        return exists;
    }

    bool HasNetworkView(GameObject obj, Rect r)
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        var exists = obj.GetComponent<NetworkView>() != null || (obj.transform.parent == null && obj.GetComponentInChildren<NetworkView>() != null);

        if (exists)
        {
            GUI.Box(r, "N", style);
        }
        return exists;
    }
}