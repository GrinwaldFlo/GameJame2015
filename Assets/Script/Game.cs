using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game : MonoBehaviour
{
  bool hasPlayedWarning;

  // Use this for initialization
  void Start()
  {
    gvar.currentLevel = 1;
    gvar.state = enState.SplashScreen;
    PlayerPrefs.SetInt(gvar.prefLevelOK + "1", 1);

    uiManager.onStart += onStart;
    uiManager.onEnd += onEnd;
    uiManager.onIntro += onIntro;
  }

  void Awake()
  {
    gvar.level = new GameLevel();
    gvar.lstVisitor = new System.Collections.Generic.List<GameObject>();
  }
  
  // Update is called once per frame
  void Update()
  {
    switch (gvar.state)
    {
      case enState.Play:
        if (!gvar.win && gvar.needsSum > 0 && gvar.needsOKp >= gvar.needsGoal)
        {
          gvar.sound.askPlay(soundPlayer.enSound.win);
          gvar.win = true;
        }
        
        if (hasPlayedWarning == false && Time.time - gvar.timeStart > gvar.timeDuration - 10f)
        {
          hasPlayedWarning = true;
          gvar.sound.askPlay(soundPlayer.enSound.endOfTheDay);
        }
        break;
      default:
        break;
    }

  }

  void onStart()
  {
    setLevelValue();
    gvar.state = enState.Play;
    gvar.sound.askPlay(soundPlayer.enSound.start);
    gvar.timeStart = Time.time;
    gvar.needsOK = 0;
    gvar.win = false;
    gvar.needsSum = 0;
    hasPlayedWarning = false;
    
    while (gvar.lstVisitor.Count > 0)
    {
      Destroy(gvar.lstVisitor[0]);
      gvar.lstVisitor.RemoveAt(0);
    }
    gvar.sound.askPlay(soundPlayer.enSound.click);
  }

  void onEnd(bool win)
  {
    if(gvar.win)
    {
      gvar.state = enState.EndWin;
      gvar.sound.askPlay(soundPlayer.enSound.win);
      gvar.ui.showCanvas(uiManager.enCanvas.EndWin);
      PlayerPrefs.SetInt(gvar.prefLevelOK + (gvar.currentLevel + 1), 1);
      if(gvar.needsOKp > PlayerPrefs.GetFloat(gvar.preflevelHighScore + gvar.currentLevel))
        PlayerPrefs.SetFloat(gvar.preflevelHighScore + gvar.currentLevel, gvar.needsOKp);
    }
    else
    {
      gvar.state = enState.EndLoose;
      gvar.sound.askPlay(soundPlayer.enSound.lose);
      gvar.ui.showCanvas(uiManager.enCanvas.EndLoose);
    }
  }

  void onIntro()
  {
    setLevelValue();
    gvar.ui.updIntro();
  }
  
  public void setLevelValue()
  {
    switch (gvar.currentLevel)
    {
      case 1:
        gvar.nbNeeds = 2;
        gvar.needsGoal = 90;
        gvar.level.nbVisitors = 5;
        gvar.attractionStdZone = 20f;
        gvar.attractionBoost1 = 50f;
        gvar.timeDuration = 60f;
        break;
      case 2:
        gvar.nbNeeds = 4;
        gvar.needsGoal = 80;
        gvar.level.nbVisitors = 10;
        gvar.attractionStdZone = 20f;
        gvar.attractionBoost1 = 50f;
        gvar.timeDuration = 90f;
        break;
      case 3:
        gvar.nbNeeds = 3;
        gvar.needsGoal = 70;
        gvar.level.nbVisitors = 15;
        gvar.attractionStdZone = 15f;
        gvar.attractionBoost1 = 30f;
        gvar.timeDuration = 90f;
        break;
      case 4:
        gvar.nbNeeds = 6;
        gvar.needsGoal = 80;
        gvar.level.nbVisitors = 20;
        gvar.attractionStdZone = 15f;
        gvar.attractionBoost1 = 35f;
        gvar.timeDuration = 100f;
        break;
      case 5:
        gvar.nbNeeds = 5;
        gvar.needsGoal = 85;
        gvar.level.nbVisitors = 25;
        gvar.attractionStdZone = 15f;
        gvar.attractionBoost1 = 35f;
        gvar.timeDuration = 120f;
        break;
      case 6:
        gvar.nbNeeds = 4;
        gvar.needsGoal = 60;
        gvar.level.nbVisitors = 30;
        gvar.attractionStdZone = 10f;
        gvar.attractionBoost1 = 30f;
        gvar.timeDuration = 100f;
        break;
      case 7:
        gvar.nbNeeds = 3;
        gvar.needsGoal = 40;
        gvar.level.nbVisitors = 35;
        gvar.attractionStdZone = 25f;
        gvar.attractionBoost1 = 50f;
        gvar.timeDuration = 45f;
        break;
      case 8:
        gvar.nbNeeds = 4;
        gvar.needsGoal = 100;
        gvar.level.nbVisitors = 40;
        gvar.attractionStdZone = 30f;
        gvar.attractionBoost1 = 60f;
        gvar.timeDuration = 120f;
        break;
      case 9:
        gvar.nbNeeds = 5;
        gvar.needsGoal = 30;
        gvar.level.nbVisitors = 45;
        gvar.attractionStdZone = 15f;
        gvar.attractionBoost1 = 20f;
        gvar.timeDuration = 90f;
        break;
      case 10: // Last level
        gvar.nbNeeds = 3;
        gvar.needsGoal = 50;
        gvar.level.nbVisitors = 50;
        gvar.attractionStdZone = 10f;
        gvar.attractionBoost1 = 25f;
        gvar.timeDuration = 105f;
        break;
      case 11: // Impossible level
        gvar.nbNeeds = 5;
        gvar.needsGoal = 100;
        gvar.level.nbVisitors = 500;
        gvar.attractionStdZone = 10f;
        gvar.attractionBoost1 = 50f;
        gvar.timeDuration = 60f;
        break;
      default:
        break;
    }
  }


}
