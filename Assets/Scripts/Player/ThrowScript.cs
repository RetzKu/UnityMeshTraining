using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    AXE,
    SWORD,
    SPEAR,
    LANCE,
}

public class ThrowScript : MonoBehaviour {

    public WeaponType Type;
    private Rigidbody Weapon;
    public float MaxLifetime;

    private Vector3 ImpactPoint;
    private Vector3 PushForce;

	void Awake()
    {
        Weapon = transform.GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    public void Throw(Vector2 To,float Force)
    {
        Weapon.AddForce(To * Force);
        Weapon.AddTorque(new Vector3(0,0,-1) * Force);
        Debug.Log("Player Threw weapon Towards" + To);
        StartCoroutine(Reset());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(ImpactPoint,PushForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Breakable")
        {

            if(Weapon.velocity.x > 3 || Weapon.velocity.y > 3)
            {
                ImpactPoint = collision.contacts[0].point;
                PushForce = (collision.transform.position - ImpactPoint).normalized;
                PushForce += ImpactPoint + Weapon.velocity.normalized;
                Weapon.velocity = Weapon.velocity / 2;
                CreatePlane(ImpactPoint, PushForce);
                Debug.Log(Weapon.velocity);
            }
        }
    }

    void CreatePlane(Vector2 from, Vector2 to)
    {
        Vector3 startPos = from;
        Vector3 endPos = to;

        Vector3 center = Vector3.Lerp(startPos, endPos, .5f);
        Vector3 cut = (endPos - startPos).normalized;

        
        GameObject goCutPlane = new GameObject("CutPlane", typeof(BoxCollider), typeof(Rigidbody), typeof(MeshSplitting.Splitters.SplitterSingleCut));

        goCutPlane.GetComponent<Collider>().isTrigger = true;
        Rigidbody bodyCutPlane = goCutPlane.GetComponent<Rigidbody>();
        bodyCutPlane.useGravity = false;
        bodyCutPlane.isKinematic = true;

        Transform transformCutPlane = goCutPlane.transform;
        transformCutPlane.position = center;
    }

    IEnumerator Reset()
    {
        bool notmoving = false;
        float age = 0;

        while(!notmoving)
        {
            yield return new WaitForSeconds(0.1f);
            age += 0.1f;
            if (Weapon.velocity.x == 0 && Weapon.velocity.y == 0)
            {
                notmoving = true;
            }
            if (age >= MaxLifetime)
            {
                notmoving = true;
            }
        }
        Destroy(transform.gameObject, 0.5f);
    }
}
