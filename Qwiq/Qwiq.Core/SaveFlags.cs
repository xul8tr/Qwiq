using System;

namespace Microsoft.Qwiq
{
    [Flags]
    public enum SaveFlags
    {
        None = 0,
        MergeLinks = 1,
        MergeAll = 2,
    }
}
