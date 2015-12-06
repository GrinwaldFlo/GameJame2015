using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisitorMove : MonoBehaviour
{
  public int number;
  public float repulsionVisitor = -1f;
  public float repulsionObstacle = -1f;
  public float repulsionDistance = 1f;
  public float attractionAttraction = 5f;
  public float vLinMax = 1f;
  public float vAngMax = 1f;
  public float aLin = 0.1f;
  public float aAng = 0.1f;
  public float vAng;
  public float vLin;
  public float angleDif;
  public bool isSelected = false;
  public GameObject asset;
  public GameObject questionMark;
  private Material cap;
  private Material shirt;
  private Material pants;

  public List<enAttribute> lstNeeds = new List<enAttribute>();
  public List<enAttribute> lstNeedsOK = new List<enAttribute>();
  public List<Attraction> lstAttractionDone = new List<Attraction>();
  public List<GameObject> lstVisitor = new List<GameObject>();
  public List<GameObject> lstObstacle = new List<GameObject>();
  public List<Attraction> lstAttraction = new List<Attraction>();
  public string thought = "";
  //private Animator animator;
  private Vector3 tmpVector;
  private Vector3 vResult;
  private Vector3 cross;

  // Use this for initialization
  void Start()
  {
    gvar.shuffleAttribute();
    for (int i = 0; i < gvar.nbNeeds; i++)
    {
      lstNeeds.Add(gvar.lstAttribute [i]);
    }
    gvar.needsSum += lstNeeds.Count;
    //animator = transform.GetChild(0).GetComponent<Animator>();

    //shirt = transform.GetChild(1).GetChild(0).GetComponent<Shader>();
    GameObject g = asset.transform.GetChild(0).gameObject;
    shirt = g.renderer.material;
    shirt.color = new Color(getRnd0_1(), getRnd0_1(), getRnd0_1());
    g = asset.transform.GetChild(0).GetChild(4).gameObject;
    cap = g.renderer.material;
    cap.color = new Color(getRnd0_1(), getRnd0_1(), getRnd0_1());
  }

  float getRnd0_1()
  {
    return Mathf.Round(Random.Range(0f, 1f));
  }


  // Update is called once per frame
  void Update()
  {
    //animator.StartPlayback();
    if (transform.position.y < -0.8f && this.particleSystem.isPlaying == false)
    {
      Destroy(this.gameObject);
    }
    if (transform.position.y < -0.5f && this.particleSystem.isPlaying == false)
    {
      this.particleSystem.Play();
    }
    if (transform.position.y < -0.2f)
    {
      transform.localScale *= 0.95f;
    }

    if (asset != null)
    {
      float var = 1f + (Mathf.PingPong(Time.time * 0.5f, 0.5f));
      
      if (isSelected)
      {
        asset.transform.localScale = new Vector3(var, var, var);
      } else
      {
        asset.transform.localScale = new Vector3(1f, 1f, 1f);
      }
    }
  }

  void FixedUpdate()
  {
    vResult = new Vector3();
    //forceWall();
    forceObstacle();
    forceAttraction();
    forceBot();

    angleDif = Vector3.Angle(vResult, transform.forward);

    if (angleDif < 45f && rigidbody.velocity.sqrMagnitude < vLinMax)
      rigidbody.AddForce(transform.forward * aLin, ForceMode.Acceleration);

    cross = Vector3.Cross(vResult, transform.forward);

    vAng = rigidbody.angularVelocity.y;
    vLin = rigidbody.velocity.sqrMagnitude;
    if (Mathf.Abs(vAng) < vAngMax)
    if (cross.y > 0f)
      rigidbody.AddTorque(0f, -aAng, 0f, ForceMode.Acceleration);
    else
      rigidbody.AddTorque(0f, aAng, 0f, ForceMode.Acceleration);
  }

  /// <summary>
  /// Adds the force.
  /// </summary>
  /// <param name="factor">Positive = obstacle, negative = honypot</param>
  /// <param name="item">Item.</param>
  /// <param name="maxDist">Max dist.</param>
  void addForce(float factor, Transform item, float maxDist)
  {
    tmpVector = new Vector3(rigidbody.position.x - item.position.x, 0f, rigidbody.position.z - item.position.z);
    float a = maxDist;
    float b =  tmpVector.magnitude;
    if(b > a)
    {
      b = a;
    }
    float r = (a - b) / a;
    
    tmpVector.Normalize();
    tmpVector = tmpVector * r * factor;
    
    vResult.x -= tmpVector.x;
    vResult.z -= tmpVector.z;
  }

  void forceObstacle()
  {
    foreach (GameObject item in lstObstacle)
    {
      addForce(repulsionObstacle, item.transform, 2.5f);
    }

    if (lstAttractionDone.Count > 0)
    {
      addForce(-3f, lstAttractionDone[0].transform, repulsionDistance);
    }
  }

  void forceBot()
  {
    int i = 0;
    while (i < lstVisitor.Count)
    {
      if (lstVisitor [i] != null)
      {
        addForce(repulsionVisitor, lstVisitor[i].transform, 2f);
        i++;
      } else
      {
        lstVisitor.RemoveAt(i);
      }
    }
  }

  void forceAttraction()
  {
    if (lstNeeds.Count == 0 || gvar.state == enState.EndLoose || gvar.state == enState.EndWin)
    {
      tmpVector = new Vector3(rigidbody.position.x - gvar.gateDoor.transform.position.x, 0f, 
                              rigidbody.position.z - gvar.gateDoor.transform.position.z);
      tmpVector.Normalize();
      vResult.x -= tmpVector.x;
      vResult.z -= tmpVector.z;
      thought = "Can’t wait to get back home!";
      questionMark.SetActive(false);
    } else
    {
      for (int i = 0; i < lstAttraction.Count; i++) 
      {
        addForce(attractionAttraction, lstAttraction[i].transform, lstAttraction[i].getDistanceAttraction());
      }
      if(lstAttraction.Count > 0)
      {
        questionMark.SetActive(false);
        thought = lstAttraction[0].txtTought;
      }
      else
      {
        questionMark.SetActive(true);
        thought = "What do we do now?";
      }
    }
  }

  void updateNeeds(List<enAttribute> lstAttribute)
  {
    for (int i = 0; i < lstNeeds.Count; i++)
    {
      foreach (var item in lstAttribute)
      {
        if (lstNeeds [i] == item)
        {
          gvar.needsOK++;
          lstNeedsOK.Add(lstNeeds [i]);
          lstNeeds.RemoveAt(i);
          gvar.sound.askPlay(soundPlayer.enSound.checkedLike);
          i--;
          break;
        }
      }
    }
  }

  void OnTriggerEnter(Collider other)
  {
    Attraction curAttraction = null;
    if (other.gameObject.tag == gvar.tagAttractionVisit || other.gameObject.tag == gvar.tagAttractionZone)
      curAttraction = other.gameObject.transform.parent.gameObject.GetComponent<Attraction>();

    if (other.gameObject.tag == gvar.tagVisitor)
    {
      lstVisitor.Add(other.gameObject);
    } else if (other.gameObject.tag == gvar.tagObstacle)
    {
      lstObstacle.Add(other.gameObject);
    } else if (other.gameObject.tag == gvar.tagAttractionVisit)
    {
      if (lstAttractionDone.Count == 0 || lstAttractionDone [0] != curAttraction)
      {
        lstAttractionDone.Insert(0, curAttraction);
        updateNeeds(curAttraction.lstAttribute);
        lstAttraction.Remove(curAttraction);
      }
    } else if (other.gameObject.tag == gvar.tagAttractionZone)
    {
      if ((lstAttractionDone.Count == 0 || lstAttractionDone [0] != curAttraction) && gvar.findNeeds(lstNeeds, curAttraction))
      {
        foreach (var item in lstAttraction) 
        {
          if(item == curAttraction)
            return;
        }
        lstAttraction.Add(curAttraction);
      }
    }
  }

  void OnTriggerExit(Collider other)
  {
    if (other.gameObject.tag == gvar.tagVisitor)
    {
      lstVisitor.Remove(other.gameObject);
    } else if (other.gameObject.tag == gvar.tagObstacle)
    {
      lstObstacle.Remove(other.gameObject);
    } else if (other.gameObject.tag == gvar.tagAttraction)
    {
      lstObstacle.Remove(other.gameObject);
    } else if (other.gameObject.tag == gvar.tagAttractionVisit)
    {
      
    } else if (other.gameObject.tag == gvar.tagAttractionZone)
    {
      lstAttraction.Remove(other.gameObject.GetComponent<Attraction>());
    }
  }
}
