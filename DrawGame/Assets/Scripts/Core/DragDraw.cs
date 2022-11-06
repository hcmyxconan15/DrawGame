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
        public LayerMask CantDrawLayer;
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
            if (CheckPhysicsPlayer(point, CantDrawLayer))
            {
                OnEndDrag(pointerEvent);
                return;
            }
            line.AddPoint(point);
            
        }
        public void OnEndDrag(PointerEventData pointerEvent)
        {
            line.UsePhysic(true);
            line.SetDraw();
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

        public bool CheckPhysicsPlayer(Vector2 point, LayerMask layer)
        {
            RaycastHit2D hit = Physics2D.CircleCast(point, 0.025f, Vector2.zero, 1f, layer);
            return hit.collider != null ? true : false;
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

