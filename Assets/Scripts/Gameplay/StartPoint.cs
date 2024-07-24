using Gameplay.Player;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay
{
    public class StartPoint : MonoBehaviour
    {
        private Color startColor;
        private Renderer rend;

        public Color hoverColor;
        [Header("路径编号")]
        public int pathNum;

        void Start()
        {
            rend = GetComponent<Renderer>();
            startColor = rend.material.color;
        }

        void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            rend.material.color = hoverColor;
        }

        void OnMouseExit()
        {
            rend.material.color = startColor;
        }

        private void OnMouseDown()
        {
            SpawnManager.instance.ChangeSpawnPoint(this.transform);
            SpawnManager.instance.SetPathNum(pathNum);
            SpawnManager.instance.SpawnCharacter();
        }
    }
}
