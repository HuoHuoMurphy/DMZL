using UnityEngine;
using System.Collections.Generic;

namespace PathologicalGames
{
    //�ع�����
    public static class PoolManager
    {
        public static readonly SpawnPoolsDict Pools = new SpawnPoolsDict();
    }

	
	//ʵ����&���ٶ���ʱ�Ĵ���
	public static class InstanceHandler  
	{
		public delegate GameObject InstantiateDelegate(GameObject prefab, Vector3 pos, Quaternion rot);
		public delegate void DestroyDelegate(GameObject instance);

        //��������
		public static InstantiateDelegate InstantiateDelegates;
	
        //���ٴ���
		public static DestroyDelegate DestroyDelegates;

        //����&ʵ����ĳ����
		internal static GameObject InstantiatePrefab(GameObject prefab, Vector3 pos, Quaternion rot)
		{
			if (InstanceHandler.InstantiateDelegates != null)
			{
				return InstanceHandler.InstantiateDelegates(prefab, pos, rot);
			}
			else
			{
				 return Object.Instantiate(prefab, pos, rot) as GameObject;
			}
		}
		
		//����ĳ����
		internal static void DestroyInstance(GameObject instance)
		{
			if (InstanceHandler.DestroyDelegates != null)
			{
				InstanceHandler.DestroyDelegates(instance);
			}
			else
			{
				Object.Destroy(instance);
			}
		}
	}


    public class SpawnPoolsDict : IDictionary<string, SpawnPool>
    {
		#region Event Handling
		public delegate void OnCreatedDelegate(SpawnPool pool);
		
		internal Dictionary<string, OnCreatedDelegate> onCreatedDelegates = 
			 new Dictionary<string, OnCreatedDelegate>();
		
        //��Ӵ�������
		public void AddOnCreatedDelegate(string poolName, OnCreatedDelegate createdDelegate)
		{
			if (!this.onCreatedDelegates.ContainsKey(poolName))
			{
				this.onCreatedDelegates.Add(poolName, createdDelegate);
				Debug.Log(string.Format(
					"Added onCreatedDelegates for pool '{0}': {1}", poolName, createdDelegate.Target)
				);
				return;
			}		
			this.onCreatedDelegates[poolName] += createdDelegate;
		}
		//�Ƴ�
		public void RemoveOnCreatedDelegate(string poolName, OnCreatedDelegate createdDelegate)
		{
			if (!this.onCreatedDelegates.ContainsKey(poolName))
				throw new KeyNotFoundException
				(
					"No OnCreatedDelegates found for pool name '" + poolName + "'."
				);
			
			this.onCreatedDelegates[poolName] -= createdDelegate;

			Debug.Log(string.Format(
				"Removed onCreatedDelegates for pool '{0}': {1}", poolName, createdDelegate.Target)
			);
		}
		
		#endregion Event Handling
		
        #region Public Custom Memebers
        //�����أ�˳�㴴��һ���ؽڵ㣩
        public SpawnPool Create(string poolName)
        {
            if (!this.assertValidPoolName(poolName))
                return null;
            var owner = new GameObject(poolName + "Pool");
            return owner.AddComponent<SpawnPool>();
        }
        //������
        public SpawnPool Create(string poolName, GameObject owner)
        {
            if (!this.assertValidPoolName(poolName))
                return null;
            string ownerName = owner.gameObject.name;
            try
            {
                owner.gameObject.name = poolName;
                return owner.AddComponent<SpawnPool>();
            }
            finally
            {
                owner.gameObject.name = ownerName;
            }
        }
        //�������Ƿ�ok�����ƺϷ��Լ������ڣ�
        private bool assertValidPoolName(string poolName)
        {
            string tmpPoolName;
            tmpPoolName = poolName.Replace("Pool", "");
            if (tmpPoolName != poolName)  // Warn if "Pool" was used in poolName
            {
                string msg = string.Format("'{0}' has the word 'Pool' in it. " +
                       "This word is reserved for GameObject defaul naming. " +
                       "The pool name has been changed to '{1}'",
                       poolName, tmpPoolName);

                Debug.LogWarning(msg);
                poolName = tmpPoolName;
            }

            if (this.ContainsKey(poolName))
            {
                Debug.Log(string.Format("A pool with the name '{0}' already exists",
                                        poolName));
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            var keysArray = new string[this._pools.Count];
            this._pools.Keys.CopyTo(keysArray, 0);
            return string.Format("[{0}]", System.String.Join(", ", keysArray));
        }
        //����һ����
        public bool Destroy(string poolName)
        {
            SpawnPool spawnPool;
            if (!this._pools.TryGetValue(poolName, out spawnPool))
            {
                Debug.LogError(
                    string.Format("PoolManager: Unable to destroy '{0}'. Not in PoolManager",
                                  poolName));
                return false;
            }

            UnityEngine.Object.Destroy(spawnPool.gameObject);
            this._pools.Remove(spawnPool.poolName);  
            return true;
        }
        //����ȫ����
        public void DestroyAll()
        {
            foreach (KeyValuePair<string, SpawnPool> pair in this._pools)
			{
                UnityEngine.Object.Destroy(pair.Value.gameObject);
			}
			this._pools.Clear(); 
        }
        #endregion Public Custom Memebers


        #region Dict Functionality
        // ���ֵ�
        private Dictionary<string, SpawnPool> _pools = new Dictionary<string, SpawnPool>();

        //��ӳ�
        internal void Add(SpawnPool spawnPool)
        {
            if (this.ContainsKey(spawnPool.poolName))
            {
                Debug.LogError(string.Format("A pool with the name '{0}' already exists. " +
                                                "This should only happen if a SpawnPool with " +
                                                "this name is added to a scene twice.",
                                             spawnPool.poolName));
                return;
            }
            this._pools.Add(spawnPool.poolName, spawnPool);
			Debug.Log(string.Format("Added pool '{0}'", spawnPool.poolName));
			if (this.onCreatedDelegates.ContainsKey(spawnPool.poolName))
				 this.onCreatedDelegates[spawnPool.poolName](spawnPool);
        }

        //��ӳ�
        public void Add(string key, SpawnPool value)
        {
            string msg = "SpawnPools add themselves to PoolManager.Pools when created, so " +
                         "there is no need to Add() them explicitly. Create pools using " +
                         "PoolManager.Pools.Create() or add a SpawnPool component to a " +
                         "GameObject.";
            throw new System.NotImplementedException(msg);
        }

        //�Ƴ�ĳ��
        internal bool Remove(SpawnPool spawnPool)
        {
            if (!this.ContainsValue(spawnPool) & Application.isPlaying)
            {
                Debug.LogError(string.Format(
					"PoolManager: Unable to remove '{0}'. Pool not in PoolManager",
                     spawnPool.poolName
				));
                return false;
            }

            this._pools.Remove(spawnPool.poolName);
            return true;
        }

        //�Ƴ�ĳ��
        public bool Remove(string poolName)
        {
            string msg = "SpawnPools can only be destroyed, not removed and kept alive" +
                         " outside of PoolManager. There are only 2 legal ways to destroy " +
                         "a SpawnPool: Destroy the GameObject directly, if you have a " +
                         "reference, or use PoolManager.Destroy(string poolName).";
            throw new System.NotImplementedException(msg);
        }

        //���������
        public int Count { get { return this._pools.Count; } }

        //ĳ�����ƻ�����Ƿ���ڣ�PoolManager.Pools.ContainsKey(poolName)��
        public bool ContainsKey(string poolName)
        {
            return this._pools.ContainsKey(poolName);
        }

        //ĳ��������Ƿ����
		public bool ContainsValue(SpawnPool pool)
		{
			return this._pools.ContainsValue(pool);
		}

        //��ͼȡ��ĳ�أ�PoolManager.Pools.TryGetValue(poolName, out pool)��
        public bool TryGetValue(string poolName, out SpawnPool spawnPool)
        {
            return this._pools.TryGetValue(poolName, out spawnPool);
        }

        #region Not Implimented
        public bool Contains(KeyValuePair<string, SpawnPool> item)
        {
			throw new System.NotImplementedException(
				"Use PoolManager.Pools.ContainsKey(string poolName) or " +
				"PoolManager.Pools.ContainsValue(SpawnPool pool) instead."
			);
        }

        public SpawnPool this[string key]
        {
            get
            {
                SpawnPool pool;
                try
                {
                    pool = this._pools[key];
                }
                catch (KeyNotFoundException)
                {
                    string msg = string.Format("A Pool with the name '{0}' not found. " +
                                                "\nPools={1}",
                                                key, this.ToString());
                    throw new KeyNotFoundException(msg);
                }

                return pool;
            }
            set
            {
                string msg = "Cannot set PoolManager.Pools[key] directly. " +
                    "SpawnPools add themselves to PoolManager.Pools when created, so " +
                    "there is no need to set them explicitly. Create pools using " +
                    "PoolManager.Pools.Create() or add a SpawnPool component to a " +
                    "GameObject.";
                throw new System.NotImplementedException(msg);
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                string msg = "If you need this, please request it.";
                throw new System.NotImplementedException(msg);
            }
        }


        public ICollection<SpawnPool> Values
        {
            get
            {
                string msg = "If you need this, please request it.";
                throw new System.NotImplementedException(msg);
            }
        }


        #region ICollection<KeyValuePair<string,SpawnPool>> Members
        private bool IsReadOnly { get { return true; } }
        bool ICollection<KeyValuePair<string, SpawnPool>>.IsReadOnly { get { return true; } }

        public void Add(KeyValuePair<string, SpawnPool> item)
        {
            string msg = "SpawnPools add themselves to PoolManager.Pools when created, so " +
                         "there is no need to Add() them explicitly. Create pools using " +
                         "PoolManager.Pools.Create() or add a SpawnPool component to a " +
                         "GameObject.";
            throw new System.NotImplementedException(msg);
        }

        public void Clear()
        {
            string msg = "Use PoolManager.Pools.DestroyAll() instead.";
            throw new System.NotImplementedException(msg);

        }

        private void CopyTo(KeyValuePair<string, SpawnPool>[] array, int arrayIndex)
        {
            string msg = "PoolManager.Pools cannot be copied";
            throw new System.NotImplementedException(msg);
        }

        void ICollection<KeyValuePair<string, SpawnPool>>.CopyTo(KeyValuePair<string, SpawnPool>[] array, int arrayIndex)
        {
            string msg = "PoolManager.Pools cannot be copied";
            throw new System.NotImplementedException(msg);
        }

        public bool Remove(KeyValuePair<string, SpawnPool> item)
        {
            string msg = "SpawnPools can only be destroyed, not removed and kept alive" +
                         " outside of PoolManager. There are only 2 legal ways to destroy " +
                         "a SpawnPool: Destroy the GameObject directly, if you have a " +
                         "reference, or use PoolManager.Destroy(string poolName).";
            throw new System.NotImplementedException(msg);
        }
        #endregion ICollection<KeyValuePair<string, SpawnPool>> Members
        #endregion Not Implimented




        #region IEnumerable<KeyValuePair<string,SpawnPool>> Members
        public IEnumerator<KeyValuePair<string, SpawnPool>> GetEnumerator()
        {
            return this._pools.GetEnumerator();
        }
        #endregion



        #region IEnumerable Members
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._pools.GetEnumerator();
        }
        #endregion

        #endregion Dict Functionality

    }

}
