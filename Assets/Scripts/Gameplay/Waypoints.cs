using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class Waypoints : MonoBehaviour
    {

        public static List<Transform[]> paths = new List<Transform[]>();

        void Awake()
        {
            foreach (Transform path in transform)
            {
                Transform[] points = new Transform[path.childCount];
                for (int i = 0; i < points.Length; i++)
                {
                    points[i] = path.GetChild(i);
                }
                paths.Add(points);
            }
        }

    }
}
