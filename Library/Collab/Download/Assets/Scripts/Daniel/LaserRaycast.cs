using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRaycast : MonoBehaviour {
	public LineRenderer line;

	public Rect Box;
    public float Distance;
    public RaycastHit2D[] hits;
    public LayerMask mask;
    public float RotationSpeed;
	public List<Transform> list = new List<Transform>();
	public Transform target;

	public bool debug;


	void Start(){
		line = GetComponent<LineRenderer>();
		InitLineRenderer();
	}

	public void InitLineRenderer(){
		line.enabled = true;
		line.widthMultiplier = Box.height;
	}
	public void UpdateLineRenderer(){
		line.SetPosition(0, transform.position);
		line.SetPosition(1, target.position);
	}

    void OnDrawGizmos()
    {
		if(debug){
			if(hits != null)
			{
				foreach(var h in hits)
				{
					Gizmos.color = Color.green;
					Gizmos.DrawCube(h.collider.transform.position, Vector2.one * 0.16f);            
				}
			}
		}
    }

	void Update () {
		UpdateLineRenderer();
		list.Clear();
		if(hits != null){
			foreach(RaycastHit2D x in hits){
				list.Add(x.transform);
			}
		}

        transform.Rotate ( Vector3.forward * ( RotationSpeed * Time.deltaTime ));

        hits = Physics2D.BoxCastAll(
            (Vector2)this.transform.position, 
            Box.size, 
            this.transform.eulerAngles.z, 
            transform.right, 
            Distance, 
            mask);

		RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, 20, mask);
		if(hit){
			if(hit.transform.tag == "Player"){
				//hit.transform.gameObject.GetComponent<PlayerController>().TakeDamage(1);
			}
			Debug.DrawLine(transform.position, hit.point);
			target.position = hit.point;
			line.SetPosition(0, transform.position);
			line.SetPosition(1, new Vector3(hit.point.x,hit.point.y,0));
		} else {
			Debug.DrawLine(transform.position, target.position);
			line.SetPosition(0, transform.position);
			line.SetPosition(1, new Vector3(target.position.x,target.position.y,0));
		}
    }


}
