using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Ground : MonoBehaviour
{
  //
  public GameObject lstHoneypot;
  public List<GameObject> lstObstacle = new List<GameObject>();
  public GameObject obstacleGameObj;
  public GameObject trees;
  RaycastHit hit;
  Ray camRay;
  int visitorMask;
  int selectableMask;
  float camRayLength = 1000f;
  private GameObject curObstacle;
  private Highlight curObstacleHighLight;
  private Attraction curAttractionHighlight;
  private VisitorMove curVisitorMove;

  // Use this for initialization
  void Start()
  {
    visitorMask = LayerMask.GetMask(gvar.layerVisitor);
    selectableMask = LayerMask.GetMask(gvar.layerSelectable);
  }

  void hitAttraction()
  {
    camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(camRay, out hit, camRayLength, selectableMask))
    {
      if (hit.collider.tag == gvar.tagAttraction)
      {
        // Disable actual highlight
        if (curObstacleHighLight != null)
          curObstacleHighLight.isSelected = false;
        if (curAttractionHighlight != null)
          curAttractionHighlight.highlight(false);

        // Play sound
        if (curObstacle != hit.collider.gameObject.transform.parent.gameObject)
          gvar.sound.askPlay(soundPlayer.enSound.hoverAttraction);
        // Enable highlight on current
        
        curAttractionHighlight = hit.collider.gameObject.transform.parent.GetComponent<Attraction>();
        curAttractionHighlight.highlight(true);
        
        curObstacle = hit.collider.gameObject.transform.parent.gameObject;
        curObstacleHighLight = curObstacle.GetComponent<Highlight>();
        // curObstacleHighLight.isSelected = true;
        
        gvar.ui.updTxtAttraction(curAttractionHighlight);

        if (Input.GetMouseButtonDown(0))
        {
          if (gvar.goBoosted != null)
          {
            gvar.goBoosted.GetComponent<Attraction>().boost(0);
          }
          gvar.goBoosted = curObstacle;
          gvar.goBoosted.GetComponent<Attraction>().boost(1);

          gvar.sound.askPlay(soundPlayer.enSound.boost);
        }
      } else
      {
        if (curObstacleHighLight != null)
          curObstacleHighLight.isSelected = false;
        
        curObstacle = null;
        curObstacleHighLight = null;
      }
    }
  }

  void hitVisitor()
  {
    camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(camRay, out hit, camRayLength, visitorMask))
    {
      if (hit.collider.tag == gvar.tagVisitor)
      {
        if (curVisitorMove != null)
          curVisitorMove.isSelected = false;

        curVisitorMove = hit.collider.gameObject.GetComponent<VisitorMove>();
        curVisitorMove.isSelected = true;
        
        gvar.ui.updTxtVisitor(curVisitorMove);
      }
    }
  }
  
  // Update is called once per frame
  void Update()
  {
    if (gvar.state == enState.Play)
    {
      hitAttraction();
      hitVisitor();
    }
  }


}
