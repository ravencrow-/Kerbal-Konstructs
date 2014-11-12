﻿using KerbalKonstructs.LaunchSites;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KerbalKonstructs.UI
{
	public class LaunchSiteSelectorGUI
	{
		LaunchSite selectedSite;
		private SiteType editorType = SiteType.Any;

		private Boolean isCareer = false;
		private Boolean isOpen = false;

		// ASH 28102014 - Needs to be bigger for filter
		Rect windowRect = new Rect(((Screen.width - Camera.main.rect.x) / 2) + Camera.main.rect.x - 125, (Screen.height / 2 - 250), 700, 580);

		public void drawSelector()
		{
			if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
			{
				// disableCareerStrategyLayer is configurable in KerbalKonstructs.cfg
				if (!KerbalKonstructs.instance.disableCareerStrategyLayer)
				{
					// ASH 11112014 Career strategy layer 
					// DISABLE career strategy layer by simply commenting out the next line
					isCareer = true;
				}
			}
			if (Camera.main != null)//Camera.main is null when first loading a scene
			{
				GUI.Window(0xB00B1E6, windowRect, drawSelectorWindow, "Launch Site Selector");
			}

			if (windowRect.Contains(Event.current.mousePosition))
			{
				InputLockManager.SetControlLock(ControlTypes.EDITOR_LOCK, "KKEditorLock");
			}
			else
			{
				InputLockManager.RemoveControlLock("KKEditorLock");
			}
		}

		public Vector2 sitesScrollPosition;
		public Vector2 descriptionScrollPosition;

		// ASH 28102014 Changed scope so we can change it by Category filter
		public List<LaunchSite> sites;

		public void drawSelectorWindow(int id)
		{
			// ASH 28102014 Category filter handling added.
			// ASH 07112014 Disabling of restricted categories added.
			GUILayout.BeginArea(new Rect(10, 25, 370, 550));
			GUILayout.BeginHorizontal();
			if (editorType == SiteType.SPH)
			{
				GUI.enabled = false;
			}
			if (GUILayout.Button("RocketPads", GUILayout.Width(80)))
			{
				sites = (editorType == SiteType.Any) ? LaunchSiteManager.getLaunchSites() : LaunchSiteManager.getLaunchSites(editorType, true, "RocketPad");
			}
			GUI.enabled = true;
			GUILayout.Space(2);
			if (editorType == SiteType.VAB)
			{
				GUI.enabled = false;
			}
			if (GUILayout.Button("Runways", GUILayout.Width(73)))
			{
				sites = (editorType == SiteType.Any) ? LaunchSiteManager.getLaunchSites() : LaunchSiteManager.getLaunchSites(editorType, true, "Runway");
			}
			GUI.enabled = true;
			GUILayout.Space(2);
			if (editorType == SiteType.VAB)
			{
				GUI.enabled = false;
			}
			if (GUILayout.Button("Helipads", GUILayout.Width(73)))
			{
				sites = (editorType == SiteType.Any) ? LaunchSiteManager.getLaunchSites() : LaunchSiteManager.getLaunchSites(editorType, true, "Helipad");
			}
			GUI.enabled = true;
			GUILayout.Space(2);
			if (GUILayout.Button("Other", GUILayout.Width(65)))
			{
				sites = (editorType == SiteType.Any) ? LaunchSiteManager.getLaunchSites() : LaunchSiteManager.getLaunchSites(editorType, true, "Other");
			}
			GUILayout.Space(2);
			if (GUILayout.Button("ALL", GUILayout.Width(45)))
			{
				sites = (editorType == SiteType.Any) ? LaunchSiteManager.getLaunchSites() : LaunchSiteManager.getLaunchSites(editorType, true, "ALL");
			}
			GUILayout.EndHorizontal();

			GUILayout.Space(10);

			sitesScrollPosition = GUILayout.BeginScrollView(sitesScrollPosition);
			// ASH 28102014 Category filter handling added
			//List<LaunchSite> sites = (editorType == SiteType.Any) ? LaunchSiteManager.getLaunchSites() : LaunchSiteManager.getLaunchSites(editorType);

			if (sites == null) sites = (editorType == SiteType.Any) ? LaunchSiteManager.getLaunchSites() : LaunchSiteManager.getLaunchSites(editorType, true, "ALL");

			foreach (LaunchSite site in sites)
				{
					GUI.enabled = !(selectedSite == site);
					if (GUILayout.Button(site.name, GUILayout.Height(30)))
					{
						selectedSite = site;
						// ASH Career Mode Unlocking
						if (!isCareer)
							LaunchSiteManager.setLaunchSite(site);
					}
				}
				GUILayout.EndScrollView();
			GUILayout.EndArea();

			//Fixes when the last item is selected and it leaves the GUI disabled
			GUI.enabled = true;

			if (selectedSite != null)
			{
				drawRightSelectorWindow();
			}
			else
			{
				if (LaunchSiteManager.getLaunchSites().Count > 0)
				{
					selectedSite = LaunchSiteManager.getLaunchSites(editorType)[0];
					// ASH Career Mode Unlocking
					if (!isCareer)
						LaunchSiteManager.setLaunchSite(selectedSite);

					// ASH 05112014 Might fix the selector centering issue on the right panel
					drawRightSelectorWindow();
				}
				else
				{
					Debug.Log("KK: ERROR Launch Selector cannot find KSC Runway or Launch Pad! PANIC! Runaway! Hide!");
				}
			}
		}

		// ASH 05112014 Might fix the selector centering issue on the right panel
		private void drawRightSelectorWindow()
		{
			GUILayout.BeginArea(new Rect(385, 25, 310, 550));
				GUILayout.Label(selectedSite.logo, GUILayout.Height(280));
				GUILayout.Label(selectedSite.name + " By " + selectedSite.author);
				descriptionScrollPosition = GUILayout.BeginScrollView(descriptionScrollPosition);
				GUILayout.Label(selectedSite.description);
				GUILayout.EndScrollView();

				float iFundsOpen = 0;
				float iFundsClose = 0;
				iFundsOpen = selectedSite.opencost;
				iFundsClose = selectedSite.closevalue;

				bool isAlwaysOpen = false;
				bool cannotBeClosed = false;

				// If it is 0 to open it is always open
				if (iFundsOpen == 0)
					isAlwaysOpen = true;

				if (iFundsClose == 0)
					cannotBeClosed = true;
				// If it is 0 to close you cannot close it
				
				if (isCareer)
				{	
					// Determine funds to open and close from instance cfg

					// Determine if a site is open or closed
					// If persistence says the site is open then isOpen = true;
					// If persistence file says nothing or site is closed then isOpen = false;

					// Testing
					GUI.enabled = !isAlwaysOpen;
					GUI.enabled = !isOpen;
					if (!isAlwaysOpen)
					{
						if (GUILayout.Button("Open Site for " + iFundsOpen + " Funds"))
						{
							// What if there isn't enough funds?

							// Open the site - save to persistence

							// Charge some funds
							Funding.Instance.AddFunds(-iFundsOpen, TransactionReasons.Cheating);
						}
					}
					GUI.enabled = true;
					
					// Testing
					GUI.enabled = isOpen;
					GUI.enabled = !cannotBeClosed;
					if (!cannotBeClosed)
					{
						if (GUILayout.Button("Close Site for " + iFundsClose + " Funds"))
						{
							// Close the site - save to persistence
							// Pay back some funds

							Funding.Instance.AddFunds(iFundsClose, TransactionReasons.Cheating);
						}
					}
					GUI.enabled = true;

					// Testing
					GUI.enabled = isOpen;
					GUI.enabled = isAlwaysOpen;
					GUI.enabled = !(selectedSite.name == EditorLogic.fetch.launchSiteName);
					
					if (GUILayout.Button("Set as Launchsite"))
					{
						LaunchSiteManager.setLaunchSite(selectedSite);
					}
					GUI.enabled = true;
				}
			GUILayout.EndArea();
		}

		public void setEditorType(SiteType type)
		{
			editorType = (KerbalKonstructs.instance.launchFromAnySite) ? SiteType.Any : type;
			if (selectedSite != null)
			{
				if (selectedSite.type != editorType && selectedSite.type != SiteType.Any)
				{
					selectedSite = LaunchSiteManager.getLaunchSites(editorType)[0];
				}
				// ASH Career Mode Unlocking
				if (!isCareer)
					LaunchSiteManager.setLaunchSite(selectedSite);
			}
		}
		
		// ASH and Ravencrow 28102014
		// Need to handle if Launch Selector is still open when switching from VAB to from SPH
		// otherwise abuse possible!
		public void Close()
		{
			sites = null;
		}
	}
}
