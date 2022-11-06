using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG_Draw
{
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(EdgeCollider2D))]
    public class Line : MonoBehaviour
    {
        public bool CantDraw => cantDraw;
        private bool cantDraw = false;
        private Rigidbody2D rigidbody2D => GetComponent<Rigidbody2D>();
        private LineRenderer lineRenderer => GetComponent<LineRenderer>();
        private EdgeCollider2D edgeCollider2D => GetComponent<EdgeCollider2D>();
        private List<Vector2> points = new List<Vector2>();
        private float distance;
        [SerializeField] private float radius;
        private int count;

        public void AddPoint(Vector2 point)
        {
            if (CantDraw) return;
            if (count >=1 &&CheckDistance(GetLastPoint().Value, point)) return;
            count++;
            lineRenderer.positionCount = count;
            lineRenderer.SetPosition(count - 1, point);
            points.Add(point);


            CircleCollider2D circle = this.gameObject.AddComponent<CircleCollider2D>();
            circle.offset = point;
            circle.radius = radius;
            
            if (count >1)
                edgeCollider2D.SetPoints(points);
        }
        public void SetDraw()
        {
            cantDraw = true;
            this.gameObject.layer = LayerMask.NameToLayer("CantDrawLayer");
        }
        public void SetPointMinDistance(float _distance)
        {
            this.distance = _distance;
        }
        
        public void UsePhysic(bool usePhysic)
        {
            if (cantDraw) return;
            rigidbody2D.isKinematic = !usePhysic;
        }
        private bool CheckDistance(Vector2 from, Vector2 to)
        {
            return Vector2.Distance(from, to) <= distance;
        }
        
        private Vector2? GetLastPoint()
        {
            if (points.Count <= 0) return null;

            return points[count - 1];
        }
    }

}

