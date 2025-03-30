using NUnit.Framework;
using System.Reflection;
using System.Runtime.InteropServices;

// Налаштування для тестового проекту
[assembly: AssemblyDescription("Тести для проекту AssemblerLexer")]
[assembly: AssemblyCopyright("Copyright © 2023")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Налаштування NUnit
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(4)]