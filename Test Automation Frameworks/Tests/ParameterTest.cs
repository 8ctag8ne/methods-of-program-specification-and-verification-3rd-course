// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using NUnit.Framework;
// using Test_Automation_Frameworks.Utilities;


// namespace Test_Automation_Frameworks.Tests
// {
//     [TestFixture]
//     [Parallelizable(ParallelScope.Self)]
//     public class ParameterTest : BaseTest
//     {
//         [Test]
//         public void ParamTest()
//         {
//             string browser = GetBrowserFromEnvironment();
//             Console.WriteLine($"Selected browser: {browser}");
//             Assert.Pass($"Browser {browser} was selected");
//         }
//     }
// }