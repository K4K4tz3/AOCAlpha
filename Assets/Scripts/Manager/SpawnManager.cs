using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private MinionSO minionSO;

    public GameObject minionPrefab;
    

    private float spawnTimer;

    [SerializeField] private Transform leftSpawnLocation;
    [SerializeField] private Transform rightSpawnLocation;

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
            SpawnMinions(leftSpawnLocation, Team.LeftTeam);
            //SpawnMinions(rightSpawnLocation, Team.RightTeam);

            //Reset timer
            spawnTimer = minionSO.spawnTimer;
        }

    }

    private void SpawnMinions(Transform spawnPoint, Team desiredTeam)
    {
        GameObject go = new GameObject("Minion Wave");

        //Minions get instantiated on their spawn points
        //They all have the same parent object

        for (int i = 0; i < minionSO.minionCountPerWave; i++)
        {
            GameObject minion = Instantiate(minionPrefab, spawnPoint.position, Quaternion.identity);
            minion.name = $"Minion number: {i}";
            minion.transform.SetParent(go.transform);
    
            AssignMinionToTeam(minion, desiredTeam);
            minion.gameObject.GetComponent<MinionController>().team = teamManager.GetObjectsTeam(minion);

            //set destination
            if(teamManager.GetObjectsTeam(minion) == Team.LeftTeam)
            {
                minion.gameObject.GetComponent<MinionController>().targetDestinationLeftTeam = rightSpawnLocation;
            }
            else if (teamManager.GetObjectsTeam(minion) == Team.RightTeam)
            {
                minion.gameObject.GetComponent<MinionController>().targetDestinationLeftTeam = leftSpawnLocation;
            }

        }

    }



    private void AssignMinionToTeam(GameObject minion, Team desiredTeam)
    {
        teamManager.AssignTeamToObject(minion, desiredTeam);
     
    }

}
