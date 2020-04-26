using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;

public class TapToPlaceObject : MonoBehaviour {

    public GameObject ObjectToPlace;
    public GameObject placementIndicator;

    private ARSessionOrigin arOrigin;
    private Pose placementPose;
    private bool placementPoseIsValid;
    private bool IsPlaced = false;

	void Start ()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
	}
	
	void Update ()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        //if(placementPoseIsValid && Input.touchCount > 0 
        if (IsPlaced == false && placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            IsPlaced = true;
            placementIndicator.SetActive(false);
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        Instantiate(ObjectToPlace, placementPose.position, placementPose.rotation);
    }

    private void UpdatePlacementIndicator()
    {
        if(placementPoseIsValid && IsPlaced == false)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arOrigin.Raycast(screenCenter, hits, TrackableType.Planes);

        //placementPoseIsValid = hits.Count > 0
        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
