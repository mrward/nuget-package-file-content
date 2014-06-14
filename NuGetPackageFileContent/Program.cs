// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.IO;
using System.Linq;

using NuGet;

namespace NuGetPackageFileContent
{
	class Program
	{
		public static void Main(string[] args)
		{
			try {
				var program = new Program();
				program.Run(args);
			} catch (Exception ex) {
				Console.WriteLine(ex);
			}
		}
		
		void Run(string[] args)
		{
			var arguments = new CommandLineArguments();
			if (!arguments.Parse(args)) {
				arguments.PrintUsage();
				return;
			}
			
			FindPackages(arguments);
		}
		
		void FindPackages(CommandLineArguments arguments)
		{
			var repository = PackageRepositoryFactory.Default.CreateRepository(arguments.Url);
			foreach (IPackage package in repository
				.Search(null, false)
				.Where (p => p.IsLatestVersion)
				.OrderByDescending(p => p.DownloadCount)
				.Skip(arguments.Skip)
				.Take(arguments.Take)) {
				
				foreach (IPackageFile file in package.GetContentFiles()) {
					string extension = Path.GetExtension(file.Path);
					if (arguments.FileExtension.Equals(extension, StringComparison.OrdinalIgnoreCase)) {
						Console.WriteLine("Package.Id: " + package.Id);
						break;
					}
				}
			}
		}
	}
}