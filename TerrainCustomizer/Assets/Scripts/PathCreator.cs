using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathCreator : MonoBehaviour {

    private LineRenderer lineRenderer;
    public Camera drawCam;
    private List<Vector3> points = new List<Vector3>();
        
    // Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
           // points.Clear();

        if (Input.GetButton("Fire1"))
        {

            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Ray ray = drawCam.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

             if (Physics.Raycast(ray, out hitInfo))
             {
                 if (hitInfo.collider != null && (hitInfo.collider.GetType() == typeof(MeshCollider)) && DistanceToLastPoint(hitInfo.point) > 0.2f)
                 {
                     points.Add(hitInfo.point);

                     lineRenderer.positionCount = points.Count;
                     lineRenderer.SetPositions(points.ToArray());

                 }
             }



            //points.Add(drawCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, drawCam.nearClipPlane)));
//            points.Add(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20));
  //          lineRenderer.positionCount = points.Count;
    //        lineRenderer.SetPositions(points.ToArray());
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            // done with the path
        }
            
	}

    public List<Vector3> getPath()
    {
        return points;
    }

    private float DistanceToLastPoint(Vector3 point)
    {
        if (!points.Any())
        {
            return Mathf.Infinity;
        } else
        {
            return Vector3.Distance(points.Last(), point);
        }
    }
}
