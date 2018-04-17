using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamic2DCollider : MonoBehaviour {

    public List<PolygonCollider2D>dynamicCollider;
    public GameObject parentTransform;
	// Use this for initialization
	void Start ()
    {
        parentTransform = transform.parent.gameObject;
        transform.rotation = new Quaternion(-90, 0, 0, 0);
        UpdateCollider();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    	
	}

    public void UpdateCollider()
    {
        PolygonCollider2D polCollider;
        PolygonCollider2D og = transform.GetComponent<PolygonCollider2D>();
        if(og != null)
        {
            polCollider = og;
        }
        else
        {
            polCollider = gameObject.AddComponent<PolygonCollider2D>();
        }
        dynamicCollider.Add(polCollider);

        MeshCollider meshData = parentTransform.GetComponent<MeshCollider>();
        Vector2[] newpoints = GetLimits(meshData.sharedMesh.vertices);

        polCollider.SetPath(0, newpoints);
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