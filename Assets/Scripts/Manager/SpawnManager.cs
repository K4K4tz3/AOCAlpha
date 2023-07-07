using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private MinionSO minionSO;

    public GameObject minionPrefab;
    public Transform[] spawnPointsLeftSide;
    public Transform[] spawnPointsRightSide;

    private float spawnTimer;

    private void Start()
    {
        //At Game Start, timer = first Spawn Timer because it's taking a while until minions first spawn in game
        //after that, timer has normal stats
        spawnTimer = minionSO.firstSpawnTimer;
        
        
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
            minion.transform.SetParent(go.transform);

        }

    }
}
