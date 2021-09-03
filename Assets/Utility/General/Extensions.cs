using UnityEngine;

namespace Utility.General
{
    public static class Extensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T: Component
        {
            if (!gameObject.TryGetComponent(typeof(T), out var component))
            {
                component = gameObject.AddComponent<T>();
            }

            return component as T;
        }
        
        public static T GetOrAddComponent<T>(this MonoBehaviour monoBehaviour) where T: Component
        {
            if (!monoBehaviour.TryGetComponent(typeof(T), out var component))
            {
                component = monoBehaviour.gameObject.AddComponent<T>();
            }

            return component as T;
        }
    }
}