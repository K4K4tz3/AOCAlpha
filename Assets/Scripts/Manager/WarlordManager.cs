using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlordManager : MonoBehaviour
{
    //Singleton for managing warlords and their team
    public static WarlordManager Instance { get; private set; }

    private Dictionary<GameObject, Team> championTeams = new Dictionary<GameObject, Team>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterChampion(GameObject champion, Team team)
    {
        championTeams[champion] = team;
    }

    public Team GetChampionTeam(GameObject champion)
    {
        if (championTeams.ContainsKey(champion))
        {
            return championTeams[champion];
        }

        return Team.LeftTeam;                   //default team
    }
}
