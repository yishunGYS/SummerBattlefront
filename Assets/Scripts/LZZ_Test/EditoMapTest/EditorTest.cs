using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[ExecuteInEditMode]
public class EditorTest : Editor
{
    public Transform floor;
    
#if UNITY_EDITOR
    public GameObject GOTarget;


    private void Update()
    {
        
    }

    [Obsolete("Obsolete")]
    void OnSceneGUI()
    {
        GOTarget = GameObject.Find("Capsule");
        if (Event.current.type == EventType.mouseDown)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject obj = (GameObject)GameObject.Instantiate(GOTarget,hit.point,GOTarget.transform.rotation);
            }
        }
    }


#endif
    
}


