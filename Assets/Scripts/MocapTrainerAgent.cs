using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class MocapTrainerAgent : Agent
{
    public int jointClassificationNumber;
    public int qualityClassificationNumber;

    [Range(0, 1)]
    public float normalizedAnimationStartTime;
    [Range(0, 1)]
    public float normalizedAnimationEndTime;

    AnimationClip anim;

    Transform[] jointTransforms;

    int numberCorrect;
    int numberIncorrect;
    int lastHundredNumCorrect;
    int lastHundredNumIncorrect;
    int hundredCounter;

    [HideInInspector]
    public int numEpisodes;
    [HideInInspector]
    public int numSteps;

    List<bool> validityOfOutputs = new List<bool>();
    List<bool> lastHundredValidityOfOutputs = new List<bool>();
    [HideInInspector]
    public float cumulativeAccuracy = 0;
    [HideInInspector]
    public float lastHundredAccuracy = 0;

    // Start is called before the first frame update
    void Start()
    {
        Transform hips = transform.GetChild(0);
        jointTransforms = hips.GetComponentsInChildren<Transform>();

        //int observationCount = 0;
        //observationCount++;
        //observationCount++;
        //observationCount++;
        //foreach (Transform child in jointTransforms)
        //{
        //    observationCount++;
        //    observationCount++;
        //    observationCount++;
        //    observationCount++;
        //}

        // Dynamically load the correct animation clip as long as it is the same name as the game objects
        Resources.LoadAll("Mocap");
        var clips = Resources.FindObjectsOfTypeAll<AnimationClip>();
        foreach (var c in clips)
        {
            if (string.Equals(c.name, gameObject.name))
            {
                anim = c;
            }
        }

        // Assign the animation into the only state of the animation controller
        if (anim)
        {
            AnimatorOverrideController aoc = new AnimatorOverrideController(GetComponent<Animator>().runtimeAnimatorController);
            var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            foreach (var a in aoc.animationClips)
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, anim));
            aoc.ApplyOverrides(anims);
            GetComponent<Animator>().runtimeAnimatorController = aoc;
        }
        else
        {
            Debug.Log("WARNING! " + gameObject.name + " could not match animation to object");
        }
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            RequestDecision();
        }
        //Debug.Log(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    public override void OnEpisodeBegin()
    {
        GetComponent<Animator>().Play(0, 0, normalizedAnimationStartTime);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 normalizedHipPosition = new Vector3();
        normalizedHipPosition.x = jointTransforms[0].localPosition.x / 10f;
        normalizedHipPosition.y = jointTransforms[0].localPosition.y / 10f;
        normalizedHipPosition.z = jointTransforms[0].localPosition.z / 10f;
        sensor.AddObservation(normalizedHipPosition);
        foreach (Transform joint in jointTransforms)
        {
            sensor.AddObservation(joint.localRotation);
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        numSteps++;
        Debug.Log(Time.timeScale);
        UnityEngine.Debug.Log("normalizedTime " + GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);
        // if the animation is not finished yet, do a small step
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < normalizedAnimationEndTime)
        {
            if (vectorAction[0] == jointClassificationNumber)
            {
                SetReward(.1f);
            }
            else
            {
                SetReward(-.01f);
            }
        }
        // if the animation is finished, end the episode
        else
        {
            Debug.Log("end episode");
            numEpisodes++;

            if (vectorAction[0] == jointClassificationNumber)
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

            OutputAccuracyToConsole();

            EndEpisode();

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
