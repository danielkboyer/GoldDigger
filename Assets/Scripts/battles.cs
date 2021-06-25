using System;
using System.Collections;
using System.Collections.Generic;

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
        //this.p1Score = p1S;
        this.p1Mentality = p1M;
        //this.p2Score = p2S;
        this.p2Mentality = p2M;
        COUNT = 5;
        p1choices = new string[COUNT];
        p1choices = new string[COUNT];
        p1HasCheated = false;
        p2HasCheated = false;
    }

    private string FirstMove(string m)
    {
        if (m == "copycat" || m == "cooperate" || m == "grudger")
        {
            return "cooperate";
        }
        else if(m == "random")
        {
            Random rnd = new Random();
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
            Random rnd = new Random();
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
    public int battle()
    {
        string p1choice = FirstMove(this.p1Mentality);
        string p2choice = FirstMove(this.p2Mentality);
        calcScore(p1choice, p2choice);

        if (p1choice == "cheat")
        {
            p1HasCheated = true;
        }
        if (p2choice == "cheat")
        {
            p2HasCheated = true;
        }


        for (int i = 1; i < 5; i++) 
        {
            p1choice = NextMove(p1Mentality, p2choices[i-1], "first");
            p2choice = NextMove(p2Mentality, p1choices[i-1], "second");
            calcScore(p1choice,p2choice);
            if (p1choice == "cheat")
            {
                p1HasCheated = true;
            }
            if (p2choice == "cheat")
            {
                p2HasCheated = true;
            }
        }

        if (p1Score >= p2Score)
        {
            return 0;
        }
        return 1;
    }

}
