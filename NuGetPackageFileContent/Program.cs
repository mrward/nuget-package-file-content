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
				program.Run();
			} catch (Exception ex) {
				Console.WriteLine(ex);
			}
		}
		
		void Run()
		{
			string url = "http://nuget.org/api/v2/";
			var repository = PackageRepositoryFactory.Default.CreateRepository(url);
			foreach (IPackage package in repository
				.Search(null, false)
				.Where (p => p.IsLatestVersion)
				.OrderByDescending(p => p.DownloadCount)
				//.Skip(100)
				.Take(100)) {
				
				foreach (IPackageFile file in package.GetContentFiles()) {
					string extension = Path.GetExtension(file.Path);
					if (".xdt" == extension.ToLower()) {
						Console.WriteLine("Package.Id: " + package.Id);
						break;
					}
				}
			}
		}
	}
}