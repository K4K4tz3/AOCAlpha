using UnityEngine;

namespace SystemSpace
{
    public class ResourceSystem : Singleton<ResourceSystem>
    {
        // Ressource List's
        // public List<SA_Turret> TurretList { get; private set; }

        // Ressourse Dictionary's (private)
        // private static Dictionary<BuildingBaseTypes, SA_Building> _buildingDict;

        protected override void Awake()
        {
            base.Awake();
            GetResources();
        }

        private void GetResources()
        {
            // Example:
            // TurretList = Resources.LoadAll<SA_Turret>("Buildings/Turrets").ToList();
            // _turretDict = TurretList.ToDictionary(r => r.SubType, r => r);
        }

        // public static SA_Building GetScriptableBuilding(BuildingBaseTypes t) => _buildingDict[t];
    }
}