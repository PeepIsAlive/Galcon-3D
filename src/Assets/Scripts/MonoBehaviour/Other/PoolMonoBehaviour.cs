using UnityEngine;
using System.Collections.Generic;

namespace Galcon
{
    public sealed class PoolPlanetsMonoBehaviour<T> where T : MonoBehaviour
    {
        private readonly List<T> _pool;
        private readonly Vector3 _spawnPosition;
        private readonly T _prefab;

        public IEnumerable<T> List => _pool;
        public T this[int index] => _pool[index];
        public int ObjectsAmount => _pool.Count;

        public PoolPlanetsMonoBehaviour(IEnumerable<T> list, Vector3 spawnPosition, bool stateObjects)
        {
            _pool = new List<T>();
            _spawnPosition = spawnPosition;

            AddObjectToPool(list);
        }

        public PoolPlanetsMonoBehaviour(T prefab, Vector3 spawnPosition, int objectAmount, bool stateObjects)
        {
            _pool = new List<T>();
            _prefab = prefab;
            _spawnPosition = spawnPosition;

            PoolCreate(objectAmount, stateObjects);
        }

        public T GetObject(bool state = false)
        {
            if (HasFreeObject(out T freeObject, state)) { return freeObject; }
            else { return CreateObject(state); }
        }

        private bool HasFreeObject(out T freeObject, bool state)
        {
            freeObject = null;

            foreach (var poolObject in _pool)
            {
                if (!poolObject.gameObject.activeInHierarchy)
                {
                    freeObject = poolObject;
                    freeObject.gameObject.SetActive(state);

                    return true;
                }
            }

            return false;
        }

        private void AddObjectToPool(IEnumerable<T> list, bool state = false)
        {
            foreach (T obj in list)
            {
                obj.gameObject.SetActive(false);
                _pool.Add(obj);
            }
        }

        private void PoolCreate(int objectAmount, bool stateObjects)
        {
            for (int i = 0; i < objectAmount; ++i)
            {
                CreateObject(stateObjects);
            }
        }

        private T CreateObject(bool defaultState = false)
        {
            var poolObject = Object.Instantiate(_prefab, _spawnPosition, Quaternion.identity);

            poolObject.gameObject.SetActive(defaultState);
            _pool.Add(poolObject);

            return poolObject;
        }
    }
}