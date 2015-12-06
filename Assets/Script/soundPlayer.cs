using UnityEngine;
using System.Collections;

public class soundPlayer : MonoBehaviour
{

  public AudioSource boost;
  public AudioSource checkedLike;
  public AudioSource click;
  public AudioSource endOfTheDay;
  public AudioSource gameMusicLoop;
  public AudioSource hoverAttraction;
  public AudioSource intro;
  public AudioSource lose;
  public AudioSource start;
  public AudioSource win;

  private bool p_enableSound = true;

  public enum enSound
  {
    boost,
    checkedLike,
    click,
    endOfTheDay,
    gameMusicLoop,
    hoverAttraction,
    intro,
    lose,
    start,
    win
  }

  public bool enableSound {
    get
    {
      return p_enableSound;
    }
    set
    {
      if(value == false)
      {
        intro.Stop();
        gameMusicLoop.Stop();
      }
      p_enableSound = value;
      PlayerPrefs.SetInt(gvar.prefSound, value ? 1 : 0);
    }
  }

  public void askPlay(enSound sound)
  {
    if (enableSound == false)
    {
      return;
    }
    switch (sound) {
      case enSound.boost:
        boost.Play();
        break;
      case enSound.checkedLike:
        checkedLike.Play();
        break;
      case enSound.click:
        click.Play();
        break;
      case enSound.endOfTheDay:
        endOfTheDay.Play();
        break;
      case enSound.gameMusicLoop:
        gameMusicLoop.Play();
        break;
      case enSound.hoverAttraction:
        hoverAttraction.Play();
        break;
      case enSound.intro:
        intro.Play();
        break;
      case enSound.lose:
        lose.Play();
        break;
      case enSound.start:
        start.Play();
        break;
      case enSound.win:
        win.Play();
        break;
    default:
    break;
    }
  }

  public void askStop(enSound sound)
  {
    switch (sound) {
      case enSound.boost:
        boost.Stop();
        break;
      case enSound.checkedLike:
        checkedLike.Stop();
        break;
      case enSound.click:
        click.Stop();
        break;
      case enSound.endOfTheDay:
        endOfTheDay.Stop();
        break;
      case enSound.gameMusicLoop:
        gameMusicLoop.Stop();
        break;
      case enSound.hoverAttraction:
        hoverAttraction.Stop();
        break;
      case enSound.intro:
        intro.Stop();
        break;
      case enSound.lose:
        lose.Stop();
        break;
      case enSound.start:
        start.Stop();
        break;
      case enSound.win:
        win.Stop();
        break;
      default:
        break;
    }
  }

  // Use this for initialization
  void Start()
  {
    enableSound = PlayerPrefs.GetInt(gvar.prefSound) == 1 ? true : false;
    gvar.sound.askPlay(soundPlayer.enSound.intro);
  }

  void Awake()
  {
    gvar.sound = this;
  }
  
  // Update is called once per frame
  void Update()
  {
  
  }
}
