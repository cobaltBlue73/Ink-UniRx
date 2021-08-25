using System;
using UniRx;

namespace Utility.UniRx
{
    public static class Extensions
    {
        public static IDisposable SetAndSubscribe<T>(this ReactiveProperty<T> prop, ref T variable, Action<T> onValueChanged)
        {
            if(prop.HasValue) variable = prop.Value;
            return prop.Subscribe(onValueChanged);
        }
        
        public static IDisposable SetAndSubscribe<T>(this IReactiveProperty<T> prop, ref T variable, Action<T> onValueChanged)
        {
            if(prop.HasValue) variable = prop.Value;
            return prop.Subscribe(onValueChanged);
        }
    }
}