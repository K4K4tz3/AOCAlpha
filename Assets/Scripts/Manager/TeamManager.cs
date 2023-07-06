using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TeamManager : MonoBehaviour
{
    //Execution Order is important
    //The list with unassigned Warlords gets initialized first in awake 
    //Warlords are added in PlayerController script in Start (needs to be in start because the list needs to exist before assigning)
    //Then, a coroutine waits for the TeamManager to assign teams 

    [SerializeField] private List<GameObject> leftTeam;
    [SerializeField] private List<GameObject> rightTeam;
    public List<GameObject> unassignedWarlords;

    private void Awake()
    {
        // Populate the list of unassigned warlords
        //AWAKE IMPORTANT
        unassignedWarlords = new List<GameObject>();
    }

    private void Start()
    {
        // Assign teams randomly
        AssignTeamsRandomly();
    }

    private void AssignTeamsRandomly()
    {
        while (unassignedWarlords.Count > 0)
        {
            int randomIndex = Random.Range(0, unassignedWarlords.Count);
            GameObject warlord = unassignedWarlords[randomIndex];

            // Assign the champion to a team
            Team team = GetRandomAvailableTeam();
            AssignTeamToChampion(warlord, team);

            unassignedWarlords.RemoveAt(randomIndex);
        }
    }

    private Team GetRandomAvailableTeam()
    {
        // Check if both teams are already full
        if (leftTeam.Count >= 1 && rightTeam.Count >= 1)
        {
            Debug.LogWarning("Both teams are already full.");
            return Team.None;
        }

        // If Team A is full, assign to Team B
        if (leftTeam.Count >= 1)
        {
            return Team.RightTeam;
        }
        // If Team B is full, assign to Team A
        else if (rightTeam.Count >= 1)
        {
            return Team.LeftTeam;
        }
        // Otherwise, randomly assign to Team A or Team B
        else
        {
            return (Random.Range(0, 2) == 0) ? Team.LeftTeam : Team.RightTeam;
        }
    }

    private void AssignTeamToChampion(GameObject warlord, Team team)
    {
        if (team == Team.LeftTeam)
        {
            leftTeam.Add(warlord);
            Debug.Log(warlord.name + " has been assigned to LeftTeam.");
        }
        else if (team == Team.RightTeam)
        {
            rightTeam.Add(warlord);
            Debug.Log(warlord.name + " has been assigned to RightTeam.");
        }
    }

    public Team GetWarlordTeam(GameObject warlord)
    {
        if(leftTeam.Contains(warlord))
        {
            return Team.LeftTeam;
        }
        else if(rightTeam.Contains(warlord))
        {
            return Team.RightTeam;
        }

        return Team.None;

      
    }
}



