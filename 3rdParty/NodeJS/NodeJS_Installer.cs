﻿using System;
using System.Diagnostics;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods; 
using O2.XRules.Database.Utils;

//O2File:Tool_API.cs
  
namespace O2.XRules.Database.APIs 
{ 
	public class Install_NodeJS_Test
	{
		public void test()
		{
			new NodeJS_Installer().start();
		}
	}
	public class NodeJS_Installer : Tool_API 
	{			
		public NodeJS_Installer()
		{
			Install_Uri = "http://nodejs.org/dist/v0.8.16/node-v0.8.16-x86.msi".uri();
			Install_File = "node-v0.8.16-x86.msi";
			Install_Dir = ProgramFilesFolder;			
			Executable = ProgramFilesFolder.pathCombine("NodeJS//node.exe");				   			
			startInstaller_FromMsi_Web();			
		}	
		public Process start()
		{
			if (isInstalled())
				return Executable.startProcess();
			return null;
		}				
	}	
}