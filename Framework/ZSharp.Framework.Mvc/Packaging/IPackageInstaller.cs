﻿using System;
using System.IO;
using NuGet;

namespace ZSharp.Framework.Mvc.Packaging
{
	public interface IPackageInstaller
	{
		//PackageInfo Install(string packageId, string version, string location, string applicationFolder);
		PackageInfo Install(Stream packageStream, string location, string applicationPath);
		void Uninstall(string packageId, string applicationFolder);
	}
}
