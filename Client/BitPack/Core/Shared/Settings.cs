﻿using BitPack.Core.Specific;
using BitPack.MVVM.ViewModel;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace BitPack.Core.Shared
{
	public class Settings
	{
		public string Username;
		public string Channel;
		public bool AutoCheckUpdates;

		public string Theme;
		public string SystemTheme; //Not part of saved settings file
		public string Accent;

		public Settings() { }
		public Settings(string username, string channel, bool autoCheckUpdates, string theme, string systemTheme, string accent)
		{
			Username = username;
			Channel = channel;
			AutoCheckUpdates = autoCheckUpdates;

			Theme = theme;
			SystemTheme = systemTheme;
			Accent = accent;
		}
		public void SetDefault()
		{
			Username = "";
			Channel = "stable";
			AutoCheckUpdates = true;

			Theme = "Auto";
			Accent = "Blue";
		}
	}

	public static class SettingsHelper
	{
		public static Settings Data { get; set; }
		public static Settings Default { get; set; }

		public static void Init()
		{
			Data = new Settings();
			Default = new Settings();
			Default.SetDefault();
		}

		public static void CallUpdate()
		{
			MainViewModel MainVM = (MainViewModel)ViewModelManager.ViewModels["Main"];
			MainVM.Update();
			SaveSettings();
		}

		public static void InitSettings()
		{
			Data.SetDefault();
			SaveSettings();
			LoadSettings();
		}

		public static void LoadSettings()
		{
			if (!File.Exists(Constants.SettingsPath) || File.ReadAllText(Constants.SettingsPath) == "")
			{
				InitSettings();
				return;
			}

			string rawJSON = File.ReadAllText(Constants.SettingsPath);
			JObject jo = JObject.Parse(rawJSON);

			bool resave = false;

			Data.SetDefault();

			if (jo["username"] == null) resave = true;
			else Data.Username = (string)jo["username"];

			if (jo["channel"] == null) resave = true;
			else Data.Channel = (string)jo["channel"];

			if (jo["autoCheckUpdates"] == null) resave = true;
			else Data.AutoCheckUpdates = (bool)jo["autoCheckUpdates"];



			if (jo["theme"] == null) resave = true;
			else Data.Theme = (string)jo["theme"];

			if (jo["accent"] == null) resave = true;
			else Data.Accent = (string)jo["accent"];



			if (resave) SaveSettings();
			ApplyVisualSettings();
		}

		public static void SaveSettings()
		{
			JObject jo = new();

			jo.Add("username", Data.Username);
			jo.Add("channel", Data.Channel);
			jo.Add("autoCheckUpdates", Data.AutoCheckUpdates);

			jo.Add("theme", Data.Theme);
			jo.Add("accent", Data.Accent);

			if (!File.Exists(Constants.SettingsPath))
			{
				int sep = Constants.SettingsPath.LastIndexOf("/");

				Directory.CreateDirectory(Constants.SettingsPath.Substring(0, sep));
				File.CreateText(Constants.SettingsPath).Close();
			}

			File.WriteAllText(Constants.SettingsPath, jo.ToString());
		}

		public static void ApplyVisualSettings()
		{
			if (!Constants.ThemeURIs.Keys.Contains(Data.Theme)) Data.Theme = "Auto";
			if (!Constants.AccentURIs.Keys.Contains(Data.Accent)) Data.Accent = "Blue";

			string accentString = Data.Accent;
			string themeString = Data.Theme;
			if (themeString == "Auto") themeString = Data.SystemTheme;

			Application.Current.Resources.MergedDictionaries[1].Source = new Uri(Constants.ThemeURIs[themeString], UriKind.Relative);
			Application.Current.Resources.MergedDictionaries[2].Source = new Uri(Constants.AccentURIs[accentString], UriKind.Relative);
		}
	}

	public class ThemeWatcher
	{
		private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
		private const string RegistryValueName = "AppsUseLightTheme";

		private enum WindowsTheme
		{
			Light,
			Dark
		}

		private static WindowsTheme GetWindowsTheme()
		{
			using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
			{
				object registryValueObject = key?.GetValue(RegistryValueName);
				if (registryValueObject == null)
				{
					return WindowsTheme.Light;
				}

				int registryValue = (int)registryValueObject;

				return registryValue > 0 ? WindowsTheme.Light : WindowsTheme.Dark;
			}
		}

		public ThemeWatcher()
		{
			SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;

			string theme = GetWindowsTheme().ToString();
			SettingsHelper.Data.SystemTheme = theme;
		}

		public void Destroy()
		{
			SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;
		}

		private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			if (e.Category != UserPreferenceCategory.General) return;
			string theme = GetWindowsTheme().ToString();

			SettingsHelper.Data.SystemTheme = theme;
			SettingsHelper.ApplyVisualSettings();
		}
	}
}
