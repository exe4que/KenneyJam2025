using System.Collections.Generic;
using UnityEngine;

namespace KenneyJam2025
{
    //Super simple pool manager
    public class PoolManager : Singleton<PoolManager>
    {
        [SerializeField] private List<PoolItem> _poolItems;
        private readonly Dictionary<string, Queue<GameObject>> _instances = new();
        
        protected override void Awake()
        {
            base.Awake();
            InitializePools();
        }
        
        private void InitializePools()
        {
            foreach (var item in _poolItems)
            {
                if (item.Prefab == null) continue;

                GameObject parent = new GameObject($"{item.Prefab.name} Pool");
                parent.transform.SetParent(transform); // Set the parent to the PoolManager's transform
                Queue<GameObject> queue = new Queue<GameObject>();
                for (int i = 0; i < item.InitialSize; i++)
                {
                    GameObject instance = Instantiate(item.Prefab, parent.transform);
                    instance.name = item.Prefab.name; // Ensure the name is set correctly
                    instance.SetActive(false);
                    queue.Enqueue(instance);
                }
                _instances[item.Prefab.name] = queue;
            }
        }
        
        public GameObject GetInstance(string prefabName)
        {
            if (_instances.TryGetValue(prefabName, out Queue<GameObject> queue))
            {
                if (queue.Count > 0)
                {
                    GameObject instance = queue.Dequeue();
                    instance.SetActive(true);
                    //Debug.Log($"Retrieved instance from pool: {prefabName}");
                    return instance;
                }
                else
                {
                    for (int i = 0; i < _poolItems.Count; i++)
                    {
                        if (_poolItems[i].Prefab.name == prefabName)
                        {
                            GameObject newInstance = Instantiate(_poolItems[i].Prefab);
                            newInstance.SetActive(true);
                            _instances[prefabName].Enqueue(newInstance);
                            //Debug.Log($"Created new instance for pool: {prefabName}");
                            return newInstance;
                        }
                    }
                }
            }
            Debug.LogWarning($"No pool found for prefab: {prefabName}");
            return null;
        }
        
        public void ReturnInstance(GameObject instance)
        {
            if (instance == null) return;

            string prefabName = instance.name;
            if (_instances.TryGetValue(prefabName, out Queue<GameObject> queue))
            {
                instance.SetActive(false);
                queue.Enqueue(instance);
            }
            else
            {
                Debug.LogWarning($"No pool found for prefab: {prefabName}, destroying instance.");
                Destroy(instance);
            }
        }
        
        
        [System.Serializable]
        public class PoolItem
        {
            public GameObject Prefab;
            public int InitialSize;
        }
    }
}