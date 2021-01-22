using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class DeployStageOnce : MonoBehaviour
{
    public GameObject AnchorStage;
    private PositionalDeviceTracker _deviceTracker;
    private GameObject _previousAnchor;
    void Start()
    {
        if(AnchorStage == null)
        {
            Debug.Log("AnchorStage must be specified");
            return;
        }

        AnchorStage.SetActive(false);
    }

    public void Awake()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforisStarted);
    }

    public void OnDestroy()
    {
        VuforiaARController.Instance.UnregisterVuforiaStartedCallback(OnVuforisStarted);
    }

    private void OnVuforisStarted()
    {
        _deviceTracker = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();
    }

    public void OnInteractiveHitTest(HitTestResult result)
    {
        if(result == null || AnchorStage == null)
        {
            Debug.LogWarning("Hit test is invalid or AnchorStage is not set");
            return;
        }

        var anchor = _deviceTracker.CreatePlaneAnchor(Guid.NewGuid().ToString(), result);

        if(anchor != null)
        {
            //AnchorStage.transform.parent = anchor.transform;
            AnchorStage.transform.localPosition = Vector3.zero;
            AnchorStage.transform.localRotation = Quaternion.identity;
            AnchorStage.SetActive(true);
        }

        if(_previousAnchor != null)
        {
            Destroy(_previousAnchor);
        }

        //_previousAnchor = anchor;
    }

}
