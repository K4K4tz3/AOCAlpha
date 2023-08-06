using UnityEngine;

public class BaseBuilding : MonoBehaviour, IDamagable
{

    #region Team Assignment
    [SerializeField] GameObject teamManagerObject;
    private TeamManager teamManager;
    public Team team;
    #endregion

    private float hp = 1000;
    private float currentHp;
    private float cityChards = 1200;
    private float regenerationRate = 3;     //can be upgraded if tower are captured (/s)




    private void Awake()
    {
        teamManager = teamManagerObject.GetComponent<TeamManager>();
        team = Team.None;

        currentHp = hp;
    }

    private void Start()
    {
        if (gameObject.tag == "BaseBuildingLeftTeam")
        {
            teamManager.AssignTeamToObject(this.gameObject, Team.LeftTeam);
        }
        else
        {
            teamManager.AssignTeamToObject(this.gameObject, Team.RightTeam);
        }

        team = teamManager.GetObjectsTeam(this.gameObject);
    }

    private void Update()
    {

    }
    public void Die()
    {
        //destroy animation
    }

    public void GetDamaged(float damage, Collider damageDealer)
    {

        if (currentHp > 0)
        {
            currentHp -= damage;
        }
        else
        {
            Die();
        }
    }
}
