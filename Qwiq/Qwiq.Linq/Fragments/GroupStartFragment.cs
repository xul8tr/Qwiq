using System;

namespace Microsoft.Qwiq.Linq.Fragments
{
    internal class GroupStartFragment : IFragment
    {
        public string Get(Type queryType)
        {
            return "(";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}

