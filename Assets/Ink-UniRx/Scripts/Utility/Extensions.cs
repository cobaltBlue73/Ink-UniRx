using System;
using Ink.Runtime;
using UniRx;

namespace InkUniRx
{
    public static class Extensions
    {
        public static void ChooseChoice(this Story story, Choice choice) => story.ChooseChoiceIndex(choice.index);
        public static bool HasChoices(this Story story) => story.currentChoices.Count > 0;
        
        public static bool HasEnded(this Story story) => !story.canContinue && !story.HasChoices();

        public static string GetCurrentPathName(this Story story) => story.state.currentPathString;
        
        public static int GetCurrentPathVisitCount(this Story story) => story.state.GetCurrentPathVisitCount();

        private static int GetCurrentPathVisitCount(this StoryState state) =>
            state.VisitCountAtPathString(state.currentPathString);

        public static IObservable<Unit> OnContinueAsObservable(this Story story) => 
            Observable.FromEvent(onContinue=> story.onDidContinue += onContinue, 
                onContinue=> story.onDidContinue += onContinue);
        
        public static IObservable<Choice> OnMakeChoiceAsObservable(this Story story) =>
            Observable.FromEvent<Choice>(onSelected => story.onMakeChoice += onSelected, 
                onSelected => story.onMakeChoice -= onSelected);

        public struct InkError
        {
            public string message;
            public Ink.ErrorType type;
        }

        public static IObservable<InkError> OnErrorAsObservable(this Story story) =>
            Observable.FromEvent<Ink.ErrorHandler, InkError>( onError => 
                    (message, type) => onError(new InkError { message = message, type = type }),
                onError=> story.onError += onError, onError=> story.onError -= onError);

        public struct ChoosePathStringArgs
        {
            public string path;
            public object[] args;
        }
        
        public static IObservable<ChoosePathStringArgs> OnChoosePathStringAsObservable(this Story story) =>
            Observable.FromEvent<Action<string, object[]>, ChoosePathStringArgs>( onChoosePathString => 
                    (path, args) => onChoosePathString(new ChoosePathStringArgs { path = path, args = args }), 
                onChoosePathString=> story.onChoosePathString += onChoosePathString, 
                onChoosePathString=> story.onChoosePathString -= onChoosePathString);

        public struct EvaluateFunctionArgs
        {
            public string functionName;
            public object[] args;
        }
        
        public static IObservable<EvaluateFunctionArgs> OnEvaluateFunctionAsObservable(this Story story) =>
            Observable.FromEvent<Action<string, object[]>, EvaluateFunctionArgs>( onEvaluateFunction => 
                    (functionName, args) => 
                        onEvaluateFunction(new EvaluateFunctionArgs { functionName = functionName, args = args }),
                onEvaluateFunction=> story.onEvaluateFunction += onEvaluateFunction, 
                onEvaluateFunction=> story.onEvaluateFunction -= onEvaluateFunction);
        
        public struct CompleteEvaluateFunctionArgs
        {
            public string functionName;
            public object[] args;
            public string textOutput;
            public object result;
        }
        
        public static IObservable<CompleteEvaluateFunctionArgs> OnCompleteEvaluateFunctionAsObservable(this Story story) =>
            Observable.FromEvent<Action<string, object[], string, object>, CompleteEvaluateFunctionArgs>( onCompleteEvaluateFunction => 
                    (functionName, args, textOutput, result) => 
                        onCompleteEvaluateFunction(new CompleteEvaluateFunctionArgs
                        { functionName = functionName, args = args, textOutput = textOutput, result = result }),
                onCompleteEvaluateFunction=> story.onCompleteEvaluateFunction += onCompleteEvaluateFunction, 
                onCompleteEvaluateFunction=> story.onCompleteEvaluateFunction -= onCompleteEvaluateFunction);

        public static IObservable<Unit> OnExternalFunctionAsObservable(this Story story, string funcName, bool lookAheadSafe = false) =>
            Observable.FromEvent(onExternalFunction => story.BindExternalFunction(funcName, onExternalFunction, lookAheadSafe), 
                _=> story.UnbindExternalFunction(funcName));
        
        public static IObservable<T> OnExternalFunctionAsObservable<T>(this Story story, string funcName, bool lookAheadSafe = false) where T: struct => 
            Observable.FromEvent<T>(onExternalFunction => story.BindExternalFunction(funcName, onExternalFunction, lookAheadSafe), 
                _=> story.UnbindExternalFunction(funcName));
        
        
        public static IObservable<object[]> OnExternalFunctionGeneralAsObservable(this Story story, string funcName, bool lookAheadSafe = true) =>
            Observable.FromEvent<Story.ExternalFunction, object[]>(onExternalFunction => 
                    args => { 
                        onExternalFunction(args); 
                        return null; 
                    }, 
                onExternalFunction => story.BindExternalFunctionGeneral(funcName, onExternalFunction, lookAheadSafe), 
                _=> story.UnbindExternalFunction(funcName));

        public static ReactiveProperty<T> GetVariableAsReactiveProperty<T>(this Story story, string varName)
        {
            var prop = new ReactiveProperty<T>((T)story.variablesState[varName]);
            prop.Subscribe(value => story.variablesState[varName] = value);
            void Observer(string _, object value) => prop.Value = (T)value;
            story.ObserveVariable(varName, Observer);
            prop.DoOnCompleted(() => story.RemoveVariableObserver(Observer, varName));
            
            return prop;
        }
        
        public static ReactiveDictionary<InkListItem, int> GetListAsReactiveCollection(this Story story, string listName)
        {
            var inkList = story.variablesState[listName] as InkList;
            var dictionary = inkList.ToReactiveDictionary();
            
            return dictionary;
        }


    }
}