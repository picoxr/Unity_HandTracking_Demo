using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCtrl : MonoBehaviour
{
    public enum TargetValue { One, Two, Three }
    public TargetValue currTargetVal;
    public enum GameMode { Tutorial, Arcade }
    public GameMode currGameMode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetUpCoinWithManager()
    {
        switch (currTargetVal)
        {
            case TargetValue.One:
                //ArcadeManager.instance.CurrCoinVal = "ZoneOne";
                ArcadeManager.instance.MakeCoinOne();

                if (currGameMode == GameMode.Tutorial)
                    TutorialManager.instance.HitTarget1Success();

                break;
            case TargetValue.Two:
                //ArcadeManager.instance.CurrCoinVal = "ZoneTwo";
                ArcadeManager.instance.MakeCoinTwo();

                if (currGameMode == GameMode.Tutorial)
                    TutorialManager.instance.HitTarget2Success();
                break;
            case TargetValue.Three:
                //ArcadeManager.instance.CurrCoinVal = "ZoneThree";
                ArcadeManager.instance.MakeCoinThree();

                if (currGameMode == GameMode.Tutorial)
                    TutorialManager.instance.HitTarget3Success();
                break;
        }
    }
}
