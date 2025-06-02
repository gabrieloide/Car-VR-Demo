using UnityEngine;
using TMPro;

public static class DebugUI
{
    public static void CreateLogBox(out TextMeshPro modifyText)
    {
        var debugUI = GameObject.Find("VerticalLayout");

        var debugText = GameObject.Instantiate(Resources.Load<GameObject>("DebugText"), debugUI.transform);
        
        if(debugText == null)
        {
            Debug.LogError("DebugText not found");
            modifyText = null;
            return;
        }
        modifyText = debugText.GetComponent<TextMeshPro>();
    }
}
