﻿using System;
using System.Reflection;
using System.Web.Mvc;

namespace BeachTime
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class MultipleButtonAttribute : ActionNameSelectorAttribute
	{
		public string Name { get; set; }
		public string Argument { get; set; }

		public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
		{
			bool isValidName = false;
			string keyValue = string.Format("{0}:{1}", Name, Argument);
			ValueProviderResult value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

			if (value != null)
			{
				controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;
				isValidName = true;
			}

			return isValidName;
		}
	}
}