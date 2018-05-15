using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragile : MonoBehaviour {

    public Vector3 tresholdVelocity;
    public int numberOfCuts;
    private Vector3 previousSpeed;
    private Rigidbody rigid;
    private Vector3 newSpeed;
    private float pointA;
    private List<GameObject> cutPlanes;
	// Use this for initialization
	void Start ()
    {
        previousSpeed = Vector3.zero;
        pointA = 0;
        rigid = transform.GetComponent<Rigidbody>();
        cutPlanes = new List<GameObject>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        pointA += Time.fixedDeltaTime;
        newSpeed = rigid.velocity;
        Vector3 diffence;
        diffence = previousSpeed - newSpeed;
        diffence *= pointA;
        print(diffence);
        if(diffence.x < -tresholdVelocity.x | diffence.x > tresholdVelocity.x)
        {
            if(diffence.y < -tresholdVelocity.y | diffence.y > tresholdVelocity.y)
            {
                Shatter();
            }
        }

        previousSpeed = newSpeed;
	}

    public void Shatter()
    {
        StartCoroutine(CutLine());
    }

    IEnumerator CutLine()
    {
        for(int i = 0; i<numberOfCuts; i++)
        {
            cutPlanes.Add(new GameObject("CutPlane", typeof(BoxCollider), typeof(Rigidbody), typeof(MeshSplitting.Splitters.SplitterSingleCut)));
        }
        foreach(GameObject goCutPlane in cutPlanes)
        {
            goCutPlane.GetComponent<Collider>().isTrigger = true;
            Rigidbody bodyCutPlane = goCutPlane.GetComponent<Rigidbody>();
            bodyCutPlane.useGravity = false;
            bodyCutPlane.isKinematic = true;

            goCutPlane.transform.position = transform.position;
            goCutPlane.transform.rotation = Random.rotation;
        }
        yield return new WaitForEndOfFrame();
    }
}
