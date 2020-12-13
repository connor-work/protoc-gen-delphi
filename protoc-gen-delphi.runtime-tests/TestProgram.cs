/// Copyright 2020 Connor Roehricht (connor.work)
/// Copyright 2020 Sotax AG
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// 
///     http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi.RuntimeTests
{
    public static class TestProgram
    {
        public static int Main(string[] args)
        {
            if (args.Length != 0) throw new ArgumentException("Invalid number of arguments");
            // This approach is based on https://github.com/microsoft/vstest/issues/2200#issuecomment-590835049
            string testDLL = Assembly.GetExecutingAssembly().Location;
            string testDLLFolder = Path.GetDirectoryName(testDLL)!;
            Assembly vsTestConsole = Assembly.LoadFrom(Path.Join(testDLLFolder, "vstest.console.dll"));
            MethodInfo vsTestMain = vsTestConsole.GetTypes().Where(type => type.Name == "Program").First()
                                                 .GetMethods().Where(method => method.Name == "Main").First();
            if (vsTestMain == null) throw new InvalidOperationException("Missing VS Test main");
            Directory.SetCurrentDirectory(testDLLFolder);
            return (int) vsTestMain.Invoke(null, new object[] { new string[] { testDLL } })!;
        }
    }
}
