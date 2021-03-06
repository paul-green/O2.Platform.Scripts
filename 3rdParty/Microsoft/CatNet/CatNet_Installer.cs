﻿using System;
using System.Diagnostics;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods; 
using O2.XRules.Database.Utils;
//O2File:Tool_API.cs

public class DynamicType
{
	public void dynamicMethod()
	{
		new O2.XRules.Database.APIs.CatNet_Installer().start();  
	}
} 
	
namespace O2.XRules.Database.APIs 
{
	
	public class CatNet_Installer : Tool_API  
	{	
		public CatNet_Installer()
		{
			config("CatNet_1.1", 				   
				   "http://download.microsoft.com/download/3/3/4/334E8A84-0F1B-4E3C-AF5F-99DA8AE0601F/CATNETx32.msi".uri(),
				   "SourceDir\\CATNetCmd.exe");
			install_JustMsiExtract_into_TargetDir();
		}
				
		
		public Process start()
		{
			if (isInstalled())
				return Executable.startProcess();
			return null;
		}		
	}
}