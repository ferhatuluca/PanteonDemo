using Core.GameUnits.Buildings;
using Core.Utilities.Singleton;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Core.Managers
{
	public class PlacementManager : SingletonMonoBehaviour<PlacementManager>
	{
        [SerializeField] private Tilemap _clickableAreaTilemap;
        [SerializeField] private GameObject _placementCursorObject;
        
        private Building _spawnedBuilding = null;
        private Vector3 _tileBottomLeft;
        
        public Vector3 CurrentHoveredGridCellWorldPos { private set; get; }
        
        private void Update()
        {
            MouseMovement();
        }
        
        private void OnEnable()
        {
            BuildingSpawnerManager.OnNewBuildingSpawned += OnNewBuildingSpawned;
        }

        private void OnDisable()
        {
            BuildingSpawnerManager.OnNewBuildingSpawned -= OnNewBuildingSpawned;
        }

        private void OnNewBuildingSpawned(Building building)
        {
            if (_spawnedBuilding)
            {
                _spawnedBuilding.GoToPool();
            }
            
            _spawnedBuilding = building;
        }

        private void MouseMovement()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridCellPos = _clickableAreaTilemap.WorldToCell(mouseWorldPos);

            CurrentHoveredGridCellWorldPos = _clickableAreaTilemap.GetCellCenterWorld(gridCellPos);
            
            MoveCursorOrBuilding(CurrentHoveredGridCellWorldPos);

            if (_spawnedBuilding && Input.GetMouseButtonDown(0))
            {
                PlaceSpawnedBuilding();
            }
        }

        private void MoveCursorOrBuilding(Vector3 gridCellWorldPos)
        {
            if (_spawnedBuilding == null)
            {
                _placementCursorObject.SetActive(true);
                _placementCursorObject.transform.position = gridCellWorldPos;
            }
            else
            {
                _placementCursorObject.SetActive(false);
                _spawnedBuilding.transform.position = gridCellWorldPos;
            }
        }

        private void PlaceSpawnedBuilding()
        {
            
        }
	}
}