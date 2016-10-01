using UnityEngine;
using Assets.Scripts.Utility;
using Assets.Scripts.Utility.Timers;

namespace Assets.Scripts.Menu
{
    public class LoadingScreen : MonoBehaviour
    {
        //------------------------------------------------------------
        enum ELoadingScreenState
        {
            DisplayImage,
            Transition
        };

        //------------------------------------------------------------
        public int transitionMinTimeSeconds = 5;
        private ELoadingScreenState stateId;
        private AbstractTimer timer;
        private bool doneBackgroundProcessing = false;
        private string whatToLoad;

        //------------------------------------------------------------
        void Start()
        {
            ARKLogger.LogMessage(eLogCategory.Control,
                                 eLogLevel.Info,
                                 "LoadingScreen: Starting.");

            this.stateId = ELoadingScreenState.DisplayImage;

            this.timer = new PhysicsTimer(this.transitionMinTimeSeconds);
            this.timer.Start();

            this.whatToLoad = SystemManager.Instance.GetSceneToLoad();

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
                case ELoadingScreenState.DisplayImage:
                    if (this.timer.IsTriggered() && this.doneBackgroundProcessing)
                        this.stateId = ELoadingScreenState.Transition;
                    break;

                case ELoadingScreenState.Transition:
                    ARKLogger.LogMessage(eLogCategory.Control,
                                         eLogLevel.Info,
                                         "LoadingScreen: Transition to ( " + this.whatToLoad +  " )");
                    SystemManager.Instance.LoadLevelSync(this.whatToLoad);
                    break;

                default:
                    ARKLogger.LogMessage(eLogCategory.Control,
                                         eLogLevel.Error,
                                         "Really shouldn't be here... illegal state id set.");

                    // Auto recover.
                    this.stateId = ELoadingScreenState.DisplayImage;
                    break;
            }
        }
    }
}
