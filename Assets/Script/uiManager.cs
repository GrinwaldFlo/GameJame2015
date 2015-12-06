using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class uiManager : MonoBehaviour
{
  // Events
  public delegate void evStart();
  public static event evStart onStart;

  public delegate void evSelLevel();
  public static event evSelLevel onSelLevel;

  public delegate void evIntro();
  public static event evIntro onIntro;
  
  public delegate void evEnd(bool win);
  public static event evEnd onEnd;


  public enum enCanvas
  {
    Play,
    SelectLevel,
    Intro,
    EndLoose,
    EndWin,
    SplashScreen,
    Tutorial
  }


  public Canvas canPlay;
  public Canvas canIntro;
  public Canvas canEnd;
  public Canvas canEndWin;
  public Canvas canSplash;
  public Canvas canTuto;
  public Canvas canSelLevel;
  public Text txtLevel;
  public Slider sliderLevel;
  public Text txtVisitorNb;
  public Text txtVisitorSum;
  public Text txtTime;
  public Text txtAttractionTitle;
  public Text txtAttractionInfo;
  public Text txtNeeds;
  public Text txtLevelTitle;
  public Text txtIntro;
  public Text txtEndLose;
  public Text txtEndWin;
  public Text txtEndTitleLose;
  public Text txtEndTitleWin;
  public Text txtAchieved;
  public Toggle[] tglNeeds;
  public Toggle tglSound;
  public Text txtPlayLevel;
  public Image panLevels;

  public Button prefabButLevel;
  private Text[] lblNeeds;
  private Button[] butLevel;


  // Use this for initialization
  void Start()
  {
    showCanvas(enCanvas.SplashScreen);
    sliderLevel.gameObject.SetActive(false);
    txtLevel.enabled = false;
    
    lblNeeds = new Text[tglNeeds.Length];
    for (int i = 0; i < tglNeeds.Length; i++)
    {
      lblNeeds[i] = tglNeeds[i].gameObject.GetComponentInChildren<Text>();
    }
    
    tglSound.isOn = PlayerPrefs.GetInt(gvar.prefSound) == 1 ? false : true;
  }

  void Awake()
  {
    gvar.ui = this;
  }
  
  // Update is called once per frame
  void Update()
  {
    switch (gvar.state)
    {
      case enState.Play:
        updPlayUI();

        if(Time.time - gvar.timeStart > gvar.timeDuration)
        {
          endPlay();
        }
        break;
      default:
        break;
    }
  }

  void startPlay()
  {
    if(onStart != null)
      onStart();

    for (int i = 0; i < tglNeeds.Length; i++) {
      tglNeeds[i].gameObject.SetActive(false);
    }
    txtAttractionInfo.text = "";
    txtAttractionTitle.text = "";
    txtVisitorNb.text = "";
    txtPlayLevel.text = "Level " + gvar.currentLevel;
  }

  void endPlay()
  {
    if(onEnd != null)
      onEnd(gvar.win);

    if(gvar.win)
    {
      txtEndTitleWin.text = "Level " + gvar.currentLevel;
      txtEndWin.text = "Satisfaction:\r\n\r\n";
      txtEndWin.text += "Goal      " + gvar.needsGoal + "%\r\n";
      txtEndWin.text += "Achieved  " + (gvar.needsOK * 100f / gvar.needsSum).ToString("0") + "%\r\n\r\n";
      txtEndWin.text += "Congratulation !";
    }
    else
    {
      txtEndTitleLose.text = "Level " + gvar.currentLevel;
      txtEndLose.text = "Satisfaction:\r\n\r\n";
      txtEndLose.text += "Goal      " + gvar.needsGoal + "%\r\n";
      txtEndLose.text += "Achieved  " + (gvar.needsOK * 100f / gvar.needsSum).ToString("0") + "%\r\n\r\n";
      txtEndLose.text += "Oh no !";
    }
  }

  public void showCanvas(enCanvas canvas)
  {
    canEnd.enabled = false;
    canEndWin.enabled = false;
    canPlay.enabled = false;
    canIntro.enabled = false;
    canSplash.enabled = false;
    canTuto.enabled = false;
    canSelLevel.enabled = false;

    switch (canvas)
    {
      case enCanvas.EndLoose:
        canEnd.enabled = true;
        break;
      case enCanvas.EndWin:
        canEndWin.enabled = true;
        break;
      case enCanvas.Play:
        canPlay.enabled = true;
        break;
      case enCanvas.Intro:
        canIntro.enabled = true;
        break;
      case enCanvas.SplashScreen:
        canSplash.enabled = true;
        break;
      case enCanvas.Tutorial:
        canTuto.enabled = true;
        break;
      case enCanvas.SelectLevel:
        canSelLevel.enabled = true;
        break;
      default:
        break;
    }
  }

  void updPlayUI()
  {
    txtVisitorSum.text = "Visitors: " + gvar.lstVisitor.Count;
    if(gvar.needsSum != 0)
      txtNeeds.text = gvar.needsOKp.ToString("0") + "% / " + gvar.needsGoal + "%"; 
    else
      txtNeeds.text = "0% / 0%";
    // Time management
    txtTime.text = (gvar.timeDuration - (Time.time - gvar.timeStart)).ToString("000");

    txtAchieved.enabled = gvar.win;
  }

  public void updTxtAttraction(Attraction attraction)
  {
    txtAttractionTitle.text = attraction.title;
    txtAttractionInfo.text = "";
    foreach (enAttribute item in attraction.lstAttribute)
    {
      txtAttractionInfo.text += "- " + item.ToString() + "\r\n";
    } 
    txtAttractionInfo.text = txtAttractionInfo.text.Replace("_", "");
  }

  public void updIntro()
  {
    txtLevelTitle.text = "Level " + gvar.currentLevel; 
    
    txtIntro.text = gvar.level.nbVisitors + " visitors\r\n";
    txtIntro.text += "  they each like " + gvar.nbNeeds + " things\r\n";
    //txtIntro.text += "5 attractions\r\n";
    txtIntro.text += gvar.timeDuration + " seconds\r\n\r\n";
    txtIntro.text += "Goal: " + gvar.needsGoal + "% satisfaction";
  }

  public void updTxtVisitor(VisitorMove visitor)
  {
    txtVisitorNb.text = visitor.thought;
    for (int i = 0; i < tglNeeds.Length; i++) {
      tglNeeds[i].gameObject.SetActive(false);
    }
    
    int curTgl = 0;
    foreach (enAttribute item in visitor.lstNeeds)
    {
      tglNeeds[curTgl].isOn = false;
      tglNeeds[curTgl].gameObject.SetActive(true);
      lblNeeds[curTgl].text = item.ToString().Replace("_", "");
      curTgl++;
    } 
    
    foreach (enAttribute item in visitor.lstNeedsOK)
    {
      tglNeeds[curTgl].isOn = true;
      tglNeeds[curTgl].gameObject.SetActive(true);
      lblNeeds[curTgl].text = item.ToString().Replace("_", "");
      curTgl++;
    } 
  }

  public void butGoIntro(bool won)
  {
    showCanvas(enCanvas.Intro);
    gvar.sound.askPlay(soundPlayer.enSound.click);

    if (won)
      gvar.currentLevel++;

    gvar.state = enState.Intro;

    if(onIntro != null)
      onIntro();
  }

  public void butGoIntro(Button sender)
  {
    showCanvas(enCanvas.Intro);
    gvar.sound.askPlay(soundPlayer.enSound.click);

    gvar.currentLevel = int.Parse(sender.GetComponentInChildren<Text>().text);

    gvar.state = enState.Intro;
    
    if(onIntro != null)
      onIntro();
  }

  public void butStart()
  {
    gvar.sound.askPlay(soundPlayer.enSound.gameMusicLoop);
    gvar.sound.askStop(soundPlayer.enSound.intro);
    showCanvas(enCanvas.Play);
    startPlay();
  }
  

  public void butGoSplash()
  {
    showCanvas(enCanvas.SplashScreen);
    gvar.state = enState.SplashScreen;
    gvar.currentLevel = 1;

    gvar.sound.askPlay(soundPlayer.enSound.click);
  }
    
  public void butClosePark()
  {
    gvar.timeDuration = 1f;
    gvar.sound.askPlay(soundPlayer.enSound.click);
    gvar.sound.askStop(soundPlayer.enSound.endOfTheDay);
  }
  
  public void butTuto()
  {
    canSplash.enabled = false;
    canTuto.enabled = true;
    gvar.sound.askPlay(soundPlayer.enSound.click);
  }
  
  public void butNoSOund()
  {
    gvar.sound.enableSound = !tglSound.isOn;
  }
  
  public void sliderLever(float val)
  {
    gvar.currentLevel = (int)sliderLevel.value;
    txtLevel.text = "Level cheater " + gvar.currentLevel;
  }
  
  public void butShowCheat()
  {
    sliderLevel.gameObject.SetActive(true);
    txtLevel.enabled = true;
  }

  public void butSelectLevel()
  {
    showCanvas(enCanvas.SelectLevel);
    gvar.sound.askPlay(soundPlayer.enSound.click);

    gvar.state = enState.SelectLevel;
    
    if(onSelLevel != null)
      onSelLevel();

    for (int i = 0; i < panLevels.transform.childCount; i++)
    {
      Destroy(panLevels.transform.GetChild(i).gameObject);
    }

    RectTransform panLevelRect = panLevels.GetComponent<RectTransform>();
    butLevel = new Button[gvar.nbLevel];
    int[] intLevel = new int[gvar.nbLevel];
    float x = 0, y = 0, space = 5f;

    int tmpLevel;
    for (int i = 0; i < gvar.nbLevel; i++)
    {
      tmpLevel = i + 1;
      intLevel[i] = tmpLevel;

      Button tmpBut = (Button)Instantiate(prefabButLevel);
      tmpBut.transform.SetParent(panLevels.gameObject.transform);
      tmpBut.onClick.AddListener(() => butGoIntro(tmpBut));
      RectTransform tmpRect = tmpBut.GetComponent<RectTransform>();
      tmpRect.anchoredPosition = new Vector2(x,y);
      Text[] tmpTxt = tmpBut.GetComponentsInChildren<Text>();
      tmpTxt[0].text = tmpLevel.ToString();
      tmpTxt[1].text = PlayerPrefs.GetFloat(gvar.preflevelHighScore + tmpLevel).ToString("0") + " %";
      tmpBut.interactable = PlayerPrefs.GetInt(gvar.prefLevelOK + tmpLevel) == 1;
      butLevel[i] = tmpBut;

      x += tmpRect.sizeDelta.x + space;
      if(x + tmpRect.sizeDelta.x + space > panLevelRect.sizeDelta.x)
      {
        x = 0;
        y -= tmpRect.sizeDelta.y + space;
      }
    }
  }
}
