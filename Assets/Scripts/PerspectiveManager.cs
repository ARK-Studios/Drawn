using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Utility;
using Assets.Scripts.Perspective;
using Assets.Scripts.Perspective.Cameras;


public class PerspectiveManager : Singleton<PerspectiveManager>
{
    private List<AbstractCamera> registeredCameras = new List<AbstractCamera>();
    

    // --------------------------------------------------------------------
    public void RegisterCamera(AbstractCamera camera)
    {
        registeredCameras.Add(camera);
        Debug.Log("Camera Registered");
    }

    public void DeregisterCamera(AbstractCamera camera)
    {
        registeredCameras.Remove(camera);
        Debug.Log("Camera DeRegistered");
    }

    private void DetermineRestriction(float viewport, eCameraBoundEntryAxis entryVector, ref eAxisRestriction restriction)
    {
        if (((viewport > 0.5F) &&
            ((entryVector == eCameraBoundEntryAxis.CameraBoundEntryNegative) || (entryVector == eCameraBoundEntryAxis.CameraBoundEntryBoth))) || 
            ((viewport < 0.5F) &&
            ((entryVector == eCameraBoundEntryAxis.CameraBoundEntryPositive) || (entryVector == eCameraBoundEntryAxis.CameraBoundEntryBoth))))
        {
            if (entryVector == eCameraBoundEntryAxis.CameraBoundEntryBoth)
            {
                restriction = eAxisRestriction.AxisRestrictionBoth;
            }
            else if (entryVector == eCameraBoundEntryAxis.CameraBoundEntryNegative)
            {
                restriction = eAxisRestriction.AxisRestrictionPositive;
            }
            else if (entryVector == eCameraBoundEntryAxis.CameraBoundEntryPositive)
            {
                restriction = eAxisRestriction.AxisRestrictionNegative;
            }
        }
    }

    public void OnCameraBoundEntry(CameraBound cameraBound)
    {
        ARKLogger.LogMessage(eLogCategory.Control,
                             eLogLevel.Info,
                             "OnCameraBoundEntry");

        foreach(AbstractCamera cam in registeredCameras)
        {
            CameraMovementRestrictions restriction = new CameraMovementRestrictions();
            Vector3 viewPos = cam.GetComponent<Camera>().WorldToViewportPoint(cameraBound.transform.position);
            DetermineRestriction(viewPos.x, cameraBound.onXEntry, ref restriction.xAxis);
            DetermineRestriction(viewPos.y, cameraBound.onYEntry, ref restriction.yAxis);
            cam.RegisterBound(cam.GetInstanceID(), restriction);
        }
    }

    public void OnCameraBoundExit(CameraBound cameraBound)
    {
        ARKLogger.LogMessage(eLogCategory.Control,
                             eLogLevel.Info,
                             "OnCameraBoundExit");

        foreach (AbstractCamera cam in registeredCameras)
        {
            ARKLogger.LogMessage(eLogCategory.Control,
                                 eLogLevel.Info,
                                 "Not Visible");
            cam.DeregisterBound(cam.GetInstanceID());
        }
    }
}

