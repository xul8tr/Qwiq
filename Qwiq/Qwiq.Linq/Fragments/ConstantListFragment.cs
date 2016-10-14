using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Qwiq.Linq.Fragments
{
    internal class ConstantListFragment : ListFragment
    {
        public ConstantListFragment(IEnumerable<string> strings)
            : base(strings.Select(s => new ConstantFragment(s)).ToArray())
        {
        }
    }
}

