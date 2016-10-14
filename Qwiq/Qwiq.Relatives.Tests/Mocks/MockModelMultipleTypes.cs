using Microsoft.Qwiq.Mapper.Attributes;

namespace Microsoft.Qwiq.Relatives.Tests.Mocks
{
    [WorkItemType("Fizz")]
    [WorkItemType("Baz")]
    [WorkItemType("Buzz")]
    public class MockModelMultipleTypes
    {
        [FieldDefinition("ID")]
        public int Id { get; internal set; }

        [FieldDefinition("Priority")]
        public int Priority { get; internal set; }
    }
}

