using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MG_Draw
{
    public class DragDraw : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public GameObject PrefabLine;
        public float distanceMin;
        private Line line;
        private Camera camera;
        private List<Line> lines = new List<Line>();
        private Rigidbody2D rigidbody2D;
        
        private void Awake()
        {
            camera = Camera.main;

        }
        public void OnBeginDrag(PointerEventData pointerEvent)
        {
            Vector3 point = ConvertWorldPosition(pointerEvent.position);
            line = Instantiate(PrefabLine).GetComponent<Line>();
            line.SetPointMinDistance(distanceMin);
            line.UsePhysic(false);
            line.AddPoint(point);
        }

        public void OnDrag(PointerEventData pointerEvent)
        {
            Vector3 point = ConvertWorldPosition(pointerEvent.position);
            line.AddPoint(point);
            
        }
        public void OnEndDrag(PointerEventData pointerEvent)
        {
            line.UsePhysic(true);
            lines.Add(line);

        }
        public Vector3 ConvertWorldPosition(Vector3 pos)
        {
            Vector3 vector = camera.ScreenToWorldPoint(pos);
            return new Vector3(vector.x, vector.y, 0);
        }

        public bool CheckCollistion(Vector3 pos)
        {
            Vector2 vector = ConvertWorldPosition(pos);
            RaycastHit2D hit = Physics2D.Raycast(vector, Vector2.zero);
            if (hit.collider != null)
            {
                return true;
            }
            return false;

        }

        public void RemoveLine()
        {
            foreach (var item in lines)
            {
                Destroy(item.gameObject);
            }
            lines.Clear();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RemoveLine();
            }
        }
    }
}

