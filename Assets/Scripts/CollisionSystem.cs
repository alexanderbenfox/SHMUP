using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem : MonoBehaviour
{
    private List<ICollidingEntity> _entities;

    public void Init()
    {
        _entities = new List<ICollidingEntity>();
        ICollidingEntity[] arr = GameObject.FindObjectsOfType<ICollidingEntity>();
        foreach (ICollidingEntity entity in arr)
            _entities.Add(entity);
    }

    public void AddEntity(ref ICollidingEntity entity)
    {
        _entities.Add(entity);
    }

    public void CheckFrame()
    {
        for(int i = 0; i < _entities.Count; i++)
        {
            ICollidingEntity entity1 = _entities[i];
            for(int j = 0; j < _entities.Count; j++)
            {
                if(i != j)
                {
                    ICollidingEntity entity2 = _entities[j];
                    if(ICollidingEntity.Collides(ref entity1, ref entity2))
                    {
                        entity1.AdjustForCollision(entity2);
                        entity1.OnCollide(entity2);
                        entity2.OnCollide(entity1);
                    }
                }
            }
            entity1.FinalizeFrame();
        }
    }
}
