using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using DbUp.ScriptProviders;

namespace DbUp
{
	class Program
	{
		static void Main(string[] args)
		{
			var connectionstring = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
			var scriptProvider = new CustomScriptProvider(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Scripts");

			//var upgrader = new DatabaseUpgrader(connectionstring, new EmbeddedScriptProvider(typeof(Program).Assembly));
			var upgrader = new DatabaseUpgrader(connectionstring, scriptProvider);

			var result = upgrader.PerformUpgrade();
			if (result.Successful)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Success!");
				Console.ReadKey();
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(result.Error);
				Console.ReadKey();
				Console.WriteLine("Failed!");
			}
		}
	}

	sealed class CustomScriptProvider : IScriptProvider
	{
		private readonly string _scriptLocation;

		public CustomScriptProvider(string scriptLocation)
		{
			_scriptLocation = scriptLocation;
		}

		public IEnumerable<SqlScript> GetScripts()
		{

			if (!Directory.Exists(_scriptLocation))
			{
				throw new Exception("Unable to find folder : " + _scriptLocation);
			}

			var listOfScripts = new List<SqlScript>();

			var sqlScripts = Directory.EnumerateFiles(_scriptLocation);

			foreach (var sqlScriptFile in sqlScripts)
			{
				var fileContents = File.ReadAllText(sqlScriptFile);
				var fileName = Path.GetFileName(sqlScriptFile);

				var sqlScript = new SqlScript(fileName, fileContents);

				listOfScripts.Add(sqlScript);

			}

			return listOfScripts;
		}
	}
}
