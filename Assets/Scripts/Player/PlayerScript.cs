using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public GameObject ThrowableObject;

    /*Vectors for mousedragging and for applying force*/
    private Vector3 StartPosition;
    private Vector3 EndPosition;
    private bool Dragging = false;

	void Start ()
    {
	}
	
	void FixedUpdate ()
    {
        if(Input.GetMouseButton(0) == true && Dragging == false)
        {
            Dragging = true;
            ThrowableObject.transform.position = transform.position;
            StartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            StartCoroutine(DragDistance());
        }
    }
    IEnumerator DragDistance()
    {
        while(Dragging)
        {
            if(Input.GetMouseButton(0) == false)
            {
                EndPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Dragging = false;
            }
            yield return new WaitForFixedUpdate();
        }
        Vector3 Towards = (EndPosition - transform.position).normalized;
        float Force = Vector3.Distance(StartPosition, EndPosition);
        var n = 90 - (Mathf.Atan2(Towards.y, Towards.x)) * 180 / Mathf.PI;
        Debug.Log(n);
        Instantiate(ThrowableObject).GetComponent<ThrowScript>().Throw(Towards,(Force *200));
    }
}
