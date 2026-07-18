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

        private Camera _cam;
        private Building _spawnedBuilding = null;
        private Vector3 _tileBottomLeft;
        
        public Vector3 CurrentHoveredGridCell { private set; get; }

        protected override void InternalAwake()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            MouseMovement();
            
            if (_spawnedBuilding && Input.GetMouseButtonDown(0))
            {
                TryPlaceSpawnedBuilding();
            }
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
            Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridCellPos = _clickableAreaTilemap.WorldToCell(mouseWorldPos);

            CurrentHoveredGridCell = _clickableAreaTilemap.GetCellCenterWorld(gridCellPos);
            CurrentHoveredGridCell = new Vector3(CurrentHoveredGridCell.x, CurrentHoveredGridCell.y, 0f);
            
            MoveCursorOrBuilding(CurrentHoveredGridCell);
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

        private void TryPlaceSpawnedBuilding()
        {
            if(!_spawnedBuilding.BuildingPlaceChecker.CanBePlaced)
                return;
            
            _spawnedBuilding.BuildingPlaceChecker.Place();
            _spawnedBuilding = null;
        }
	}
}