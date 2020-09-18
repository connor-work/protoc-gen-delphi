using System.IO;
using System.Linq;
using System.Reflection;

namespace protoc_gen_delphi.runtime_tests
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            // This approach is based on https://github.com/microsoft/vstest/issues/2200#issuecomment-590835049
            string testDLL = Assembly.GetExecutingAssembly().Location;
            string testDLLFolder = Path.GetDirectoryName(testDLL);
            Assembly vsTestConsole = Assembly.LoadFrom(Path.Join(testDLLFolder, @"vstest.console.dll"));
            MethodInfo vsTestMain = vsTestConsole.GetTypes().Where(type => type.Name == "Program").First()
                                                 .GetMethods().Where(method => method.Name == "Main").First();
            Directory.SetCurrentDirectory(testDLLFolder);
            return (int) vsTestMain.Invoke(null, new object[] { args.Append(testDLL).ToArray() });
        }
    }
}
