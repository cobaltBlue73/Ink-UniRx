using System;
using Cysharp.Threading.Tasks;

namespace InkUniRx
{
    public interface IStoryContinueOverride
    {
        public UniTask WaitForContinueAsync(Func<UniTask> defaultContinue);
    }
}