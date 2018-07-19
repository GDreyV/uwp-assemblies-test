using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace AssembliesTest.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("I'm running at " + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromSameFolder);

            try
            {
                var value = Task.Run(() => JsonConvert.DeserializeObject("{\"name\": \"value\"}")).Result;
                System.Console.WriteLine(value);
            } catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }

            System.Console.ReadKey();
        }

        static Assembly LoadFromSameFolder(object sender, ResolveEventArgs args)
        {
            string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string assemblyPath = Path.Combine(folderPath, "..", new AssemblyName(args.Name).Name + ".dll");
            assemblyPath = Path.GetFullPath(assemblyPath);
            if (!File.Exists(assemblyPath)) return null;
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }
    }
}
