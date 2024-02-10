using UnityEngine;

public static partial class DL
{

	public static void Break()
	{
		if (!isActive)
			return;

		Debug.Break();
	}

	public static void ClearDeveloperConsole()
	{
		if (!isActive)
			return;

		Debug.ClearDeveloperConsole();
	}

	public static void Log(object message)
	{
		if (!isActive)
			return;

		if (Settings.SaveToFileEnabled)
		{
			if (Settings.Line_StackTrace_SaveToFile)
			{
				// Sformatuj wpis do listy
				SendLogEntryFromDirectToFile("", message.ToString(), LoggerType.Log, StackTraceUtility.ExtractStackTrace());
			}
			else
			{
				// Sformatuj wpis do listy
				SendLogEntryFromDirectToFile("", message.ToString(), LoggerType.Log);
			}

		}

		if (!Debug.isDebugBuild)
			return;

		//todo Server

		if (!Settings.ShowInUnity)
			return;

		if (!Settings.Log_ShowInUnity)
			return;

		if (Settings.Line_StackTrace_ShowInUnity)
		{
			StackTraceLogType temp = Application.GetStackTraceLogType(LogType.Log);

			if (temp == StackTraceLogType.None)
				Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);

			Debug.Log(message.ToString().Colored(defaultColor));
			Application.SetStackTraceLogType(LogType.Log, temp);
		}
		else
		{
			StackTraceLogType temp = Application.GetStackTraceLogType(LogType.Log);

			if (temp == StackTraceLogType.ScriptOnly)
				Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);

			Debug.Log(message.ToString().Colored(defaultColor));
			Application.SetStackTraceLogType(LogType.Log, temp);
		}

	}
	public static void Log(string channel, object message)
	{
		if (!isActive)
			return;

		if (Settings.SaveToFileEnabled)
		{
			if (Settings.Log_StackTrace_SaveToFile)
			{
				// Sformatuj wpis do listy
				SendLogEntryFromDirectToFile(channel, message.ToString(), LoggerType.Log, StackTraceUtility.ExtractStackTrace());
			}
			else
			{
				// Sformatuj wpis do listy
				SendLogEntryFromDirectToFile(channel, message.ToString(), LoggerType.Log);
			}

		}

		if (!Debug.isDebugBuild)
			return;

		//todo Server

		if (!Settings.ShowInUnity)
			return;

		if (!Settings.Log_ShowInUnity)
			return;

		if (Settings.Log_StackTrace_ShowInUnity)
		{
			StackTraceLogType temp = Application.GetStackTraceLogType(LogType.Log);

			if (temp == StackTraceLogType.None)
				Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);

			Debug.Log(message.ToString().Colored(defaultColor));
			Application.SetStackTraceLogType(LogType.Log, temp);
		}
		else
		{
			StackTraceLogType temp = Application.GetStackTraceLogType(LogType.Log);

			if (temp == StackTraceLogType.ScriptOnly)
				Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);

			Debug.Log(message.ToString().Colored(defaultColor));
			Application.SetStackTraceLogType(LogType.Log, temp);
		}
	}

	public static void Log(string channel, object message, params object[] args)
	{
		if (!isActive)
			return;

		if (Settings.SaveToFileEnabled)
		{
			if (Settings.Log_StackTrace_SaveToFile)
			{
				// Sformatuj wpis do listy
				SendLogEntryFromDirectToFile(channel, string.Format(message.ToString(), args), LoggerType.Log, StackTraceUtility.ExtractStackTrace());
			}
			else
			{
				// Sformatuj wpis do listy
				SendLogEntryFromDirectToFile(channel, string.Format(message.ToString(), args), LoggerType.Log);
			}

		}

		if (!Debug.isDebugBuild)
			return;

		//todo Server

		if (!Settings.ShowInUnity)
			return;

		if (!Settings.Log_ShowInUnity)
			return;

		StackTraceLogType temp = Application.GetStackTraceLogType(LogType.Log);

		if (temp == StackTraceLogType.None)
			Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);

		Debug.Log($"{channel}: ".Bold() + string.Format(message.ToString().Colored(defaultColor), args));

		Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
	}















	public static void LogError(string channel = "", object message = null, params object[] args) { }
	public static void LogInfo(string channel = "", object message = null, params object[] args) { }
	public static void LogWarning(string channel = "", object message = null, params object[] args) { }
	public static void LogException(System.Exception exception) { }
	public static void LogStackTrace(bool isDiagnosticStackTrace = false) { }
	public static void AddSeparator() { }

}