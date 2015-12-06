using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GateDoor : MonoBehaviour
{
	float nextSend;
	public GameObject botOri;
	public GameObject groundGameObj;
	public GameObject bots;
	private List<GameObject> lstHoneypot = new List<GameObject> ();
	private float frequency;
	private float dispertion = 0.4f;


	// Use this for initialization
	void Start ()
	{
    gvar.gateDoor = this.gameObject;

		GameObject honeypots = groundGameObj.GetComponent<Ground> ().lstHoneypot;

		for (int i = 0; i < honeypots.transform.childCount; i++) {
			lstHoneypot.Add (honeypots.transform.GetChild (i).gameObject);
		}
		frequency = 1.5f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (gvar.state == enState.Play && gvar.lstVisitor.Count < gvar.level.nbVisitors) 
		{
			if (Time.time > nextSend) {
				Vector3 newPos = this.gameObject.transform.position;
				
				newPos.x += Random.Range (-dispertion, dispertion);
				newPos.z += Random.Range (-dispertion, dispertion);

        Quaternion q = Quaternion.identity;

				
				GameObject newBot = (GameObject)Instantiate (botOri, newPos, q);
        newBot.transform.Rotate(new Vector3(0f,180f,0f));
				gvar.lstVisitor.Add(newBot);
    
				VisitorMove visitorMove = newBot.GetComponent<VisitorMove>();
				visitorMove.number = gvar.lstVisitor.Count;
//				visitorMove.honeypot = lstHoneypot [Random.Range (0, lstHoneypot.Count)];
				newBot.transform.parent = bots.transform;
				nextSend = Time.time + frequency;
			}
		}
	}

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == gvar.tagVisitor)
    {
      VisitorMove v = other.gameObject.GetComponent<VisitorMove>();

      if(gvar.state != enState.Play || (v.lstAttraction.Count == 0 && v.lstAttractionDone.Count > 0))
        Destroy(other.gameObject);
    } 
  }
}
