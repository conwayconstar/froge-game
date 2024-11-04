using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uimanager : MonoBehaviour {


    public static Uimanager instance;
    [field: SerializeField] public UIPanelHomeScreen ui_HomeScreen { get; private set; }
    [field : SerializeField] public GameScreenUI ui_GameScreen { get; private set; }
    [field : SerializeField] public GameOverScreenUI ui_GameOver { get; private set; }
    [field : SerializeField] public UIPausePanel ui_PausePanel { get; private set; }
    [field : SerializeField] public UISettingScreen ui_SettingPanel { get; private set; }
    [field : SerializeField] public UIRevivePanel ui_RewivePanel { get; private set; }


    private void Awake() {
        instance = this;
    }
}
