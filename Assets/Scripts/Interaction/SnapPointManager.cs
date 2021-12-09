using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class SnapPointManager : MonoBehaviour
    {
        public static SnapPointManager instance;
        private InventorySystem playerInventory;
        [SerializeField] private GameObject snapPrefab;

        private void Awake()
        {
            if (instance == null) instance = this;
        }

        private void OnUpdateInventory()
        {
            foreach (Transform _t in transform)
            {
                Destroy(_t.gameObject);
            }

            PopulateSlots();
        }


        private void PopulateSlots()
        {
            if (playerInventory.inventory.Count == 0) return;

            List<BehaviorStateManager> _interactables = new List<BehaviorStateManager>();

            foreach (InventoryItem _item in playerInventory.inventory)
            {
                int _index = playerInventory.inventory.IndexOf(_item);
                GameObject _snapObject = Instantiate(snapPrefab);
                _snapObject.transform.SetParent(transform, false);

                SnapPoint _snap = _snapObject.GetComponent<SnapPoint>();

                GameObject _displayItem = Instantiate(_item.data.Prefab);
                _interactables.Add(_displayItem.GetComponent<BehaviorStateManager>());
                _interactables[_index].SetNonInteractive();
                _interactables[_index].TurnOffWorldIcon();

                if (_index == 0) _snap.transform.localPosition = Vector3.zero;
                else _snap.SetNextPoint(_interactables[_index - 1].UpperSnap); //set the snap point using the last item

                _snap.AddChild(_displayItem.transform);
            }
        }

        private void Start()
        {
            playerInventory = InventorySystem.PlayerInventory;
            playerInventory.OnInventoryUpdate += OnUpdateInventory;
        }
        
        private void OnDestroy()
        {
            playerInventory.OnInventoryUpdate -= OnUpdateInventory;
        }

    }
}