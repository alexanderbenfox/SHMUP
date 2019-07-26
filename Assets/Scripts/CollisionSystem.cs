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
        {
            Debug.Log("Adding " + entity.name + " to the collision system.");
            _entities.Add(entity);
        }
    }

    public void AddEntity(ref ICollidingEntity entity)
    {
        _entities.Add(entity);
    }

    public void RemoveEntity(ref ICollidingEntity entity)
    {
        _entities.Remove(entity);
    }

    public void DestroyEntity(ref ICollidingEntity entity)
    {
        _entities.Remove(entity);
        Destroy(entity.gameObject);
    }

    public void CheckFrame(float dt)
    {
        for(int i = 0; i < _entities.Count; i++)
        {
            ICollidingEntity entity1 = _entities[i];
            //triggers dont cause collisions
            if (entity1.isTrigger)
            {
                for (int j = 0; j < _entities.Count; j++)
                {
                    if (i != j)
                    {
                        ICollidingEntity entity2 = _entities[j];
                        if (ICollidingEntity.Collides(ref entity1, ref entity2))
                        {
                            Debug.Log("Collision between " + entity1.name + " and " + entity2.name);
                            entity1.OnCollide(entity2);
                            //entity2.OnCollide(entity1);
                        }
                    }
                }
            }
            entity1.FinalizeFrame(dt);
        }
    }
}
