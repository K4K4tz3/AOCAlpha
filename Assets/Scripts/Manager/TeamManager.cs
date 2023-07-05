using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TeamManager : MonoBehaviour
{

    private Dictionary<GameObject, Team> teamAssignments = new Dictionary<GameObject, Team>();

    public void AssignTeam(GameObject warlord, Team team)
    {
        // Champion already assigned to a team, update the team assignment
        if (teamAssignments.ContainsKey(warlord))
        {
            teamAssignments[warlord] = team;
        }
        else
        {
            // Champion is not yet assigned to a team, add a new assignment
            teamAssignments.Add(warlord, team);
        }
    }

    // Method to get the team of a champion
    public Team GetTeam(GameObject warlord)
    {
        if (teamAssignments.ContainsKey(warlord))
        {
            return teamAssignments[warlord];
        }
        else
        {
            // Champion not found in team assignments, return a default team
            return Team.LeftTeam;
        }
    }

    public Team ChooseRandomTeam()
    {
        // Get all the available teams from the Team enum
        Team[] allTeams = (Team[])Enum.GetValues(typeof(Team));

        // Choose a random team from the available options
        Team randomTeam = allTeams[Random.Range(0, allTeams.Length)];

        return randomTeam;
    }

    public Team AssignFreeTeam(GameObject champion)
    {
        // Get all the available teams from the Team enum
        Team[] allTeams = (Team[])Enum.GetValues(typeof(Team));

        // Create a list to store the teams that are already assigned to other champions
        List<Team> assignedTeams = new List<Team>();

        // Iterate through each champion and collect the assigned teams
        foreach (var kvp in teamAssignments)
        {
            if (kvp.Key != champion)
            {
                assignedTeams.Add(kvp.Value);
            }
        }

        // Iterate through each team and check if it is already assigned to another champion
        foreach (Team team in allTeams)
        {
            if (!assignedTeams.Contains(team))
            {
                // Team is not assigned to any other champion, assign it to the current champion
                AssignTeam(champion, team);
                return team;
            }
        }

        // If all teams are already assigned to other champions, return a default team (e.g., TeamA)
        return Team.RightTeam;
    }

    public bool IsTeamAssigned(Team team)
    {
        // Iterate through the team assignments dictionary and check if any champion is assigned to the given team
        foreach (var kvp in teamAssignments)
        {
            if (kvp.Value == team)
            {
                // Found a champion assigned to the given team
                return true;
            }
        }

        // No champion assigned to the given team
        return false;
    }

}
