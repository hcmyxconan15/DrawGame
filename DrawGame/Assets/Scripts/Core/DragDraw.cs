using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MG_Draw
{
    public class DragDraw : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public GameObject PrefabLine;
        LineRenderer line;
        Camera camera;
        private int index = 0;
        public List<LineRenderer> lines = new List<LineRenderer>();
        List<Vector2> points = new List<Vector2>();
        EdgeCollider2D collider;
        public ContactFilter2D contactFilter;
        public float time;
        private void Awake()
        {
            camera = Camera.main;

        }
        public void OnBeginDrag(PointerEventData pointerEvent)
        {
            if (CheckCollistion(pointerEvent.position)) return;
            line = Instantiate(PrefabLine).GetComponent<LineRenderer>();
            collider = line.gameObject.GetComponent<EdgeCollider2D>();
            Vector3 point = ConvertWorldPosition(pointerEvent.position);
            line.gameObject.transform.position = point;
            line.SetPosition(0, point);
            points.Add((Vector2)point);
        }

        public void OnDrag(PointerEventData pointerEvent)
        {
            if (line == null) return;
            if (time <= 0.05f) return;
            index++;
            line.positionCount++;
            Vector3 point = ConvertWorldPosition(pointerEvent.position);
            line.SetPosition(index, point);
            points.Add((Vector2)point);
            time = 0;
        }
        public void OnEndDrag(PointerEventData pointerEvent)
        {
            if (points.Count == 0) return;
            index = 0;
            line.gameObject.transform.position = Vector3.zero;
            line.useWorldSpace = false;
            collider.SetPoints(points);
            lines.Add(line);
            Publisher.Instant.Broadcast(EnumObsever.Default);
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
        public void TimeWait()
        {
            time += Time.deltaTime * 1f;
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
            TimeWait();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RemoveLine();
            }
        }
    }
}

