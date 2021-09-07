using System;
using UniRx;
using UnityEngine;

namespace Utility.UniRx
{
    public static class ReactivePropertyExtensions
    {
        public static IDisposable InitializeAndBindPlayerPref(this IReactiveProperty<int> prop, string key, int defaultValue = 0)
        {
            prop.Value = PlayerPrefs.GetInt(key, defaultValue);
            return prop.Subscribe(val => PlayerPrefs.SetInt(key, val));
        }
        
        public static IDisposable InitializeAndBindPlayerPref(this IReactiveProperty<float> prop, string key, float defaultValue = 0)
        {
            prop.Value = PlayerPrefs.GetFloat(key, defaultValue);
            return prop.Subscribe(val => PlayerPrefs.SetFloat(key, val));
        }
        
        public static IDisposable InitializeAndBindPlayerPref(this IReactiveProperty<string> prop, string key, string defaultValue = "")
        {
            prop.Value = PlayerPrefs.GetString(key, defaultValue);
            return prop.Subscribe(val => PlayerPrefs.SetString(key, val));
        }
        
        public static IDisposable InitializeAndBindPlayerPref(this IReactiveProperty<bool> prop, string key, bool defaultValue = false)
        {
            prop.Value = PlayerPrefs.GetInt(key, defaultValue? 1: 0) == 1;
            return prop.Subscribe(val => PlayerPrefs.SetInt(key, val? 1: 0));
        }
        
        public static IReactiveProperty<int> AsPlayerPrefReactiveProperty(this ReactiveProperty<int> defaultValue, string key, CompositeDisposable disposables = null)
        {
            var prop = new ReactiveProperty<int>();
            var disposable = prop.InitializeAndBindPlayerPref(key, defaultValue.Value);
            disposables?.Add(disposable);
            disposable = defaultValue.Subscribe(prop.SetValueAndForceNotify);
            disposables?.Add(disposable);
            return prop;
        }
        
        public static IReactiveProperty<float> AsPlayerPrefReactiveProperty(this ReactiveProperty<float> defaultValue, string key, CompositeDisposable disposables = null)
        {
            var prop = new ReactiveProperty<float>();
            var disposable = prop.InitializeAndBindPlayerPref(key, defaultValue.Value);
            disposables?.Add(disposable);
            disposable = defaultValue.Subscribe(prop.SetValueAndForceNotify);
            disposables?.Add(disposable);
            return prop;
        }
        
        public static IReactiveProperty<string> AsPlayerPrefReactiveProperty(this ReactiveProperty<string> defaultValue, string key, CompositeDisposable disposables = null)
        {
            var prop = new ReactiveProperty<string>();
            var disposable = prop.InitializeAndBindPlayerPref(key, defaultValue.Value);
            disposables?.Add(disposable);
            disposable = defaultValue.Subscribe(prop.SetValueAndForceNotify);
            disposables?.Add(disposable);
            return prop;
        }
        
        public static IReactiveProperty<bool> AsPlayerPrefReactiveProperty(this ReactiveProperty<bool> defaultValue, string key, CompositeDisposable disposables = null)
        {
            var prop = new ReactiveProperty<bool>();
            var disposable = prop.InitializeAndBindPlayerPref(key, defaultValue.Value);
            disposables?.Add(disposable);
            disposable = defaultValue.Subscribe(prop.SetValueAndForceNotify);
            disposables?.Add(disposable);
            return prop;
        }
    }
}