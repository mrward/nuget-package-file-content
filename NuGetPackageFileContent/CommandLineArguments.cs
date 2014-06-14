
using System;

namespace NuGetPackageFileContent
{
	public class CommandLineArguments
	{
		public CommandLineArguments()
		{
			Take = 100;
			FileExtension = String.Empty;
			Url = "http://nuget.org/api/v2/";
		}
		
		public string FileExtension { get; private set; }
		public int Skip { get; private set; }
		public int Take { get; private set; }
		public string Url { get; private set; }
		
		public void PrintUsage()
		{
			Console.WriteLine("NuGetPackageFileContent file-extension [-url:uri] [-skip:number] [-take:number]");
		}
		
		public bool Parse(string[] args)
		{
			if (args.Length == 0) {
				return false;
			}
			
			FileExtension = args[0];
			EnsureFileExtensionStartsWithDot();
			
			for (int i = 1; i < args.Length; ++i) {
				if (!ParseNamedArgument(args[i])) {
					return false;
				}
			}
			
			return true;
		}
		
		bool ParseNamedArgument(string arg)
		{
			if (IsSkipArgument(arg)) {
				return ParseSkipArgument(arg);
			} else if (IsTakeArgument(arg)) {
				return ParseTakeArgument(arg);
			} else if (IsUrlArgument(arg)) {
				return ParseUrlArgument(arg);
			}
			return false;
		}
		
		bool ParseTakeArgument(string arg)
		{
			int? argumentValue = GetIntegerArgumentValue(arg);
			if (argumentValue.HasValue) {
				Take = argumentValue.Value;
				return true;
			}
			
			return false;
		}
		
		int? GetIntegerArgumentValue(string arg)
		{
			int index = arg.IndexOf(':');
			if (index > 0) {
				string integer = arg.Substring(index + 1);
				int value = 0;
				if (Int32.TryParse(integer, out value)) {
					return value;
				}
				return null;
			}
			return null;
		}
		
		bool ParseSkipArgument(string arg)
		{
			int? argumentValue = GetIntegerArgumentValue(arg);
			if (argumentValue.HasValue) {
				Skip = argumentValue.Value;
				return true;
			}
			
			return false;
		}
		
		bool IsSkipArgument(string arg)
		{
			return arg.StartsWith("-skip:", StringComparison.OrdinalIgnoreCase);
		}
		
		bool IsTakeArgument(string arg)
		{
			return arg.StartsWith("-take:", StringComparison.OrdinalIgnoreCase);
		}
		
		void EnsureFileExtensionStartsWithDot()
		{
			if (!FileExtension.StartsWith(".")) {
				FileExtension = "." + FileExtension;
			}
		}
		
		bool ParseUrlArgument(string arg)
		{
			int index = arg.IndexOf(':');
			if (index > 0) {
				Url = arg.Substring(index + 1);
				return Url.Length > 0;
			}
			return false;
		}
		
		bool IsUrlArgument(string arg)
		{
			return arg.StartsWith("-url:", StringComparison.OrdinalIgnoreCase);
		}
	}
}
