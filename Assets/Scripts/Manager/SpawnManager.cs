using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private MinionSO minionSO;
    [SerializeField] private Transform leftSpawnLocation;
    [SerializeField] private Transform rightSpawnLocation;

    [SerializeField] private GameObject minionPrefab;

    private float spawnCooldown = 1f;
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
        if (teamManager.unassignedMinions != null)
        {
            teamManager.unassignedMinions.Add(this.gameObject);
        }

        spawnTimer = minionSO.spawnTimer;
    }

    private void Update()
    {
        if (spawnTimer <= 0)
        {
            SpawnMinions(leftSpawnLocation, Team.LeftTeam);
            //SpawnMinions(rightSpawnLocation, Team.RightTeam);

            StopCoroutine(SpawnMinionInDelay(leftSpawnLocation, Team.LeftTeam));

            //Reset timer
            spawnTimer = minionSO.spawnTimer;
        }

        spawnTimer -= Time.deltaTime;
    }

    private void SpawnMinions(Transform spawnPoint, Team desiredTeam)
    {

        StartCoroutine(SpawnMinionInDelay(spawnPoint, desiredTeam)) ;

    }

    private IEnumerator SpawnMinionInDelay(Transform spawnPoint, Team desiredTeam)
    {
        GameObject go = new GameObject("Minion Wave");

        for (int i = 0; i < minionSO.minionCountPerWave; i++)
        {

            GameObject minion = Instantiate(minionPrefab, spawnPoint.position, Quaternion.identity);
            minion.name = $"Minion number: {i}";
            minion.transform.SetParent(go.transform);

            AssignMinionToTeam(minion, desiredTeam);
            minion.gameObject.GetComponent<MinionController>().team = teamManager.GetObjectsTeam(minion);

            //set destination
            if (teamManager.GetObjectsTeam(minion) == Team.LeftTeam)
            {
                minion.gameObject.GetComponent<MinionController>().TargetDestinationLeftTeam = rightSpawnLocation;
            }
            else if (teamManager.GetObjectsTeam(minion) == Team.RightTeam)
            {
                minion.gameObject.GetComponent<MinionController>().TargetDestinationLeftTeam = leftSpawnLocation;
            }

            yield return new WaitForSeconds(spawnCooldown);

        }
    }

    private void AssignMinionToTeam(GameObject minion, Team desiredTeam)
    {
        teamManager.AssignTeamToObject(minion, desiredTeam);

    }

}
