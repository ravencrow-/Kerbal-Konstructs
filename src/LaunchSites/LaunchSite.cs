﻿using System;
using UnityEngine;

namespace KerbalKonstructs.LaunchSites
{
	public class LaunchSite
	{
		public string name;
		public string author;
		public SiteType type;
		public Texture logo;
		public Texture icon;
		public string description;

		// ASH 28102014 - Added category
		public string category;
		// ASH Added career strategy
		public float opencost;
		public float closevalue;

		public LaunchSite(string sName, string sAuthor, SiteType sType, Texture sLogo, Texture sIcon, string sDescription, string sDevice = "Other", float fOpenCost = 0, float fCloseValue = 0)
		{
			name = sName;
			author = sAuthor;
			type = sType;
			logo = sLogo;
			icon = sIcon;
			description = sDescription;
			// ASH 28102014 - Added category
			category = sDevice;
			// ASH Added career strategy
			opencost = fOpenCost;
			closevalue = fCloseValue;
		}
	}

	public enum SiteType
	{
		VAB,
		SPH,
		Any
	}
}
