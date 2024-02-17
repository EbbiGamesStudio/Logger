using System.Linq;

using UnityEngine;

public static partial class DL
{
	/// <summary>
	/// Wywołuje Unity.Break()
	/// </summary>
	public static void Break()
	{
		if (!isActive)
			return;

		Debug.Break();
	}

	/// <summary>
	/// Czyści konsolę deweloperską
	/// </summary>
	public static void ClearDeveloperConsole()
	{
		if (!isActive)
			return;

		Debug.ClearDeveloperConsole();
	}

	/// <summary>
	/// Wyświetla w konsoli deweloperskiej linie tekstu
	/// ze względu że ogólne debugowanie jest czasochłonne, 
	/// ta metoda musi być szybka oraz łatwo pomijalna
	/// Najczęsciej będzie używana do loopów i innych szybkich operacji
	/// </summary>
	public static void Line(object message)
	{
		if (!isActive) return;

		if (Settings.SaveToFileEnabled) SendLogEntryFromDirectToFile("", message.ToString(), LoggerType.Line);

		if (!Debug.isDebugBuild) return;

		if (!Settings.ShowInUnity) return;
		if (!Settings.Line_ShowInUnity) return;

		string result = message.ToString();
		result = result.Colored(lineColor);

		Debug.Log(result);
	}

	/// <summary>
	/// Wyświetla w konsoli deweloperskiej linie tekstu
	/// ze względu że ogólne debugowanie jest czasochłonne, 
	/// ta metoda musi być szybka oraz łatwo pomijalna
	/// Najczęsciej będzie używana do loopów i innych szybkich operacji
	/// </summary>
	public static void Line(string channel, object message)
	{
		if (!isActive) return;

		if (Settings.SaveToFileEnabled) SendLogEntryFromDirectToFile("", message.ToString(), LoggerType.Line);

		if (!Debug.isDebugBuild) return;

		if (!Settings.ShowInUnity) return;
		if (!Settings.Line_ShowInUnity) return;

		string result = $"{channel}: ";
		result += message.ToString();
		result = result.Colored(lineColor);

		Debug.Log(result);
	}

	/// <summary>
	/// Wyświetla w konsoli deweloperskiej linie tekstu
	/// </summary>
	public static void Line(string channel, object message, params object[] args)
	{
		if (!isActive) return;

		if (Settings.SaveToFileEnabled)
		{
			// Wyślij wpis do pliku ze stackTrace
			SendLogEntryFromDirectToFile(channel, string.Format(message.ToString(), args), LoggerType.Line);
		}

		if (!Debug.isDebugBuild) return;

		if (!Settings.ShowInUnity) return;
		if (!Settings.Log_ShowInUnity) return;

		string[] resultArgs = args.Select(arg => arg.ToString()).ToArray();
		resultArgs = resultArgs.Italics();

		string result = $"{channel}: ";
		result += string.Format(message.ToString(), resultArgs);
		result = result.Colored(lineColor);

		Debug.Log(result);
	}


	/// <summary>
	/// Wyświetla w konsoli deweloperskiej linie tekstu
	/// </summary>
	public static void Log(object message)
	{
		if (!isActive) return;

		if (Settings.SaveToFileEnabled)
		{
			if (Settings.Log_StackTrace_SaveToFile)
			{
				// Wyślij wpis do pliku ze stackTrace
				SendLogEntryFromDirectToFile("", message.ToString(), LoggerType.Line, StackTraceUtility.ExtractStackTrace());
			}
			else
			{
				// Wyślij wpis do pliku bez stackTrace
				SendLogEntryFromDirectToFile("", message.ToString(), LoggerType.Line);
			}
		}

		if (!Debug.isDebugBuild) return;

		if (!Settings.ShowInUnity) return;
		if (!Settings.Log_ShowInUnity) return;

		string result = message.ToString();
		Debug.Log(result.Colored(defaultColor));
	}

	/// <summary>
	/// Wyświetla w konsoli deweloperskiej linie tekstu
	/// </summary>
	public static void Log(string channel, object message)
	{
		if (!isActive) return;

		if (Settings.SaveToFileEnabled)
		{
			if (Settings.Log_StackTrace_SaveToFile)
			{
				// Wyślij wpis do pliku ze stackTrace
				SendLogEntryFromDirectToFile(channel, message.ToString(), LoggerType.Log, StackTraceUtility.ExtractStackTrace());
			}
			else
			{
				// Wyślij wpis do pliku bez stackTrace
				SendLogEntryFromDirectToFile(channel, message.ToString(), LoggerType.Log);
			}

		}

		if (!Debug.isDebugBuild) return;

		if (!Settings.ShowInUnity) return;
		if (!Settings.Log_ShowInUnity) return;

		string result = $"{channel}: ".Bold();
		result += message.ToString();
		result = result.Colored(defaultColor);

		Debug.Log(result);
	}

	/// <summary>
	/// Wyświetla w konsoli deweloperskiej linie tekstu
	/// </summary>
	public static void Log(string channel, object message, params object[] args)
	{
		if (!isActive) return;

		if (Settings.SaveToFileEnabled)
		{
			if (Settings.Log_StackTrace_SaveToFile)
			{
				// Wyślij wpis do pliku ze stackTrace
				SendLogEntryFromDirectToFile(channel, string.Format(message.ToString(), args), LoggerType.Log, StackTraceUtility.ExtractStackTrace());
			}
			else
			{
				// Wyślij wpis do pliku bez stackTrace
				SendLogEntryFromDirectToFile(channel, string.Format(message.ToString(), args), LoggerType.Log);
			}

		}

		if (!Debug.isDebugBuild) return;

		if (!Settings.ShowInUnity) return;
		if (!Settings.Log_ShowInUnity) return;

		string[] resultArgs = args.Select(arg => arg.ToString()).ToArray();
		resultArgs = resultArgs.Italics();
		resultArgs = resultArgs.Colored(argColor);

		string result = $"{channel}: ".Bold();
		result += string.Format(message.ToString(), resultArgs);
		result = result.Colored(defaultColor);

		Debug.Log(result);
	}

	/// <summary>
	/// Wyświetla w konsoli deweloperskiej linie tekstu
	/// </summary>
	public static void LogInfo(object message)
	{
		if (!isActive) return;

		if (Settings.SaveToFileEnabled)
		{
			// Wyślij wpis do pliku ze stackTrace
			SendLogEntryFromDirectToFile("", message.ToString(), LoggerType.Line, StackTraceUtility.ExtractStackTrace());
		}

		if (!Debug.isDebugBuild) return;

		if (!Settings.ShowInUnity) return;

		string result = message.ToString();
		result = result.Colored(infoColor);

		Debug.Log(result);
	}

	/// <summary>
	/// Wyświetla w konsoli deweloperskiej linie tekstu
	/// </summary>
	public static void LogInfo(string channel, object message)
	{
		if (!isActive) return;

		if (Settings.SaveToFileEnabled)
		{
			// Wyślij wpis do pliku ze stackTrace
			SendLogEntryFromDirectToFile(channel, message.ToString(), LoggerType.Info, StackTraceUtility.ExtractStackTrace());
		}

		if (!Debug.isDebugBuild) return;

		if (!Settings.ShowInUnity) return;

		string result = $"{channel}: ".Bold();
		result = result.Colored(infoColor);
		result += message.ToString().Colored(defaultColor);

		Debug.Log(result);
	}

	/// <summary>
	/// Wyświetla w konsoli deweloperskiej linie tekstu
	/// </summary>
	public static void LogInfo(string channel, object message, params object[] args)
	{
		if (!isActive) return;

		if (Settings.SaveToFileEnabled)
		{
			// Wyślij wpis do pliku ze stackTrace
			SendLogEntryFromDirectToFile(channel, string.Format(message.ToString(), args), LoggerType.Info, StackTraceUtility.ExtractStackTrace());
		}

		if (!Debug.isDebugBuild) return;

		if (!Settings.ShowInUnity) return;

		string[] resultArgs = args.Select(arg => arg.ToString()).ToArray();
		resultArgs = resultArgs.Italics();
		resultArgs = resultArgs.Colored(argColor);

		string result = $"{channel}: ".Bold();
		result = result.Colored(infoColor);
		result += string.Format(message.ToString(), resultArgs);
		result = result.Colored(defaultColor);

		Debug.Log(result);
	}

	/// <summary>
	/// Wyświetla w konsoli deweloperskiej linie tekstu
	/// </summary>
	public static void LogWarning(object message)
	{
		if (!isActive) return;

		if (Settings.SaveToFileEnabled)
		{
			// Wyślij wpis do pliku ze stackTrace
			SendLogEntryFromDirectToFile("", message.ToString(), LoggerType.Line, StackTraceUtility.ExtractStackTrace());
		}

		if (!Debug.isDebugBuild) return;

		if (!Settings.ShowInUnity) return;

		string result = message.ToString();
		result = result.Colored(warningColor);

		Debug.LogWarning(result);
	}

	/// <summary>
	/// Wyświetla w konsoli deweloperskiej linie tekstu
	/// </summary>
	public static void LogWarning(string channel, object message)
	{
		if (!isActive) return;

		if (Settings.SaveToFileEnabled)
		{
			// Wyślij wpis do pliku ze stackTrace
			SendLogEntryFromDirectToFile(channel, message.ToString(), LoggerType.Warning, StackTraceUtility.ExtractStackTrace());
		}

		if (!Debug.isDebugBuild) return;

		if (!Settings.ShowInUnity) return;

		string result = $"{channel}: ".Bold();
		result = result.Colored(warningColor);
		result += message.ToString().Colored(defaultColor);

		Debug.LogWarning(result);
	}

	/// <summary>
	/// Wyświetla w konsoli deweloperskiej linie tekstu
	/// </summary>
	public static void LogWarning(string channel, object message, params object[] args)
	{
		if (!isActive) return;

		if (Settings.SaveToFileEnabled)
		{
			// Wyślij wpis do pliku ze stackTrace
			SendLogEntryFromDirectToFile(channel, string.Format(message.ToString(), args), LoggerType.Warning, StackTraceUtility.ExtractStackTrace());
		}

		if (!Debug.isDebugBuild) return;

		if (!Settings.ShowInUnity) return;

		string[] resultArgs = args.Select(arg => arg.ToString()).ToArray();
		resultArgs = resultArgs.Italics();
		resultArgs = resultArgs.Colored(argColor);

		string result = $"{channel}: ".Bold();
		result = result.Colored(warningColor);
		result += string.Format(message.ToString(), resultArgs);
		result = result.Colored(defaultColor);

		Debug.LogWarning(result);
	}

	/// <summary>
	/// Wyświetla w konsoli deweloperskiej linie tekstu
	/// </summary>
	public static void LogError(object message)
	{
		if (!isActive) return;

		if (Settings.SaveToFileEnabled)
		{
			// Wyślij wpis do pliku ze stackTrace
			SendLogEntryFromDirectToFile("", message.ToString(), LoggerType.Line, StackTraceUtility.ExtractStackTrace());
		}

		if (!Debug.isDebugBuild) return;

		if (!Settings.ShowInUnity) return;

		string result = message.ToString();
		result = result.Colored(errorColor);

		Debug.LogError(result);
	}

	/// <summary>
	/// Wyświetla w konsoli deweloperskiej linie tekstu
	/// </summary>
	public static void LogError(string channel, object message)
	{
		if (!isActive) return;

		if (Settings.SaveToFileEnabled)
		{
			// Wyślij wpis do pliku ze stackTrace
			SendLogEntryFromDirectToFile(channel, message.ToString(), LoggerType.Error, StackTraceUtility.ExtractStackTrace());
		}

		if (!Debug.isDebugBuild) return;

		if (!Settings.ShowInUnity) return;

		string result = $"{channel}: ".Bold();
		result = result.Colored(errorColor);
		result += message.ToString().Colored(defaultColor);

		Debug.LogError(result);
	}

	/// <summary>
	/// Wyświetla w konsoli deweloperskiej linie tekstu
	/// </summary>
	public static void LogError(string channel, object message, params object[] args)
	{
		if (!isActive) return;

		if (Settings.SaveToFileEnabled)
		{
			// Wyślij wpis do pliku ze stackTrace
			SendLogEntryFromDirectToFile(channel, string.Format(message.ToString(), args), LoggerType.Error, StackTraceUtility.ExtractStackTrace());
		}

		if (!Debug.isDebugBuild) return;

		if (!Settings.ShowInUnity) return;

		string[] resultArgs = args.Select(arg => arg.ToString()).ToArray();
		resultArgs = resultArgs.Italics();
		resultArgs = resultArgs.Colored(argColor);

		string result = $"{channel}: ".Bold();
		result = result.Colored(errorColor);
		result += string.Format(message.ToString(), resultArgs);
		result = result.Colored(defaultColor);

		Debug.LogError(result);
	}

	/// <summary>
	/// Wyświetla w konsoli deweloperskiej wyjątek
	/// </summary>
	public static void LogException(System.Exception exception)
	{
		if (!isActive) return;

		if (!Debug.isDebugBuild) return;

		if (!Settings.ShowInUnity) return;

		Debug.LogException(exception);
	}

	public static void AddSeparator()
	{
		if (!isActive) return;

		if (Settings.SaveToFileEnabled)
		{
			// Wyślij wpis do pliku ze stackTrace
			SendLogEntryFromDirectToFile("", "", LoggerType.Separator, "==================================================================================================================");
		}

		if (!Debug.isDebugBuild) return;

		if (!Settings.ShowInUnity) return;

		Debug.Log("==================================================================================================================".Colored(lineColor));
	}

	public static string Margin => Settings.MarginForStackTraceInFile > 0 ? new string(' ', Settings.MarginForStackTraceInFile) : "";

	public static void LogStackTrace()
	{
		if (!isActive) return;
		if (!Debug.isDebugBuild) return;
		string stackTrace = StackTraceUtility.ExtractStackTrace();
		if (Settings.SaveToFileEnabled)
		{
			// Wyślij wpis do pliku ze stackTrace
			SendLogEntryFromDirectToFile("", "", LoggerType.StackTrace, stackTrace);
		}

		if (!Settings.ShowInUnity) return;

		Debug.Log(FormatStackTraceForUnity(stackTrace));
	}
}