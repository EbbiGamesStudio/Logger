using UnityEngine;

public static partial class DL
{

 	public static void Break()
	{
		if (!isActive)
			return;

		UnityEngine.Debug.Break();
	}

	public static void ClearDeveloperConsole()
	{
		if (!isActive)
			return;

		UnityEngine.Debug.ClearDeveloperConsole();
	}

	public static void Log(object message)
	{
		// Jeśli debugowanie jest wyłączone, zakończ działanie metody
		if (!isActive)
			return;

		// Jeśli zapisywanie logów do pliku jest włączone, zapisz log
		if (SaveLogToFile)
			Logger(LoggerType.Log, "", message.ToString());

		// Jeśli wyświetlanie logów w Unity jest włączone, wyświetl log
		if (ShowLinesInUnity && ShowLogInUnity)
			UnityEngine.Debug.Log(message.ToString().Colored(defaultColor) + "\n");

	}

	public static void Log(string channel, object message)
	{
		// Jeśli debugowanie jest wyłączone, zakończ działanie metody
		if (!isActive)
			return;

		// Jeśli zapisywanie logów do pliku jest włączone, zapisz log
		if (SaveLogToFile)
			Logger(LoggerType.Log, channel, message.ToString());

		// Jeśli wyświetlanie logów w Unity jest włączone, wyświetl log
		if (ShowLogInUnity)
			UnityEngine.Debug.Log(string.Format("{0}: {1}\n", channel.Colored(defaultColor).Bold(), message.ToString().Colored(defaultColor)));
	}

	public static void Log(string channel, object message, params object[] args)
	{
		// Jeśli debugowanie jest wyłączone, zakończ działanie metody
		if (!isActive)
			return;

		// Jeśli zapisywanie logów do pliku jest włączone, zapisz log
		if (SaveLogToFile)
			Logger(LoggerType.Log, channel, string.Format(message.ToString(), args));

		// Jeśli wyświetlanie logów w Unity jest włączone, wyświetl log
		if (ShowLogInUnity)
			UnityEngine.Debug.Log(string.Format("{0}: {1}\n", channel.Colored(defaultColor).Bold(), string.Format(message.ToString().Colored(defaultColor), args)));
	}



	public static void LogError(object message)
	{
		// Jeśli debugowanie jest wyłączone, zakończ działanie metody
		if (!isActive)
			return;

		// Jeśli zapisywanie logów do pliku jest włączone, zapisz log
		if (SaveLogToFile)
			Logger(LoggerType.Error, "", message.ToString());

		// Jeśli wyświetlanie logów w Unity jest włączone, wyświetl log
		if (ShowLinesInUnity && ShowErrorInUnity)
			UnityEngine.Debug.LogError(message.ToString().Colored(defaultColor) + "\n");

		
	}

	public static void LogError(string channel, object message)
	{
		// Jeśli debugowanie jest wyłączone, zakończ działanie metody
		if (!isActive)
			return;

		// Jeśli zapisywanie logów do pliku jest włączone, zapisz log
		if (SaveLogToFile)
			Logger(LoggerType.Error, channel, message.ToString());

		// Jeśli wyświetlanie logów w Unity jest włączone, wyświetl log
		if (ShowErrorInUnity)
			UnityEngine.Debug.LogError(string.Format("{0}: {1}\n", channel.Colored(Colors.red).Bold(), message.ToString().Colored(defaultColor)));
	}

	public static void LogError(string channel, object message, params object[] args)
	{
		// Jeśli debugowanie jest wyłączone, zakończ działanie metody
		if (!isActive)
			return;

		// Jeśli zapisywanie logów do pliku jest włączone, zapisz log
		if (SaveLogToFile)
			Logger(LoggerType.Error, channel, string.Format(message.ToString(), args));

		// Jeśli wyświetlanie logów w Unity jest włączone, wyświetl log
		if (ShowErrorInUnity)
			UnityEngine.Debug.LogError(string.Format("{0}: {1}\n", channel.Colored(Colors.red).Bold(), string.Format(message.ToString().Colored(defaultColor), args)));
	}



	public static void LogInfo(object message)
	{
		// Jeśli debugowanie jest wyłączone, zakończ działanie metody
		if (!isActive)
			return;

		// Jeśli zapisywanie logów do pliku jest włączone, zapisz log
		if (SaveLogToFile)
			Logger(LoggerType.Info, "", message.ToString());

		// Jeśli wyświetlanie logów w Unity jest włączone, wyświetl log
		if (ShowLinesInUnity && ShowInfoInUnity)
			UnityEngine.Debug.Log(message.ToString().Colored(defaultColor) + "\n");
	}

	public static void LogInfo(string channel, object message)
	{
		// Jeśli debugowanie jest wyłączone, zakończ działanie metody
		if (!isActive)
			return;

		// Jeśli zapisywanie logów do pliku jest włączone, zapisz log
		if (SaveLogToFile)
			Logger(LoggerType.Info, channel, message.ToString());

		// Jeśli wyświetlanie logów w Unity jest włączone, wyświetl log
		if (ShowInfoInUnity)
			UnityEngine.Debug.Log(string.Format("{0}: {1}\n", channel.Colored(Colors.green).Bold(), message.ToString().Colored(defaultColor)));
	}

	public static void LogInfo(string channel, object message, params object[] args)
	{
		// Jeśli debugowanie jest wyłączone, zakończ działanie metody
		if (!isActive)
			return;

		// Jeśli zapisywanie logów do pliku jest włączone, zapisz log
		if (SaveLogToFile)
			Logger(LoggerType.Info, channel, string.Format(message.ToString(), args));

		// Jeśli wyświetlanie logów w Unity jest włączone, wyświetl log
		if (ShowInfoInUnity)
			UnityEngine.Debug.Log(string.Format("{0}: {1}\n", channel.Colored(Colors.green).Bold(), string.Format(message.ToString().Colored(defaultColor), args)));
	}



	public static void LogWarning(object message)
	{
		// Jeśli debugowanie jest wyłączone, zakończ działanie metody
		if (!isActive)
			return;

		// Jeśli zapisywanie logów do pliku jest włączone, zapisz log
		if (SaveLogToFile)
			Logger(LoggerType.Warning, "", message.ToString());

		// Jeśli wyświetlanie logów w Unity jest włączone, wyświetl log
		if (ShowLinesInUnity && ShowWarningInUnity)
			UnityEngine.Debug.LogWarning(message.ToString().Colored(defaultColor) + "\n");
	}

	public static void LogWarning(string channel, object message)
	{
		// Jeśli debugowanie jest wyłączone, zakończ działanie metody
		if (!isActive)
			return;

		// Jeśli zapisywanie logów do pliku jest włączone, zapisz log
		if (SaveLogToFile)
			Logger(LoggerType.Warning, channel, message.ToString());

		// Jeśli wyświetlanie logów w Unity jest włączone, wyświetl log
		if (ShowWarningInUnity)
			UnityEngine.Debug.LogWarning(string.Format("{0}: {1}\n", channel.Colored(Colors.yellow).Bold(), message.ToString().Colored(defaultColor)));
	}

	public static void LogWarning(string channel, object message, params object[] args)
	{
		// Jeśli debugowanie jest wyłączone, zakończ działanie metody
		if (!isActive)
			return;

		// Jeśli zapisywanie logów do pliku jest włączone, zapisz log
		if (SaveLogToFile)
			Logger(LoggerType.Warning, channel, string.Format(message.ToString(), args));

		// Jeśli wyświetlanie logów w Unity jest włączone, wyświetl log
		if (ShowWarningInUnity)
			UnityEngine.Debug.LogWarning(string.Format("{0}: {1}\n", channel.Colored(Colors.yellow).Bold(), string.Format(message.ToString().Colored(defaultColor), args)));
	}

	public static void LogException(System.Exception exception)
	{
		// Jeśli debugowanie jest wyłączone, zakończ działanie metody
		if (!isActive)
			return;
		
		// Jeśli wyświetlanie logów w Unity jest włączone, wyświetl log
		if (ShowExceptionInUnity)
			UnityEngine.Debug.LogException(exception);
	}

	public static void LogStackTrace(bool isDiagnosticStackTrace=false)
	{
		// Jeśli debugowanie jest wyłączone, zakończ działanie metody
		if (!isActive)
			return;

		// Jeśli zapisywanie logów do pliku jest włączone, zapisz log
		if (SaveLogToFile)
		{
            if (isDiagnosticStackTrace)
				Logger(LoggerType.Log, "", new System.Diagnostics.StackTrace().ToString());
			else
				Logger(LoggerType.Log, "", System.Environment.StackTrace);
		}
	}
}