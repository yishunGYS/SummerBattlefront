using System.Collections;
using System.Collections.Generic;
using Systems.Level;
using UnityEngine;

public class TestLevelEnd : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LevelManager.Instance.LevelEnd();
        }
    }
}
