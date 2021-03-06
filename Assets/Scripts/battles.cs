using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class battles
{
    private int COUNT;// rounds of battles

    private int p1Score;
    private int p2Score;

    private string p1Mentality;
    private string p2Mentality;

    private string[] p1choices;
    private string[] p2choices;

    private bool p1HasCheated;
    private bool p2HasCheated;

    public battles(string p1M, string p2M)
    {
        this.p1Mentality = p1M;
        this.p2Mentality = p2M;
        //Debug.Log("a1: " + p1M + ", a2:" + p2M);
        COUNT = 5;
        p1choices = new string[COUNT];
        p2choices = new string[COUNT];
        p1HasCheated = false;
        p2HasCheated = false;
    }

    private string FirstMove(string m)
    {
        if (m == "copycat" || m == "cooperate" || m == "grudger")
        {
            return "cooperate";
        }
        else if (m == "random")
        {
            System.Random rnd = new System.Random();
            int r = rnd.Next(0, 2);
            if (r == 0)
            {
                return "cooperate";
            }
        }
        return "cheat";
    }

    private string NextMove(string mentality, string otherChoice, string pos)
    {
        if (mentality == "cooperate")
        {
            return "cooperate";
        }
        if (mentality == "cheat")
        {
            return "cheat";
        }
        if (mentality == "random")
        {
            System.Random rnd = new System.Random();
            int r = rnd.Next(0, 2);
            if (r == 0)
            {
                return "cooperate";
            }
            else { return "cheat"; }
        }
        if (mentality == "copycat")
        {
            return otherChoice;
        }
        else
        {
            if (pos == "first")
            {
                if (p1HasCheated == true)
                {
                    return "cheat";
                }
                else
                {
                    return "cooperate";
                }
            }
            if (pos == "second")
            {
                if (p2HasCheated == true)
                {
                    return "cheat";
                }
                else
                {
                    return "cooperate";
                }
            }
        }
        return "cooperate";
    }

    private void calcScore(string p1choice, string p2choice)
    {
        if (p1choice == "cooperate" && p2choice == "cooperate")
        {
            UpdateScores(2, 2);
        }
        else if (p1choice == "cooperate" && p2choice == "cheat")
        {
            UpdateScores(3, -1);
        }
        else if (p1choice == "cheat" && p2choice == "cooperate")
        {
            UpdateScores(-1, 3);
        }
        else
        {
            UpdateScores(0, 0);
        }
    }

    private void UpdateScores(int p1, int p2)
    {
        this.p1Score = this.p1Score + p1;
        this.p2Score = this.p2Score + p2;
    }


    /// <summary>
    /// return 0 or 1, 0 if function caller won
    /// </summary>
    public int[] battle()
    {
        this.p1choices[0] = FirstMove(this.p1Mentality);
        this.p2choices[0] = FirstMove(this.p2Mentality);
        calcScore(p1choices[0], p2choices[0]);
        //Debug.Log(p1Score + ", " + p2Score);

        if (p1choices[0] == "cheat")
        {
            p1HasCheated = true;
        }
        if (p2choices[0] == "cheat")
        {
            p2HasCheated = true;
        }


        for (int i = 1; i < 5; i++)
        {
            p1choices[i] = NextMove(p1Mentality, p2choices[i - 1], "first");
            p2choices[i] = NextMove(p2Mentality, p1choices[i - 1], "second");
            calcScore(p1choices[i], p2choices[i]);
            if (p1choices[i] == "cheat")
            {
                p1HasCheated = true;
            }
            if (p2choices[i] == "cheat")
            {
                p2HasCheated = true;
            }
            //Debug.Log(p1Score + ", " + p2Score);
        }

        if (p1Score > p2Score)
        {
            int t = (p1Score - p2Score) / 2;
            int[] ret = { 0, t };
            return ret;
        }
        else if (p1Score < p2Score)
        {
            int t = (p2Score - p1Score) / 2;
            int[] ret = { 1, t };
            return ret;
        }
        else
        {
            int[] ret = { 1, 0 };
            return ret;
        }
    }

    static void Main(string[] args)
    {
        string a1 = "cheat";
        string a2 = "cooperate";
        battles battle = new battles(a1, a2);
        Console.WriteLine(battle.battle());
    }

}
