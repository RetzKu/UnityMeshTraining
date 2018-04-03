using MeshSplitting.Splitables;
using UnityEngine;

namespace MeshSplitting.Splitters
{
    //[AddComponentMenu("Mesh Splitting/Splitter")]
    [RequireComponent(typeof(Collider2D))]
    public class Splitter2D : MonoBehaviour
    {
        protected Transform _transform;

        protected virtual void Awake()
        {
            _transform = GetComponent<Transform>();
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            MonoBehaviour[] components = other.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour component in components)
            {
                ISplitable splitable = component as ISplitable;
                if (splitable != null)
                {
                    //Impactpo
                    /*
                    ImpactPoint = collision.contacts[0].point;
                    PushForce = (collision.transform.position - ImpactPoint).normalized;
                    PushForce += ImpactPoint + Weapon.velocity.normalized;
                    Weapon.velocity = Weapon.velocity / 2;
                    CreatePlane(ImpactPoint, PushForce);
                    */
                    SplitObject(splitable, other.gameObject);
                    break;
                }
            }
        }

        protected virtual void SplitObject(ISplitable splitable, GameObject go)
        {
            splitable.Split(_transform);
        }
    }
}