using UnityEngine;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Menu
{
    public class LaunchMenu : MonoBehaviour
    {
        //------------------------------------------------------------
        enum EMainMenuState
        {
            Startup,
            Menu,
        };

        //------------------------------------------------------------
        enum EMenuButtonId
        {
            None,
            StartGame,
            Quit,
        };
        
        //------------------------------------------------------------
        private EMainMenuState stateId;
        private EMenuButtonId buttonId;

        //------------------------------------------------------------
        void Start()
        {
            ARKLogger.LogMessage(eLogCategory.Control,
                                 eLogLevel.Info, 
                                 "MainMenu: Starting.");

            this.stateId = EMainMenuState.Startup;
            this.buttonId = EMenuButtonId.None;
        }

        //------------------------------------------------------------
        void Update()
        {
            switch (this.stateId)
            {
                case EMainMenuState.Startup:
                    ARKLogger.LogMessage(eLogCategory.Control,
                                      eLogLevel.Info, 
                                      "MainMenu: State: Startup.");

                    // Here we would do any menu preparation work.
                    
                    // Here we would transition through UI appear animations
                    // but for now we lets just go into menu input phase
                    this.stateId = EMainMenuState.Menu;
                    break;
                    
                case EMainMenuState.Menu:
                    if (this.buttonId == EMenuButtonId.StartGame)
                    {
                        ARKLogger.LogMessage(eLogCategory.Control,
                                             eLogLevel.Info,                                                                                    
                                             "MainMenu: Prototype Scene Selected.");
                        SystemManager.Instance.TransitionToScene("Scene2");
                    }
                    else if (this.buttonId == EMenuButtonId.Quit)
                    {
                        ARKLogger.LogMessage(eLogCategory.Control,
                                             eLogLevel.Info,
                                             "MainMenu: Quit Selected.");
                        SystemManager.Instance.Quit();
                    }

                    this.buttonId = EMenuButtonId.None;
                    break;

                default:
                    ARKLogger.LogMessage(eLogCategory.Control,
                                         eLogLevel.Error,
                                         "Really shouldn't be here... illegal state id set.");

                    // Auto recover.
                    this.stateId = EMainMenuState.Startup;
                    break;
            }
        }

        //-----------------------------------------------------------------
        void OnGUI()                                                                                                 
        {
            if (GUI.Button(new Rect(200.0f, 100.0f, 300.0f, 100.0f), "Start Game"))
            {
                this.buttonId = EMenuButtonId.StartGame;
            }

            if (GUI.Button(new Rect(200.0f, 350.0f, 300.0f, 100.0f), "Quit"))
            {
                this.buttonId = EMenuButtonId.Quit;                                                  
            }
        }
    }
}