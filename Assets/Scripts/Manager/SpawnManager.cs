using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private MinionSO minionSO;

    public GameObject minionPrefab;
    public Transform[] spawnPointsLeftSide;
    public Transform[] spawnPointsRightSide;

    private float spawnTimer;


    #region Team Assignment
    [SerializeField] private GameObject teamManagerObject;
    private TeamManager teamManager;
    #endregion

    private void Awake()
    {
        teamManager = teamManagerObject.GetComponent<TeamManager>();

    }

    private void Start()
    {
        //At Game Start, timer = first Spawn Timer because it's taking a while until minions first spawn in game
        //after that, timer has normal stats
        spawnTimer = minionSO.firstSpawnTimer;

        if (teamManager.unassignedMinions != null)
        {
            teamManager.unassignedMinions.Add(this.gameObject);
            
        }


    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;


        if (spawnTimer <= 0)
        {
            SpawnMinions(spawnPointsLeftSide);
            //SpawnMinions(spawnPointsRightSide);

            //Reset timer
            spawnTimer = minionSO.spawnTimer;
        }

    }

    private void SpawnMinions(Transform[] spawnPoints)
    {
        GameObject go = new GameObject("Minion Wave");

        //Minions get instantiated on their spawn points
        //They all have the same parent object

        for (int i = 0; i < minionSO.minionCountPerWave; i++)
        {
            GameObject minion = Instantiate(minionPrefab, spawnPoints[i].position, Quaternion.identity);
            minion.name = $"Minion number: {i}";
            minion.transform.SetParent(go.transform);

            //Check if minion is spawned on left or right side
            AssignMinionToTeam(minion, Team.LeftTeam);
            
            minion.gameObject.GetComponent<MinionController>().team = teamManager.GetObjectsTeam(minion);

        }

    }



    private void AssignMinionToTeam(GameObject minion, Team desiredTeam)
    {
        teamManager.AssignTeamToObject(minion, desiredTeam);
     
    }

}
