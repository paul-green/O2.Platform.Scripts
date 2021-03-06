﻿using System;
using System.Diagnostics;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods; 
//O2File:Tool_API.cs

using O2.XRules.Database.Utils;

namespace O2.XRules.Database.APIs
{
	public class Install_Snoop_Test
	{
		public void test()
		{
			new Snoop_Installer().start();
		}
	}
	public class Snoop_Installer : Tool_API 
	{	
		public Snoop_Installer() : this(true)
		{
		}
		
		public Snoop_Installer(bool installNow)
		{
			config("Snoop", "Snoop v2.7.1", "Snoop.zip");			
    		Install_Uri = "https://github.com/downloads/cplotts/snoopwpf/Snoop.zip".uri();
    		if (installNow)
    			install();		
		}
		
		
		public bool install()
		{
			"Installing {0}".info(ToolName);
			return installFromZip_Web(); 						
		}
		
		public Process start()
		{
			if (install())
				return Install_Dir.pathCombine("Snoop\\snoop.exe").startProcess();
			return null;
		}		
	}
}