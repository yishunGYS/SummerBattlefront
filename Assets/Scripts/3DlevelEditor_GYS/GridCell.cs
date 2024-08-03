using UnityEngine;

namespace _3DlevelEditor_GYS
{
    public class GridCell: MonoBehaviour
    {
        public GridCell previousCell;
        public GridCell nextCell;

        private void OnDrawGizmos()
        {
            if (previousCell != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, previousCell.transform.position);
            }
            if (nextCell != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, nextCell.transform.position);
            }
        }
    }
}