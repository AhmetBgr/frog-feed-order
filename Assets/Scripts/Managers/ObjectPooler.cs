using System.Collections.Generic;
using UnityEngine;

public interface IPoolableObject {
    void OnObjectSpawn(); // Called when the object is spawned when loading a level

    void ReturnToPool(); 

}

public class ObjectPooler : MonoBehaviour {
    [System.Serializable]
    public class Pool {
        public GameObject prefab;      // Prefab to instantiate
        public int size;               // Number of objects to preload
    }

    public List<Pool> pools;
    public  Dictionary<string, List<GameObject>> poolDictionary;

    public static ObjectPooler instance;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }
        //DontDestroyOnLoad(this.gameObject);

        poolDictionary = new Dictionary<string, List<GameObject>>();

        // Initialize each pool
        foreach (Pool pool in pools) {
            List<GameObject> objectPool = new List<GameObject>();

            for (int i = 0; i < pool.size; i++) {
                GameObject obj = Instantiate(pool.prefab);
                obj.name = obj.name.Replace("(Clone)", "");
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                objectPool.Add(obj);
            }

            poolDictionary.Add(pool.prefab.name, objectPool);
        }
    }

    public GameObject SpawnFromPool(string name) {
        if (poolDictionary == null) 
            poolDictionary = new Dictionary<string, List<GameObject>>();

        // Checks if pool exists
        if (!poolDictionary.ContainsKey(name)) {
            // Generate new pool with given name
            List<GameObject> pool = new List<GameObject>();
            poolDictionary.Add(name, pool);
            Debug.LogWarning($"Pool with tag {name} does not exist. Creating the pool.");
        }

        List<GameObject> objectPool = poolDictionary[name];

        // If the pool is empty, attempt to dynamically resize it
        if (objectPool.Count == 0) {

            Debug.LogWarning($"Pool with tag {name} is empty. Creating new object dynamically.");
            GameObject newObject = Instantiate(pools.Find(pool => pool.prefab.name == name).prefab);
            newObject.name = newObject.name.Replace("(Clone)", "");
            newObject.SetActive(false);
            poolDictionary[name].Add(newObject);
        }


        GameObject objectToSpawn = poolDictionary[name].Find(pool => pool.name == name);
        poolDictionary[name].Remove(objectToSpawn);

        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    public  void AddToPool(string tag, GameObject objectToReturn) {
        if (!poolDictionary.ContainsKey(tag)) {
            Debug.LogWarning($"Pool with tag {tag} does not exist. Cannot add object to pool.");
            objectToReturn.transform.SetParent(null);
            //Destroy(objectToReturn); // Destroy the object to avoid memory leaks
            return;
        }

        poolDictionary[tag].Add(objectToReturn);

        objectToReturn.transform.SetParent(transform);
        objectToReturn.transform.localPosition = Vector3.zero;
        objectToReturn.transform.localEulerAngles = Vector3.zero;

        objectToReturn.SetActive(false);
    }
}
