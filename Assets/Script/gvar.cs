using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum enAttribute
{
	souvenir,
	high,
	slow,
	long_,
	scary,
	food,
	spinning,
	wet,
	fast,
	short_,
	view
}

public enum enState
{
  SplashScreen,
  Intro,
  Play,
  EndWin,
  EndLoose,
  SelectLevel
}

public static class gvar
{
  public static soundPlayer sound;
  public static uiManager ui;

	public static string tagVisitor = "Visitor";
	public static string tagObstacle = "Obstacle";
	public static string tagAttraction = "Attraction";
  public static string tagAttractionZone = "AttractionZone";
  public static string tagAttractionVisit = "AttractionVisit";

  public static string prefSound = "Sound";
  public static string prefLevelOK = "LevelOK";
  public static string preflevelHighScore = "LevelHighScore";

	public static string layerVisitor = "Visitor";
	public static string layerGround = "Ground";
  public static string layerSelectable = "Selectable";

  public static string childNameAttractionZone = "AttractionZone";
  public static string childNameCylinderZone = "CylinderZone";

	public static int currentLevel;
	public static GameLevel level;
	public static enState state;
	public static List<GameObject> lstVisitor;
	public static float timeStart;
  public static float timeDuration = 100f;

  public static float attractionStdZone = 15f;
  public static float attractionBoost1 = 30f;
  public static int needsSum = 99;
  public static int needsOK = 0;
  public static int needsGoal = 0;
  public static int nbNeeds = 0;
  public static bool win = false;
  public static int nbLevel = 10;

  public static float needsOKp
  {
    get
    {
      return gvar.needsOK * 100f / gvar.needsSum;
    }
  }

  public static GameObject gateDoor;

  public static GameObject goBoosted = null;
  
  public static enAttribute[] lstAttribute = new enAttribute[]{enAttribute.fast, enAttribute.food, enAttribute.scary, enAttribute.high,
  enAttribute.long_, enAttribute.short_, enAttribute.slow,  enAttribute.souvenir, enAttribute.spinning, enAttribute.view,
  enAttribute.wet};

  public static void shuffleAttribute()
  {
    shuffleArray(lstAttribute);
  }

  public static bool findNeeds(List<enAttribute> lstNeeds, Attraction a)
  {
    if (a.lstAttribute.Count == 0)
      return true;

    foreach (enAttribute item in lstNeeds)
    {
      foreach (enAttribute item2 in a.lstAttribute) {
        if(item == item2)
          return true;
      }
    }

    return false;
  }

  public static void shuffleArray<T> (T[] array)
  {
    int n = array.Length;
    while (n > 1) 
    {
      int k = Random.Range(0, n--);
      T temp = array[n];
      array[n] = array[k];
      array[k] = temp;
    }
  }
}




public class GameLevel
{
	public int nbVisitors;
	public bool[] attractionEnabled = new bool[10];
	
	public GameLevel()
	{
		
	}
}
