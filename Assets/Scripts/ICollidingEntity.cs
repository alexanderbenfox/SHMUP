using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ICollidingEntity : MonoBehaviour
{
    protected BoxCollider2D _collider;
    protected Vector2 _dp = new Vector2(0, 0);
    protected Vector2 _force = new Vector2(0, 0);
    protected Vector2 _scaledSize = Vector2.zero;
    public BoxCollider2D GetCollider() { return _collider; }

    //triggers can trigger collisions with other colliders in space
    public bool isTrigger = true;
    //non physics means that it doesnt resolve collisions in space
    public bool nonPhysics = true;

    public bool invertedCollider = false;

    public string type = "";


    public virtual Vector2 GetNextFramePosition()
    {

        return new Vector2(this.transform.position.x + _dp.x + _force.x, this.transform.position.y + _dp.y + _force.y);
    }

    public Rect GetNextRect()
    {
        if(_scaledSize == Vector2.zero)
            _scaledSize = new Vector2(_collider.size.x * this.transform.localScale.x, _collider.size.y * this.transform.localScale.y);
        return new Rect(ToMinimumCorner(GetNextFramePosition()), _scaledSize);
    }

    private Vector2 ToMinimumCorner(Vector2 center)
    {
        return new Vector2(center.x - _scaledSize.x / 2, center.y - _scaledSize.y / 2);
    }

    public virtual void FinalizeFrame(float dt)
    {
        transform.Translate(_dp + _force);
        _dp = Vector2.zero;
        _force = Vector2.zero;
    }

    public virtual void OnCollide(ICollidingEntity entity)
    {
        if (!nonPhysics && !entity.nonPhysics)
            AdjustForCollision(entity);
    }

    public static bool Collides(ref ICollidingEntity e1, ref ICollidingEntity e2)
    {
        if (e1 == null || e2 == null)
            return false;
        return e1.Overlaps(e2);
    }

    public void AdjustForCollision(ICollidingEntity other)
    {
        Vector2 overlap = GetOverlap(other);
        _force += -overlap;
    }

    public bool Overlaps(ICollidingEntity other)
    {
        Rect r1 = GetNextRect();
        Rect r2 = other.GetNextRect();
        if(!invertedCollider && !other.invertedCollider)
        {
            return r1.Overlaps(r2);
        }
        else if(!invertedCollider && other.invertedCollider)
        {
            if (!r1.Overlaps(r2))
                return true;

            var a = r2.Contains(new Vector2(r1.xMax, r1.yMax));
            var b = r2.Contains(new Vector2(r1.xMax, r1.yMin));
            var c = r2.Contains(new Vector2(r1.xMin, r1.yMax));
            var d = r2.Contains(new Vector2(r1.xMin, r1.yMin));
            int pointCount = 0;
            if (a) pointCount++;
            if (b) pointCount++;
            if (c) pointCount++;
            if (d) pointCount++;

            return pointCount < 4;
            
        }
        return false;
    }

    Vector2 GetOverlap(ICollidingEntity other)
    {
        Rect r1 = GetNextRect();
        Rect r2 = other.GetNextRect();

        Vector2 dir = (GetNextFramePosition() - new Vector2(this.transform.position.x, this.transform.position.y)).normalized;

        Vector2 overlap = new Vector2(0, 0);
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
        return overlap;
    }
}
