﻿using System;
using System.Linq;
using System.Text;
using System.Drawing;  
using System.Windows.Forms;
using System.Drawing.Imaging; 
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using O2.DotNetWrappers.ExtensionMethods;


//O2File:WindowFinder.cs

//O2File:_Extra_methods_To_Add_to_Main_CodeBase.cs

namespace O2.XRules.Database.APIs
{
	public class API_Win32_Handle_Hijack_test
	{
		public void test()
		{
			"API Win32 Handle Hijack test".popupWindow().add_Handle_HijackGui();
		}		
	} 
	
	public class API_Win32_Handle_Hijack
	{
		
	}
	
	public class Win32_Handle_Hijack : Control
	{
		public Panel 			topPanel;
		public WindowFinder 	windowFinder;
		public TextBox 			targetHandle;	
		public TextBox 			parentHandle;
		public Panel 			hijackedWindow;		
		public PictureBox 		pictureBox;
		public Label 			className;
		public IntPtr			hijackedHandle;
		public IntPtr			hijackedParent;
		public TextBox 			test;
			
		public Win32_Handle_Hijack()
		{
			this.width(400);
			this.height(400);
			buildGui();
		}
		
		public Win32_Handle_Hijack buildGui()
		{									
			Action restore = 
				()=>{												
						if (hijackedHandle != IntPtr.Zero)
						{
							"restoring {0} to parent {1}".info(hijackedHandle, hijackedParent);
							hijackedHandle.setParent(hijackedParent);
							hijackedParent.window_Redraw();
							hijackedHandle.window_Redraw();
						}
					};
					
			Action hijack = 
				()=>{ 
						restore();
						var handle = targetHandle.get_Text().toInt().intPtr();						
						var newParent = hijackedWindow.clear().handle();
						"Hijacking {0} into window {1}".info(handle, newParent);
						hijackedHandle = handle;
						hijackedParent = parentHandle.get_Text().toInt().intPtr();
						handle.setParent(newParent);						
					};					
			Action screenShot = 
				()=>{	
						restore();
						try
						{
							var handle = targetHandle.get_Text().toInt().intPtr();						
							var bitmap = handle.window_ScreenShot();
							hijackedWindow.clear().add_PictureBox().layout_Zoom().show(bitmap);
						}
						catch(Exception ex)
						{
							ex.log();
						}
					};
			Action<IntPtr> setTarget = 
				(handle)=>{
							 	targetHandle.set_Text(handle.str());
							 	className.set_Text(handle.className());
							 	pictureBox.show(handle.window_ScreenShot());
								//pictureBox
							};
			topPanel = this.add_Panel();				
			topPanel.insert_Above(35).splitContainerFixed()
						.add_WindowFinder(ref windowFinder) 
						.append_Label("Handle:").top(10).append_TextBox(ref targetHandle)
						.append_Label("Parent:").top(10).append_TextBox(ref parentHandle)
						.append_Link("Hijack", ()=> hijack()).top(10)
						.append_Link("Restore", ()=> restore()) 
						.append_Link("Screenshot", ()=> screenShot()) 
						.append_PictureBox(ref pictureBox)
						.append_TextBox(ref test).set_Text("Hijack me").top(10) 
						.append_Label(ref className).topAdd(2);

				
						
			hijackedWindow = topPanel.add_GroupBox("Hijacked Window/Control").add_Panel();		
					
			targetHandle.onTextChange((text)=> parentHandle.set_Text(text.toInt().intPtr().parent().str())); 
			windowFinder.Window_Changed = setTarget; 
				
//			setTarget(test.handle()); 
			
			pictureBox.layout_Zoom();					  
			this.onClosed(
				()=>{
						"On Closed".info();
						restore();
					});
			var groupBox = hijackedWindow.parent();;
			var originalText = groupBox.get_Text();
			var splitcontainer = groupBox.splitContainer();
			
			groupBox.DoubleClick+=(sender,e)=>
				{		
					var collapsed = splitcontainer.Panel1Collapsed;
					if (collapsed)
					{
						splitcontainer.panel1Collapsed(false);					
						groupBox.set_Text(originalText);		
					}
					else
					{
						splitcontainer.panel1Collapsed(true);		
						groupBox.set_Text(".");			
					}
				};	
			return this;	
		}
	}
	
	public static class API_Win32_Handle_Hijack_ExtensionMethods
	{	
					
		public static Win32_Handle_Hijack add_Handle_HijackGui(this Control hostPanel)
		{											
			return hostPanel.add_Control<Win32_Handle_Hijack>();;
		}
	}
}