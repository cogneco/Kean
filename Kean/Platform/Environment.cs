using System;
using Kean.Extension;
using Collection = Kean.Collection;
using Management = System.Management;

namespace Kean.Platform
{
	public class Environment :
		Collection.Dictionary<string, string>
	{

		Environment()
		{
			int cpuCount = 0;
			if (Kean.Environment.IsWindows)
			{
				Management.ManagementObjectSearcher searcher = new Management.ManagementObjectSearcher("SELECT * FROM Win32_Processor");
				foreach (Management.ManagementObject share in searcher.Get())
					this["Cpu" + cpuCount++] = share.Properties["Name"].Value + ", " + share.Properties["Caption"].Value;
			}
			this["OperatingSystem"] =
					System.Environment.OSVersion.VersionString +
					(System.Environment.Is64BitOperatingSystem ? " 64bit " : " ");
			this["MachineName"] = System.Environment.MachineName.ToLower();
			this["User"] = System.Environment.UserName;

			this["Executable"] = System.IO.Path.GetFileName(System.Environment.GetCommandLineArgs().First());
			this["RuntimeVersion"] = System.Environment.Version.ToString();
		}
		static Environment current;
		public static Environment Current 
		{
			get 
			{
				if (Environment.current.IsNull())
					Environment.current = new Environment();
				return Environment.current; } 
		}
	}
}
