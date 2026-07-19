using System;
using Core.Enums;
using Core.Managers;
using Core.Scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProductionMenu : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _contentPanelTransform;
        [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;

        private RectTransform _firstItem;
        private float _totalRepeatedItemHeight;

        private Vector2 _oldVelocity; 
        private bool _isUpdated;

        private void Start()
        {
            SpawnBuildingUIs();
        }
        
        private void Update()
        {
            if (_isUpdated)
            {
                _isUpdated = false; 
                _scrollRect.velocity = _oldVelocity;
            }
            
            Vector2 position = _contentPanelTransform.anchoredPosition;
            if (position.y < _totalRepeatedItemHeight * 0.5f)
            {
                _oldVelocity = _scrollRect.velocity;
                position.y += _totalRepeatedItemHeight;
                _contentPanelTransform.anchoredPosition = position;
                _isUpdated = true;
            }
            else if (position.y > _totalRepeatedItemHeight * 1.5f)
            {
                _oldVelocity = _scrollRect.velocity;
                position.y -= _totalRepeatedItemHeight;
                _contentPanelTransform.anchoredPosition = position;
                _isUpdated = true;
            }
        }

        private void SpawnBuildingUIs()
        {
            TeamType[] teamTypes = (TeamType[])Enum.GetValues(typeof(TeamType));
            BuildingData[] buildingData = GameManager.Instance.GeneralData.BuildingData;
            
            for (int repeat = 0; repeat < 3; repeat++)
            {
                foreach (TeamType teamType in teamTypes)
                {
                    foreach (BuildingData data in buildingData)
                    {
                        BuildingUI ui = Instantiate(data.BuildingUIPrefab, _contentPanelTransform);
                        ui.Init(data, teamType);

                        if (_firstItem == null)
                            _firstItem = ui.GetComponent<RectTransform>();
                    }
                }
            }

            int repeatedItemCount = buildingData.Length * teamTypes.Length;
            float oneItemHeight = _firstItem.rect.height + _verticalLayoutGroup.spacing;

            _totalRepeatedItemHeight = repeatedItemCount * oneItemHeight;
            
            Vector2 pos = _contentPanelTransform.anchoredPosition;
            pos.y = _totalRepeatedItemHeight;
            _contentPanelTransform.anchoredPosition = pos;
            
            Canvas.ForceUpdateCanvases();
        }
    }
}