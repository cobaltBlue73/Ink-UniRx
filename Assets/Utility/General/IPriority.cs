using System;

namespace Utility.General
{
    public interface IPriority: IComparable<IPriority>
    {
        public int Priority { get; }
    }
}