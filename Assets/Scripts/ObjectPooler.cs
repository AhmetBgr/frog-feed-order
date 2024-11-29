using System.Collections.Generic;
using UnityEngine;

public interface IPoolableObject {
    void OnObjectSpawn(); // Called when the object is activated

    void ReturnToPool(); 

}

public class ObjectPooler : MonoBehaviour {
    [System.Serializable]
    public class Pool {
        //public string tag;             // Identifier for the object type
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
                //objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.prefab.name, objectPool);
        }
    }

    void Start() {

    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Vector3 angles) {


        if (!poolDictionary.ContainsKey(tag)) {
            Debug.LogError($"Pool with tag {tag} does not exist.");
            return null;
        }

        List<GameObject> objectPool = poolDictionary[tag];

        // If the pool is empty, attempt to dynamically resize
        if (objectPool.Count == 0) {

            Debug.LogWarning($"Pool with tag {tag} is empty. Creating new object dynamically.");
            GameObject newObject = Instantiate(pools.Find(pool => pool.prefab.name == tag).prefab);
            newObject.name = newObject.name.Replace("(Clone)", "");
            newObject.SetActive(false);
            objectPool.Add(newObject);
            //objectPool.Enqueue(newObject); // Add the new object to the pool

            /*Pool poolConfig = pools.Find(pool => pool.prefab.name == tag);

            if (poolConfig != null) {
                Debug.LogWarning($"Pool with tag {tag} is empty. Creating new object dynamically.");
                GameObject newObject = Instantiate(poolConfig.prefab);
                newObject.name = newObject.name.Replace("(Clone)", "");
                newObject.SetActive(false);
                objectPool.Add(newObject);
                //objectPool.Enqueue(newObject); // Add the new object to the pool
            }
            else {
                Debug.LogWarning($"Pool with tag {tag} cannot be resized. Consider increasing its initial size.");
                return null;
            }*/
        }


        GameObject objectToSpawn = objectPool.Find(pool => pool.name == tag);
        poolDictionary[tag].Remove(objectToSpawn);

        //objectToSpawn.transform.localPosition = position;
        //objectToSpawn.transform.localEulerAngles = angles;
        //objectToSpawn.transform.GetChild(0).gameObject.SetActive(true);
        objectToSpawn.SetActive(true);

        // Call the interface method if applicable
        /*IPoolableObject poolable = objectToSpawn.GetComponent<IPoolableObject>();
        if (poolable != null) {
            poolable.OnObjectSpawn();
        }*/

        //poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public  void AddToPool(string tag, GameObject objectToReturn) {
        if (!poolDictionary.ContainsKey(tag)) {
            Debug.LogWarning($"Pool with tag {tag} does not exist. Cannot add object to pool.");
            objectToReturn.transform.SetParent(null);
            //Destroy(objectToReturn); // Destroy the object to avoid memory leaks
            return;
        }

        objectToReturn.transform.SetParent(transform);
        objectToReturn.SetActive(false);
        poolDictionary[tag].Add(objectToReturn);
    }
}
