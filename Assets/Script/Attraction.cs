using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attraction : MonoBehaviour
{
  public bool boostedAtStart = false;
  public string title;
  public string txtTought;
  public GameObject asset;
  public List<enAttribute> lstAttribute = new List<enAttribute>();
  private GameObject cylinderZone = null;
  GameObject attractionZone;
  SphereCollider curCollider;
  float curScale;
  bool isBoosted;

  // Use this for initialization
  void Start()
  {
    uiManager.onStart += onStart;

    attractionZone = this.transform.FindChild(gvar.childNameAttractionZone).gameObject;
    curCollider = attractionZone.GetComponent<SphereCollider>();
    cylinderZone = transform.FindChild(gvar.childNameCylinderZone).gameObject;


  }

  void onStart()
  {
    if (boostedAtStart)
      boost(1);
    else
      boost(0);
  }

  public void boost(int param)
  {
    switch (param)
    {
      case 0:
        isBoosted = false;
        curCollider.radius = gvar.attractionStdZone;
        break;
      case 1:
        isBoosted = true;
        curCollider.radius = gvar.attractionBoost1;
        if(boostedAtStart)
          curCollider.radius *=2f;
        break;
      default:
        break;
    }
  }

  public void highlight(bool value)
  {
    cylinderZone.transform.localScale = new Vector3(curCollider.radius * 2, 0f, curCollider.radius * 2);
    cylinderZone.SetActive(value);
  }

  public float getDistanceAttraction()
  {
    return curCollider.radius;
  }
    
  // Update is called once per frame
  void Update()
  {
    if (asset != null)
    {
      float var = 1f + (Mathf.PingPong(Time.time * 0.04f, 0.03f));
    
      if (isBoosted)
      {
        asset.transform.localScale = new Vector3(var, var, var);
      } else
      {
        asset.transform.localScale = new Vector3(1f, 1f, 1f);
      }
    }
  }
}
