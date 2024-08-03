using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditoeTest2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        
        Vector3 mousePosition = Event.current.mousePosition ;
        Debug.Log(mousePosition);
        //Debug.Log(Camera.main.ScreenToWorldPoint(mousePos));
    }
}
