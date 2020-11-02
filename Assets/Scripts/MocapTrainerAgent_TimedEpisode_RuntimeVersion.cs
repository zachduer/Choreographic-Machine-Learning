using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

public class MocapTrainerAgent_TimedEpisode_RuntimeVersion : Agent
{
    // public int jointClassificationNumber;
    // public int qualityClassificationNumber;

    //public FeatureLabels featureLabels;

    public float episodeDuration = 2;

    [Range(0, 1)]
    public float normalizedAnimationStartTime = 0;
    [Range(0, 1)]
    public float normalizedAnimationEndTime = 1;

    float animationLength;
    [HideInInspector]
    public float currentAnimationTime;

    [HideInInspector]
    public int numEpisodes;
    [HideInInspector]
    public int numSteps;

    string[] jointNames;
    Transform[] joints;
   
    // the values derived from the labels for this recording
    int[] recordingVectorActionValues;

    float episodeTimer; // counts up time in non-real time, but as much time is allowed to pass in the virtual environment.  (so, the training slowing down the program doesn't matter because we get an accurate count of how much game time has passed).

    //AnimationClip animClip;
    public string recordingName;
    Animation anim;

    int numberCorrect;
    int numberIncorrect;
    int lastHundredNumCorrect;
    int lastHundredNumIncorrect;
    int hundredCounter;

    List<bool> validityOfOutputs = new List<bool>();
    List<bool> lastHundredValidityOfOutputs = new List<bool>();
    [HideInInspector]
    public float cumulativeAccuracy = 0;
    [HideInInspector]
    public float lastHundredAccuracy = 0;

    // The commented out line will work when we have a data structure for features working
    //public void Init(string _recordingName, string[] _jointNamesBaby, string _behaviorName, FeatureLabels _featureLabels)
    public void Init(string _recordingName, string[] _jointNamesBaby, string _behaviorName, int[] _recordingVectorActionValues)
    {
        recordingName = _recordingName;
        jointNames = _jointNamesBaby;
        recordingVectorActionValues = _recordingVectorActionValues;
        //featureLabels = _featureLabels;     // The commented out line will work when we have a data structure for features working
    }

    // Start is called before the first frame update
    void Start()
    {
        joints = new Transform[jointNames.Length];

        // Get the joints we'll be using
        Transform[] allChildren = transform.GetComponentsInChildren<Transform>();
        int jointCounter = 0;
        for (int i = 0; i < allChildren.Length; i++)
        {
            Transform child = allChildren[i];

            // check to see if the name is on the list of valid joint names
            bool validJoint = false;
            for(int j = 0; j < jointNames.Length; j++)
            {
                if(child.name == jointNames[j])
                {
                    validJoint = true;
                }
            }

            if (validJoint)
            {
                joints[jointCounter] = child;
                jointCounter++;
            }
        }

        episodeTimer = 0;

        GetComponent<Animation>().Play();
        animationLength = GetComponent<Animation>().clip.length;
        anim = GetComponent<Animation>();
    }


    private void Update()
    {
        if (gameObject.activeSelf)
        {
            RequestDecision();
            episodeTimer += Time.deltaTime;
        }
    }

    public override void OnEpisodeBegin()
    {
        // nothing to do here in the Timed Version because the animations play on their own loop not tied to the timed episode
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 normalizedHipPosition = new Vector3();
        normalizedHipPosition.x = joints[0].localPosition.x / 10f;
        normalizedHipPosition.y = joints[0].localPosition.y / 10f;
        normalizedHipPosition.z = joints[0].localPosition.z / 10f;
        sensor.AddObservation(normalizedHipPosition);
        try
        {
            foreach (Transform joint in joints)
            {
                sensor.AddObservation(joint.localRotation);
            }
        }
        catch (Exception)
        {
            Debug.LogError("Error! Imported joint names do not match joints present in skeleton");
        }

    }

    public override void OnActionReceived(float[] vectorAction)
    {
        numSteps++;

        currentAnimationTime = anim[recordingName].time;

        for (int i = 0; i < vectorAction.Length; i++)
        {
            if (episodeTimer < episodeDuration)
            {
                // The commented out line works when we have a data structure for features working
                if (vectorAction[i] == recordingVectorActionValues[i])
                {
                    SetReward(.1f);
                }
                else
                {
                    SetReward(-.01f);
                }
            }
            else
            {
                numEpisodes++;

                if (vectorAction[i] == recordingVectorActionValues[i])
                {
                    SetReward(1.0f);
                    validityOfOutputs.Add(true);
                    lastHundredValidityOfOutputs.Add(true);
                }
                else
                {
                    SetReward(-.1f);
                    validityOfOutputs.Add(false);
                    lastHundredValidityOfOutputs.Add(false);
                }

                episodeTimer = 0;

                OutputAccuracyToConsole();

                EndEpisode();
            }

        }
    }

    void OutputAccuracyToConsole()
    {

        if (lastHundredValidityOfOutputs.Count > 100)
        {
            lastHundredValidityOfOutputs.RemoveAt(0);
        }

        int numTotalCorrect = 0;
        for (int i = 0; i < validityOfOutputs.Count; i++)
        {
            if (validityOfOutputs[i])
            {
                numTotalCorrect++;
            }
        }

        int lastHundredCorrect = 0;
        for (int i = 0; i < lastHundredValidityOfOutputs.Count; i++)
        {
            if (lastHundredValidityOfOutputs[i])
            {
                lastHundredCorrect++;
            }
        }

        cumulativeAccuracy = (float)numTotalCorrect / (float)validityOfOutputs.Count;
        lastHundredAccuracy = (float)lastHundredCorrect / (float)lastHundredValidityOfOutputs.Count;
        //Debug.Log(gameObject.name + "... numEpisodes: " + numEpisodes + " ... cumulativeAccuracy: " + cumulativeAccuracy.ToString("F2") + " ... lastHundredAccuracy: " + lastHundredAccuracy.ToString("F2"));
    }
}
