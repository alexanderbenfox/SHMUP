using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ICollidingEntity : MonoBehaviour
{
    protected BoxCollider2D _collider;
    protected Vector2 _dp = new Vector2(0, 0);
    protected Vector2 _force = new Vector2(0, 0);
    public BoxCollider2D GetCollider() { return _collider; }
    public bool isTrigger = true;
    public bool invertedCollider = false;

    public string type = "";


    public virtual Vector2 GetNextFramePosition()
    {

        return new Vector2(this.transform.position.x + _dp.x + _force.x, this.transform.position.y + _dp.y + _force.y);
    }

    public Rect GetNextRect()
    {
        return new Rect(ToMinimumCorner(GetNextFramePosition()), _collider.size);
    }

    private Vector2 ToMinimumCorner(Vector2 center)
    {
        return new Vector2(center.x - GetCollider().size.x / 2, center.y - GetCollider().size.y / 2);
    }

    public virtual void FinalizeFrame(float dt)
    {
        transform.Translate(_dp + _force);
        _dp = Vector2.zero;
        _force = Vector2.zero;
    }

    public virtual void OnCollide(ICollidingEntity entity)
    {
        //if (!isTrigger)
        //    AdjustForCollision(entity);
    }

    public static bool Collides(ref ICollidingEntity e1, ref ICollidingEntity e2)
    {
        if (e1 == null || e2 == null)
            return false;
        var overlap = e1.GetOverlap(e2);
        return overlap.x != 0 || overlap.y != 0;
    }

    public void AdjustForCollision(ICollidingEntity other)
    {
        Vector2 overlap = GetOverlap(other);
        _force += -overlap;
    }

    Vector2 GetOverlap(ICollidingEntity other)
    {
        Rect r1 = GetNextRect();
        Rect r2 = other.GetNextRect();

        Vector2 dir = (GetNextFramePosition() - new Vector2(this.transform.position.x, this.transform.position.y)).normalized;

        Vector2 overlap = new Vector2(0, 0);
        if (r1.Overlaps(r2))
        {
            if (!other.invertedCollider)
            {
                //collides on the right
                if (dir.x > 0 && r1.xMax > r2.xMin)
                    overlap.x += (r1.xMax - r2.xMin);
                //collides on the left
                if (dir.x < 0 && r1.xMin < r2.xMax)
                    overlap.x -= (r2.xMax - r1.xMin);
                //collides on the top
                if (dir.y > 0 && r1.yMax > r2.yMin)
                    overlap.y += (r1.yMax - r2.yMin);
                //collides on the bottom
                if (dir.y < 0 && r1.yMin < r2.yMax)
                    overlap.y -= (r2.yMax - r1.yMin);
            }
            else
            {
                //collides on the right
                if (dir.x > 0 && r1.xMax > r2.xMax)
                    overlap.x += (r1.xMax - r2.xMax);
                //collides on the left
                if (dir.x < 0 && r1.xMin < r2.xMin)
                    overlap.x -= (r2.xMin - r1.xMin);
                //collides on the top
                if (dir.y > 0 && r1.yMax > r2.yMax)
                    overlap.y += (r1.yMax - r2.yMax);
                //collides on the bottom
                if (dir.y < 0 && r1.yMin < r2.yMin)
                    overlap.y -= (r2.yMin - r1.yMin);
            }
        }
        return overlap;
    }
}
