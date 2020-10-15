using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextLogWriter : MonoBehaviour
{
    public string runID;
    string path = "Assets/Resources/TrainingLogs/TrainingResults" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt";
    //writer;

    // Start is called before the first frame update
    void Start()
    {
        path = "Assets/summaries/CustomTrainingLogs/TrainingResults_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")+ "_" + runID + ".txt";
        //writer = new StreamWriter(new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write));

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("recording, time, step, episode, total_steps_across_recordings, total_episodes_across_recordings, cumulative_accuracy, last_hundred_accuracy");
        writer.Close();
    }

    // Update is called once per frame
    void Update()
    {

        StreamWriter writer = new StreamWriter(path, true);

        int totalStepsAcrossRecordings = 0;
        int totalEpisodesAcrossRecordings = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                if (child.GetComponent<MocapTrainerAgent>() != null)
                {
                    totalStepsAcrossRecordings += child.GetComponent<MocapTrainerAgent>().numSteps;
                    totalEpisodesAcrossRecordings += child.GetComponent<MocapTrainerAgent>().numEpisodes;
                }
            }
        }

        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                if (child.GetComponent<MocapTrainerAgent>() != null)
                {
                    writer.WriteLine(child.name + ","
                    + Time.time + ","
                    + child.GetComponent<MocapTrainerAgent>().numSteps + ","
                    + child.GetComponent<MocapTrainerAgent>().numEpisodes + ","
                    + totalStepsAcrossRecordings + ","
                    + totalEpisodesAcrossRecordings + ","
                    + child.GetComponent<MocapTrainerAgent>().cumulativeAccuracy.ToString("F3") + ","
                    + child.GetComponent<MocapTrainerAgent>().lastHundredAccuracy);
                }
            }

        }
        writer.Close();
    }
}
