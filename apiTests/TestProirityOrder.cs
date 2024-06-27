using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace apiTests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PriorityAttribute : Attribute
    {
        public int Priority { get; private set; }

        public PriorityAttribute(int priority)
        {
            Priority = priority;
        }
    }
    public class TestPriorityOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
            where TTestCase : ITestCase
        {
            var sortedCases = testCases.Select(testCase =>
            {
                var priority = testCase.TestMethod.Method.GetCustomAttributes(typeof(PriorityAttribute).AssemblyQualifiedName)
                    .FirstOrDefault()?.GetNamedArgument<int>("Priority") ?? 0;
                return new { testCase, priority };
            })
            .OrderBy(tc => tc.priority)
            .Select(tc => tc.testCase);

            return sortedCases;
        }
    }
}