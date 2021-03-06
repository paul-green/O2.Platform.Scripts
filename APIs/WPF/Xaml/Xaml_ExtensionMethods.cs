﻿using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.Integration;
using System.Windows;
using O2.DotNetWrappers.ExtensionMethods;
using System.Windows.Markup;
using System.Windows.Controls;
using System.IO;
using O2.XRules.Database.APIs;

//O2Ref:FluentSharp.WPF.dll
//O2File:XamlCode.cs

//O2Ref:GraphSharp.dll
//O2Ref:QuickGraph.dll
//O2Ref:GraphSharp.Controls.dll 

//O2Ref:WindowsFormsIntegration.dll
//O2Ref:PresentationFramework.dll
//O2Ref:PresentationCore.dll
//O2Ref:WindowsBase.dll
//O2Ref:System.Xaml.dll



namespace O2.XRules.Database.Utils
{
    public static class Xaml_ExtensionMethods
    {
        public static ElementHost add_XamlViewer(this System.Windows.Forms.Control control)
        {
            return control.add_WPF_Host();
        }

        public static UIElement showXaml(this ElementHost elementHost, string xamlCode)
        {
            return elementHost.invokeOnThread<UIElement>(
                () =>
                {
                    try
                    {
                        if (xamlCode.valid())
                        {
                            ParserContext parserContext = new ParserContext();
                            parserContext.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                            parserContext.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
                            UIElement childElement = null;
                            var xamlObject = XamlReader.Parse(xamlCode, parserContext);
                            if (xamlObject is UIElement)
                            {
                                var frame = new Frame();
                                frame.Content = (UIElement)xamlObject;
                                childElement = frame;
                                elementHost.Child = childElement;
                                "Xaml code loaded into ElementHost".info();
                                return (UIElement)xamlObject;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        "in showXaml: {0}".format(ex.Message).error();		// trying to display the InnerException was trowing an error
                        //ex.log();
                    }
                    return null;
                });
        }

        public static UIElement xaml_CreateUIElement<T>(this string xamlCode)
            where T : UIElement
        {
            return xamlCode.xaml_CreateUIElement(null);
        }

        public static UIElement xaml_CreateUIElement<T>(this string xamlCode, Dictionary<string, string> extraXmlnsValues)
            where T : UIElement
        {
            var newElement = xamlCode.xaml_CreateUIElement(extraXmlnsValues);
            if (newElement is T)
                return (T)newElement;
            return null;
        }

        public static UIElement xaml_CreateUIElement(this string xamlCode)
        {
            return xamlCode.xaml_CreateUIElement(null);
        }

        public static UIElement xaml_CreateUIElement(this string xamlCode, Dictionary<string, string> extraXmlnsValues)
        {
            try
            {
                if (xamlCode.valid())
                {
                    ParserContext parserContext = new ParserContext();
                    parserContext.XamlTypeMapper = new XamlTypeMapper(new string[] { "AvalonDock" });

                    parserContext.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                    parserContext.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
                    if (extraXmlnsValues.notNull())
                        foreach (var item in extraXmlnsValues)
                            parserContext.XmlnsDictionary.Add(item.Key, item.Value);
                    //parserContext.XmlnsDictionary.Add();
                    var xamlObject = XamlReader.Parse(xamlCode, parserContext);
                    if (xamlObject is UIElement)
                        return (UIElement)xamlObject;
                }
            }
            catch (Exception ex)
            {
                ex.log("in xaml_CreateUIElement", false);
            }
            return null;
        }

        public static T xaml_CreateUIElement<T>(this UIElement uiElement, string xamlCode)
            where T : UIElement
        {
            return uiElement.xaml_CreateUIElement<T>(xamlCode, null);
        }

        public static T xaml_CreateUIElement<T>(this UIElement uiElement, string xamlCode, Dictionary<string, string> extraXmlnsValues)
            where T : UIElement
        {
            var newElement = uiElement.xaml_CreateUIElement(xamlCode, extraXmlnsValues);
            if (newElement is T)
                return (T)newElement;
            return null;
        }

        public static UIElement xaml_CreateUIElement(this UIElement uiElement, string xamlCode)
        {
            return uiElement.xaml_CreateUIElement(xamlCode, null);
        }

        public static UIElement xaml_CreateUIElement(this UIElement uiElement, string xamlCode, Dictionary<string, string> extraXmlnsValues)
        {
            return (UIElement)uiElement.wpfInvoke(
                () =>
                {
                    var newUIElement = xamlCode.xaml_CreateUIElement(extraXmlnsValues);
                    uiElement.add_Control_Wpf(newUIElement);
                    return newUIElement;
                });
        }

        public static string xaml(this UIElement uiElement)
        {
            return uiElement.xaml_Code(true);
        }

        public static string xaml_Code(this UIElement uiElement)
        {
            return uiElement.xaml_Code(true);
        }

        public static string xaml_Code(this UIElement uiElement, bool indent)
        {
            return (string)uiElement.wpfInvoke(
                () =>
                {
                    var stringWriter = new StringWriter();
                    var xmlTextWriter = new XmlTextWriter(stringWriter);
                    xmlTextWriter.Formatting = (indent) ? Formatting.Indented : Formatting.None;



                    //xmlTextWriter.Settings.prop("OutputMethod",XmlOutputMethod.Xml);


                    XamlWriter.Save(uiElement, xmlTextWriter);
                    return stringWriter.str();
                });
        }
        
        public static Button add_Xaml_Button(this UIElement uiElement, string buttonText)
    	{
    		return uiElement.xaml_CreateUIElement<Button>(XamlCode.new_Button(buttonText));	   
    	}
    	public static Button add_Xaml_Button(this UIElement uiElement, string buttonText,  Action onClickCallback)
    	{
    		return uiElement.add_Xaml_Button( buttonText, -1,-1 ,onClickCallback);
    	}
    	
    	public static Button add_Xaml_Button(this UIElement uiElement, string buttonText, int left, int top,  Action onClickCallback)
    	{
    		var button = uiElement.add_Xaml_Button(buttonText);
    		button.onClick_Wpf(onClickCallback);
    		if (left > -1)
    			button.left_Wpf(left);
    		if (top > -1)
    			button.top_Wpf(top);
			  
    		return button;
    	}
    	    	    	
    	public static Button add_Xaml_Link(this UIElement uiElement, string linkText, Action onClickCallback)
    	{
    		return uiElement.add_Xaml_Link(linkText, "5",onClickCallback);
    	}
    	
    	public static Button add_Xaml_Link(this UIElement uiElement, string linkText, string margin, Action onClickCallback)
    	{
    		return (Button)uiElement.wpfInvoke(
    			()=>{
    					var link = uiElement.xaml_CreateUIElement<Button>(XamlCode.link(linkText,margin));	 
    					link.Focusable = false;
    					link.onClick_Wpf(onClickCallback);
    					return link;
    				});
    	}    
    	
    	//public static TextBlock add_TextBlock(this  
    	public static TextBlock add_Xaml_Title(this UIElement uiElement, string text)
    	{
	    	return (TextBlock)uiElement.wpfInvoke(
	    			()=>{
	    					var textXaml = XamlCode.textBlock(text, "Center", "Segoe, Segoe UI, Arial", "Bold", "14pt", "Black");
	    					return uiElement.xaml_CreateUIElement<TextBlock>(textXaml);
	    				});
    	}    		
    }
}
