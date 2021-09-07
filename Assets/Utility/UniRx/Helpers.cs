using UniRx;
using UnityEngine;

namespace Utility.UniRx
{
    public static class Helpers
    {
        public static ReactiveProperty<int> GetIntegerPlayerPrefAsReactiveProperty(string key, int defaultValue = 0, CompositeDisposable disposables = null)
        {
            var prop = new ReactiveProperty<int>(PlayerPrefs.GetInt(key, defaultValue));
            var disposable = prop.Subscribe(val => PlayerPrefs.SetInt(key, val));
            disposables?.Add(disposable);
            return prop;
        }
        
        public static ReactiveProperty<float> GetFloatPlayerPrefAsReactiveProperty(string key, float defaultValue = 0, CompositeDisposable disposables = null)
        {
            var prop = new ReactiveProperty<float>(PlayerPrefs.GetFloat(key, defaultValue));
            var disposable = prop.Subscribe(val => PlayerPrefs.SetFloat(key, val));
            disposables?.Add(disposable);
            return prop;
        }
        
        public static ReactiveProperty<string> GetStringPlayerPrefAsReactiveProperty(string key, string defaultValue = "", CompositeDisposable disposables = null)
        {
            var prop = new ReactiveProperty<string>(PlayerPrefs.GetString(key, defaultValue));
            var disposable = prop.Subscribe(val => PlayerPrefs.SetString(key, val));
            disposables?.Add(disposable);
            return prop;
        }
        
        public static ReactiveProperty<bool> GetBooleanPlayerPrefAsReactiveProperty(string key, bool defaultValue = false, CompositeDisposable disposables = null)
        {
            var prop = new ReactiveProperty<bool>(PlayerPrefs.GetInt(key, defaultValue? 1: 0) == 1);
            var disposable = prop.Subscribe(val => PlayerPrefs.SetInt(key, val? 1: 0));
            disposables?.Add(disposable);
            return prop;
        }
        
        public static ReactiveProperty<int> GetIntegerPlayerPrefAsReactiveProperty(string key, IntReactiveProperty defaultValue, CompositeDisposable disposables = null)
        {
            var prop = new ReactiveProperty<int>(PlayerPrefs.GetInt(key, defaultValue.Value));
            var disposable = prop.Subscribe(val => PlayerPrefs.SetInt(key, val)).AddTo(disposables);
            disposables?.Add(disposable);
            disposable = defaultValue.Subscribe(prop.SetValueAndForceNotify);
            disposables?.Add(disposable);
            return prop;
        }
        
        public static ReactiveProperty<float> GetFloatPlayerPrefAsReactiveProperty(string key, FloatReactiveProperty defaultValue, CompositeDisposable disposables = null)
        {
            var prop = new ReactiveProperty<float>(PlayerPrefs.GetFloat(key, defaultValue.Value));
            var disposable = prop.Subscribe(val => PlayerPrefs.SetFloat(key, val));
            disposables?.Add(disposable);
            disposable = defaultValue.Subscribe(prop.SetValueAndForceNotify);
            disposables?.Add(disposable);
            return prop;
        }
        
        public static ReactiveProperty<string> GetStringPlayerPrefAsReactiveProperty(string key, StringReactiveProperty defaultValue, CompositeDisposable disposables = null)
        {
            var prop = new ReactiveProperty<string>(PlayerPrefs.GetString(key, defaultValue.Value));
            var disposable = prop.Subscribe(val => PlayerPrefs.SetString(key, val));
            disposables?.Add(disposable);
            disposable = defaultValue.Subscribe(prop.SetValueAndForceNotify);
            disposables?.Add(disposable);
            return prop;
        }
        
        public static ReactiveProperty<bool> GetBooleanPlayerPrefAsReactiveProperty(string key, BoolReactiveProperty defaultValue, CompositeDisposable disposables = null)
        {
            var prop = new ReactiveProperty<bool>(PlayerPrefs.GetInt(key, defaultValue.Value? 1: 0) == 1);
            var disposable = prop.Subscribe(val => PlayerPrefs.SetInt(key, val? 1: 0));
            disposables?.Add(disposable);
            disposable = defaultValue.Subscribe(prop.SetValueAndForceNotify);
            disposables?.Add(disposable);
            return prop;
        }
    }
}