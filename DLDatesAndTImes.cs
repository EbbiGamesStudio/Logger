using System;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;

public static partial class DL
{
	public struct DateTimeFormat
	{
		/// <summary>
		/// Format daty i czasu w formacie: 2020-01-01 12:00:00.000
		/// </summary>
		public const string Full = "yyyy-MM-dd HH.mm.ss.fff";
		/// <summary>
		/// Format daty i czasu w formacie: 2020-01-01 12:00:00
		/// </summary>
		public const string Long = "yyyy-MM-dd HH:mm:ss";
		/// <summary>
		/// Format daty i czasu w formacie: 2020-01-01
		/// </summary>
		public const string Short = "yyyy-MM-dd";
		/// <summary>
		/// Format daty i czasu w formacie: 12:00:00.000
		/// </summary>
		public const string Time = "HH:mm:ss.fff";
		/// <summary>
		/// Format daty i czasu w formacie: 12:00:00
		/// </summary>
		public const string TimeShort = "HH:mm:ss";
	}

	private static Timer timer;

	public const string Version = "v1.1";
	public const string FullNameApplication = "Debuger (DL) " + Version;
	private const string separator = "==================================================================================================================";
	private static List<string> logList;

	private static string GetCurrentTime() => DateTime.Now.ToString(DateTimeFormat.Time);
	private static string GetCurrentTimeForFilename() => DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss.fff");


	// Czy debuger jest aktywny, jest to możliwość całkowitego wyłączenia debugera.
	private static bool isActive = true;
	private static string filePath = "";

	public static DLSettings Settings;

	public const int LogEntryTimeLength = 12;
	public const int LogEntryTypeLength = 9;
	public const int LogEntryTitleLength = 30;
	public const int LogEntrySeparatorLength = 3;
	public const int LogEntryLeftMargin = LogEntryTimeLength + LogEntryTypeLength + LogEntryTitleLength + (LogEntrySeparatorLength * 2);

	private static int bufferIndex = 0;

	private static string lineColor = Colors.Gray;
	private static string argColor = Colors.Cyan;
	private static string defaultColor = Colors.White;
	private static string infoColor = Colors.Lime;
	private static string warningColor = Colors.Yellow;
	private static string errorColor = Colors.Red;

	/// <summary>
	/// Zmienna, która przechowuje informację o tym, czy debuger jest aktywny
	/// Kiedy debuger jest wyłączony, wszystkie metody debugowania są przerywane.
	/// </summary>
	public static bool Enabled
	{
		get => isActive;
		set => isActive = value;
	}

	/// <summary>
	/// Ustawia ilość plików logów
	/// </summary>
	public static void SetFilesCount(int count)
	{
		Settings.File_MaximumFilesCount = count;
	}

}