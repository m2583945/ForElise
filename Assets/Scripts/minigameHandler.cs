using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class minigameHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject[] minigames;
    //public bool[] gameDone;
    public GameObject gameFrame;
    public int currentGame = 0;
    public DialogueRunner dr;
    DialogueHandler dh;
    public GameObject scenebg;
    int dayValue = 1;
    void Start()
    {
        dr.AddCommandHandler<int>("startDay", startDay);
        dh = GameObject.Find("scriptholder").gameObject.GetComponent<DialogueHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void disableAllGames()
    {
        for(int i = 0; i < minigames.Length; i++)
        {
            minigames[i].SetActive(false);
        }
    }

    public void startDay(int day)
    {
        //play screen transition;
        scenebg.SetActive(false);
        gameFrame.SetActive(true);
        dayValue = day;
        //minigames[0].gameObject.SetActive(true);
        disableAllGames();
        dh.fadeMusicOut("dialogue");
        if(day == 1)
        {
            print("day 1 start");
            minigames[0].gameObject.SetActive(true);
            return;
        }
        if (day == 2)
        {
            print("day 2 start");
            currentGame = 1;
            minigames[1].gameObject.SetActive(true);
            return;
        }
        if(day == 3)
        {
            print("day 3 start");
            currentGame = 3;
            minigames[3].gameObject.SetActive(true);
            return;

        }
    }
    public void endDay(int day)
    {
        dh.fadeMusicIn("dialogue");
        print("endday");
        scenebg.SetActive(true);
        gameFrame.SetActive(false);
        minigames[currentGame].gameObject.SetActive(false);
        if(dayValue == 1)
        {
            dh.runMaintenance(2);
            //dh.runNode("Maintenance1");
        }
        if(dayValue == 2)
        {
            currentGame = 2;
            print("end the second day");
            dh.runMaintenance(4);
        }
        if (dayValue == 3)
        {
            //print("day value is 3!!");
            dh.runMaintenance(6);
        }
    }
    public void nextGame()
    {
        //print("starting next day");
        //print("turn off" + currentGame.ToString());
        if (currentGame == 0)
        {
            print("ending the day");
            currentGame++;
            endDay(1);
            return;
        }
        if(currentGame == 2)
        {
            currentGame++;
            endDay(2);
            return;
        }
        if(currentGame == 4)
        {
            currentGame++;
            endDay(3);
            return;
        }
        else
        {
            //print("end of day current game is " + currentGame.ToString());
            minigames[currentGame].gameObject.SetActive(false);
            currentGame++;
            //play screen transition
            minigames[currentGame].gameObject.SetActive(true);
        }

    }
}
