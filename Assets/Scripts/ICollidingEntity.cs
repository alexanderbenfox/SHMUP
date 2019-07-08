using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ICollidingEntity : MonoBehaviour
{
    protected BoxCollider2D _collider;
    protected Vector2 _dp;
    public BoxCollider2D GetCollider() { return _collider; }


    public virtual Vector2 GetNextFramePosition()
    {
        return new Vector2(this.transform.position.x + _dp.x, this.transform.position.y + _dp.y);
    }

    public virtual void FinalizeFrame()
    {
        transform.Translate(_dp);
        _dp = Vector2.zero;
    }

    public virtual void OnCollide(ICollidingEntity entity)
    {}

    public static bool Collides(ref ICollidingEntity e1, ref ICollidingEntity e2)
    {
        Rect r1 = new Rect(e1.GetNextFramePosition(), e1.GetCollider().size);
        Rect r2 = new Rect(e2.GetNextFramePosition(), e2.GetCollider().size);
        return r1.Overlaps(r2);
    }

    public void AdjustForCollision(ICollidingEntity other)
    {
        Rect r1 = new Rect(GetNextFramePosition(), GetCollider().size);
        Rect r2 = new Rect(other.GetNextFramePosition(), other.GetCollider().size);

        Vector2 dir = (GetNextFramePosition() - new Vector2(transform.position.x, this.transform.position.y)).normalized;
        Vector2 adjustment = Vector2.zero;

        if (dir.x > 0 && r1.xMax > r2.xMin)
            adjustment += new Vector2(r1.xMax - r2.xMin, 0);
        if (dir.x < 0 && r1.xMin > r2.xMax)
            adjustment += new Vector2(r1.xMin - r2.xMax, 0);
        if (dir.y > 0 && r1.yMax > r2.yMin)
            adjustment += new Vector2(0, r1.yMax - r2.yMin);
        if (dir.y < 0 && r1.yMin > r2.yMax)
            adjustment += new Vector2(0, r1.yMin - r2.yMax);

        _dp += adjustment;
    }
}
