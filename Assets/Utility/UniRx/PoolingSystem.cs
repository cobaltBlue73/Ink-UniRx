using System.Collections.Generic;
using UniRx;
using UniRx.Toolkit;
using UniRx.Triggers;
using UnityEngine;
using Utility.General;

namespace Utility.UniRx
{
    public static class PoolingSystem
    {
        public static GameObject Rent(this GameObject prefab) => Manager.Instance.Rent(prefab);

        public static void Return(this GameObject instance)
        {
            if( instance.TryGetComponent(typeof(Manager.PooledObject), out var pooledObject)) 
                (pooledObject as Manager.PooledObject)?.Return();
        }

        public static void InitializePool(this GameObject prefab) => Manager.Instance.InitPool(prefab);

        private class Manager
        {
            #region Internals

            public class Pool : ObjectPool<PooledObject>
            {
                private readonly GameObject _prefab;
                private readonly Transform _parent;
                
                public bool DontDestroyOnLoad { get; }

                public Pool(GameObject prefab, Transform parent = null, bool dontDestroyOnLoad = false)
                {
                    _prefab = prefab;
                    DontDestroyOnLoad = dontDestroyOnLoad;
                    _parent = parent ? parent : 
                        new GameObject($"{prefab.name}_{(DontDestroyOnLoad? "Persistent": "")}Pool").transform;
                    DontDestroyOnLoad = dontDestroyOnLoad;
                    if (dontDestroyOnLoad)
                    {
                        Object.DontDestroyOnLoad(_parent.gameObject);
                    }
                    else
                    {
                        _parent.OnDestroyAsObservable().Subscribe(_ => Dispose());
                    }
                }
                
                protected override PooledObject CreateInstance()
                {
                    var instance = Object.Instantiate(_prefab, _parent, true);
                    var pooledObject = instance.GetOrAddComponent<PooledObject>();
                    pooledObject.Pool = this;
                    return pooledObject;
                }

                protected override void OnBeforeReturn(PooledObject instance)
                {
                    base.OnBeforeReturn(instance);
                    instance.transform.SetParent(_parent);
                }

                protected override void OnBeforeRent(PooledObject instance)
                {
                    instance.transform.SetParent(null);
                    base.OnBeforeRent(instance);
                }

                protected override void OnClear(PooledObject instance)
                {
                    instance.transform.SetParent(null);
                    base.OnClear(instance);
                }

                protected override void Dispose(bool disposing)
                {
                    base.Dispose(disposing);
                    if (DontDestroyOnLoad)
                    {
                        Object.Destroy(_parent);
                    }
                    else
                    {
                        Manager.Instance._pools.Remove(_prefab.GetInstanceID());
                    }
                    
                }
            }
            
            public class PooledObject: MonoBehaviour
            {
                public Pool Pool;

                public void Return() => Pool?.Return(this);
            }

            #endregion
            
            #region Static
            public static Manager Instance => _instance ??= new Manager();
            private static Manager _instance;

            #endregion
        
            private Manager() {}

            #region Member Variables

            private readonly Dictionary<int, Pool> _pools =
                new Dictionary<int, Pool>();

            #endregion

            #region Methods

            #region Public

            public GameObject Rent(GameObject prefab) => GetPool(prefab).Rent().gameObject;

            public void Return(GameObject prefab, PooledObject instance) => GetPool(prefab).Return(instance);

            public Pool InitPool(GameObject prefab)
            {
                var pool = new Pool(prefab);
                _pools.Add(prefab.GetInstanceID(), pool);

                return pool;
            } 

            #endregion

            #region Private

            private Pool GetPool(GameObject prefab) =>
                _pools.TryGetValue(prefab.GetInstanceID(), out var pool) ?
                    pool : InitPool(prefab);

            #endregion

            #endregion


        }
    }
   
}