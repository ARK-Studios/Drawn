using UnityEngine;
using Assets.Scripts.Utility;
using Assets.Scripts.Utility.Timers;

namespace Assets.Scripts.Menu
{
    public class SplashScreen : MonoBehaviour
    {
        //------------------------------------------------------------
        enum ESlashScreenState
        {
            DisplayLogo,
            Transition
        };

        //------------------------------------------------------------
        public int splashMinTimeSeconds = 5;
        private ESlashScreenState stateId;
        private AbstractTimer timer;
        private bool doneBackgroundProcessing = false;

        //------------------------------------------------------------
        void Start()
        {
            ARKLogger.LogMessage(eLogCategory.Control,
                                 eLogLevel.Info,
                                 "SpashScreen: Starting.");

            this.stateId = ESlashScreenState.DisplayLogo;

            this.timer = new PhysicsTimer(splashMinTimeSeconds);
            this.timer.Start();

            // No work to do while displaying splash screen, so lets 
            // just transition when the timer is up
            this.doneBackgroundProcessing = true;
        }

        //------------------------------------------------------------
        void FixedUpdate()
        {
            // Update Physics / Reliable Timer
            this.timer.Update();
        }

        //------------------------------------------------------------
        void Update()
        {
            switch (this.stateId)
            {
                case ESlashScreenState.DisplayLogo:
                    if (this.timer.IsTriggered() && this.doneBackgroundProcessing)
                        this.stateId = ESlashScreenState.Transition;
                    break;

                case ESlashScreenState.Transition:
                    ARKLogger.LogMessage(eLogCategory.Control,
                                         eLogLevel.Info,
                                         "SplashScreen: Transition to LaunchMenu.");
                    SystemManager.Instance.LoadLevelSync("LaunchMenu");
                    break;

                default:
                    ARKLogger.LogMessage(eLogCategory.Control,
                                         eLogLevel.Error,
                                         "Really shouldn't be here... illegal state id set.");

                    // Auto recover.
                    this.stateId = ESlashScreenState.DisplayLogo;
                    break;
            }
        }

        //-----------------------------------------------------------------
        void OnGUI()
        {
            GUI.Button(new Rect(100.0f, 100.0f, 500.0f, 500.0f), "BIG LOGO DISPLAYED HERE!");
        }
    }
}
