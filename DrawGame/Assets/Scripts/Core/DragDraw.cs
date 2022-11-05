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
        private LineRenderer line;
        private Camera camera;
        private int index = 0;
        public List<LineRenderer> lines = new List<LineRenderer>();
        private List<Vector2> points = new List<Vector2>();
        private EdgeCollider2D collider;
        private int cout;
        private Rigidbody2D rigidbody2D;
        
        private void Awake()
        {
            camera = Camera.main;
            cout = 0;

        }
        public void OnBeginDrag(PointerEventData pointerEvent)
        {
            if (CheckCollistion(pointerEvent.position)) return;
            line = Instantiate(PrefabLine).GetComponent<LineRenderer>();
            collider = line.gameObject.GetComponent<EdgeCollider2D>();
            Vector3 point = ConvertWorldPosition(pointerEvent.position);
            rigidbody2D = line.GetComponent<Rigidbody2D>();
            CircleCollider2D cicle = line.gameObject.AddComponent<CircleCollider2D>();
            cicle.radius = 0.025f;
            cicle.offset = point;
            line.gameObject.transform.position = point;
            line.SetPosition(0, point);
            points.Add((Vector2)point);
            cout++;
        }

        public void OnDrag(PointerEventData pointerEvent)
        {
            Vector3 point = ConvertWorldPosition(pointerEvent.position);
            if (CheckDistance(points[cout-1], point)) return;
            index++;
            line.positionCount++;
            CircleCollider2D cicle = line.gameObject.AddComponent<CircleCollider2D>();
            cicle.radius = 0.025f;
            cicle.offset = point;
            line.SetPosition(index, point);
            points.Add((Vector2)point);
            if (cout > 1) collider.SetPoints(points);
            cout++;
        }
        public void OnEndDrag(PointerEventData pointerEvent)
        {
            if (cout <= 1) return;
            index = 0;
            line.gameObject.transform.position = Vector3.zero;
            line.useWorldSpace = false;
            lines.Add(line);
            cout = 0;
            //Publisher.Instant.Broadcast(EnumObsever.Default);
            points.Clear();
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
        public bool CheckDistance(Vector2 from, Vector2 to)
        {
            return Vector2.Distance(from, to) <= distanceMin;
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

