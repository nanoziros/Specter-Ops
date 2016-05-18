namespace  SpecterOps.Utilities
{
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// Generic pool system to use with any kind of gameobject
    /// </summary>
    public class GenericPoolSystem  {

        // List of pooled gameobjects 
        private List<GameObject> pooledObjects;

        // Control parameters
        private GameObject pooledObject;
        private Transform gameObjectParent;

        /// <summary>
        /// Constructor for creating a new object pool
        /// </summary>
        public GenericPoolSystem(GameObject obj, int initialPoolSize, Transform parent = null)
        {
            // Store common parent
            this.gameObjectParent = parent;

            // Store object to instance
            this.pooledObject = obj;

            // Instantiate a new list of game objects to store our pooled objects in
            this.pooledObjects = new List<GameObject>();

            // Create and add an object based on the provided initial size
            for (int i = 0; i < initialPoolSize; i++)
                this.AddObject(this.pooledObject);
        }

        /// <summary>
        /// Add a new obj to the pool
        /// </summary>
        private GameObject AddObject(GameObject obj)
        {
            // Instantiate pool gameobject
            GameObject nObj = GameObject.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;

            // Set  gameobject name
            nObj.name = nObj.name.Replace("(Clone)", "(Pooled)");

            // Make sure the object isn't active
            nObj.SetActive(false);

            // Set object parent
            nObj.transform.parent = this.gameObjectParent;

            // Add the object too our list
            this.pooledObjects.Add(nObj);

            // Return reference
            return nObj;
        }

        /// <summary>
        /// Returns an active object from the object pool without resetting any of its values so
        /// I will need to set its values and set it inactive again when you are done with it
        /// </summary>
        public GameObject GetObject()
        {
            // Iterate through all pooled objects
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                // Look for the first one that is inactive
                if (pooledObjects[i].activeSelf == false)
                {
                    //set the object to active
                    pooledObjects[i].SetActive(true);
                    //return the object we found
                    return pooledObjects[i];
                }
            }
            // If we make it this far, we obviously didn't find an inactive object
            // so we need to see if we can grow beyond our current count
            GameObject newObj = this.AddObject(this.pooledObject);

            // Set it to active since we are about to use it
            newObj.SetActive(true);
            
            // Return new object
            return newObj;
        }
    }
}
