using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Dynamic2DCollider : MonoBehaviour {

    public GameObject parentTransform;
    public PolygonCollider2D poly;
    private Mesh parentMesh;
    private MeshCollider parentCollider;

    void Start ()
    {
        parentTransform = transform.parent.gameObject;
        //UpdateCollider();
        InitSplit();
        poly = transform.GetComponent<PolygonCollider2D>();
        transform.GetComponent<Rigidbody2D>().gravityScale = 0;
        poly.isTrigger = true; 
	}
	
	void Update ()
    {
        UpdateCollider();
        if(Input.GetKeyDown(KeyCode.A) == true) //Debug checki jos jokin hajoaa;
        {
        }
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        parentTransform.transform.SetParent(transform);
        transform.GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    public void InitSplit()
    {
        parentTransform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX |RigidbodyConstraints.FreezeRotationZ;
        StartCoroutine(WaitForCreation());
        parentTransform.GetComponent<MeshCollider>().convex = true; 
        transform.position = Vector3.zero;
    }
    private IEnumerator WaitForCreation()
    {
        yield return new WaitForFixedUpdate();
        poly = transform.GetComponent<PolygonCollider2D>();
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(1, 1, 1);
        transform.Rotate(180, 0, 0);
    }
    public void UpdateCollider()
    {
        if(poly == null)
        {
            poly = transform.GetComponent<PolygonCollider2D>();
        }
        MeshCollider meshData = parentTransform.GetComponent<MeshCollider>();
        Vector2[] newpoints = GetLimits(meshData.sharedMesh.vertices);
        poly.SetPath(0, newpoints);
    }

    Vector2[] GetLimits(Vector3[] points)
    {
        Vector2[] corners = new Vector2[4];

        /*UpLeft Corner search*/
        float highscore = 0;
        foreach (Vector3 point in points)
        {
            float score = 0;
            score += point.z;
            score -= point.x;
            if (score > highscore)
            {
                highscore = score;
                corners[0].x = point.x;
                corners[0].y = point.z;
            }
        }
        highscore = 0;
        /*Up Right*/
        foreach (Vector3 point in points)
        {
            float score = 0;
            score += point.z;
            score += point.x;
            if (score > highscore)
            {
                highscore = score;
                corners[1].x = point.x;
                corners[1].y = point.z;
            }
        }
        highscore = 0;
        /*Down Right*/
        foreach (Vector3 point in points)
        {
            float score = 0;
            score -= point.z;
            score += point.x;
            if (score > highscore)
            {
                highscore = score;
                corners[2].x = point.x;
                corners[2].y = point.z;
            }
        }
        highscore = 0;
        /*Down Left*/
        foreach (Vector3 point in points)
        {
            float score = 0;
            score -= point.z;
            score -= point.x;
            if (score > highscore)
            {
                highscore = score;
                corners[3].x = point.x;
                corners[3].y = point.z;
            }
        }
        return corners;
    }
} 