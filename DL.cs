using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

using UnityEngine;

/// <summary>
/// Klasa DL zawiera narzędzia do logowania i debugowania aplikacji.
/// </summary>
public static partial class DL
{
	static DL()
	{
		Application.logMessageReceived += Application_logMessageReceived;
		Application.quitting += Application_quitting;
		logList = new List<string>();
		timer = new System.Threading.Timer((e) => SaveToFile(), null, 0, 5000);
	}

	/// <summary>
	/// Statyczna metoda, która obsługuje zdarzenie otrzymane z Unity
	/// </summary>
	private static void Application_logMessageReceived(string condition, string stackTrace, LogType type)
	{
		// Jeśli debugowanie jest wyłączone, zakończ działanie metody
		if (!isActive)
			return;

		// Jeśli zapisywanie logów do pliku jest włączone, zapisz log do listy
		if (Settings.SaveToFileEnabled)
		{
			// Sformatuj wpis do listy
			SendLogEntryFromUnityToFile(condition, stackTrace, type);
		}
	}

	private static void Application_quitting()
	{
		// Jeśli zapisywanie logów do pliku jest włączone, zapisz log
		if (Settings.SaveToFileEnabled)
			SaveToFile();

		// Zatrzymaj timer przy zniszczeniu obiektu
		if (timer != null)
		{
			timer.Dispose();
		}
	}

	private static void SendLogEntryFromUnityToFile(string condition, string stackTrace, LogType type)
	{
		// ze względu że kolorowanie w konsoli zapisuje się też do pliku, oraz to dubluje wpisy w pliku
		// to LogEntry z Unity będzie przejmował tylko Exceptions
		if (type != LogType.Exception)
			return;

		const string sep = " | ";

		// Ustawienie tytułu loga
		// Format: [12:00:00.000 | LogType | Unity Runtine | Log message]
		string logTitle = string.Concat(
							GetCurrentTime().SetLength(LogEntryTimeLength),
							sep,
							type.ToString().SetLength(LogEntryTypeLength),
							sep,
							"Unity Runtine".SetLength(LogEntryTitleLength),
							sep,
							condition);

		// Dodaj log do listy
		AddToFileQueue(logTitle);

		// Jeśli zapisywanie śladu stosu do pliku jest włączone, zapisz ślad stosu
		if (!Settings.FromUnity_StackTrace_Enabled)
			return;

		// Sformatuj ślad stosu
		string stack = $"{stackTrace}";

		// stwórz tablicę linii ze śladu stosu
		string[] stackLines = stack.Split('\n');



		stackLines = stackLines
			.Where(line => !string.IsNullOrWhiteSpace(line)                      // Usuń puste linie ze śladu stosu
						&& !line.StartsWith("UnityEngine.Debug:")                // Usuń linie z Unity
						&& !line.StartsWith("UnityEngine.StackTraceUtility:")    // Usuń linie z Unity
						&& !line.StartsWith("UnityEditor")                       // Usuń linie z Unity Editor
						&& !line.StartsWith("DL")).ToArray();                    // Usuń linie z debugera

		for (int i = 0; i < stackLines.Length; i++)
		{
			string[] parts = stackLines[i].Split(new string[] { " (at " }, StringSplitOptions.None);
			if (parts.Length == 2)
			{
				// Pierwsza część zawiera nazwę klasy i metodę
				string classAndMethod = parts[0];

				// Druga część zawiera ścieżkę pliku i numer linii
				string filePathAndLine = parts[1];

				// Usuń zbędne nawiasy i nawiasy kwadratowe
				// classAndMethod = classAndMethod.Replace(" ()", "");
				filePathAndLine = filePathAndLine.Replace(")", "").Replace("]", "");

				// Sformatuj linię
				stackLines[i] = classAndMethod + "\n"
					+ new string(' ', LogEntryLeftMargin) + sep
					+ "".SetLength(Settings.MarginForStackTraceInFile)
					+ filePathAndLine + "\n"
					+ new string(' ', LogEntryLeftMargin) + sep;
			}
		}

		stackLines = stackLines.Select(line =>
				new string(' ', LogEntryLeftMargin)
				+ sep
				+ "".SetLength(Settings.MarginForStackTraceInFile)
				+ line)    // Dodaj wcięcie do każdej linii
			.ToArray();

		//stack = string.Join("\n", stackLines)
		//		+ "\n" + new string(' ', LogEntryLeftMargin)
		//		+ sep;
		stack = string.Join("\n", stackLines);

		AddToFileQueue(stack);

	}

	private static void SendLogEntryFromDirectToFile(string titleLogEntry, string messageLogEntry, LoggerType logType, string stackTrace = "")
	{
		//todo obróbkę stacktrace zrobić przy zapisie do pliku.
		// do tego pomyśleć o buforze i zapisie do pliku w tle bądź co jakiś czas.
		const string sep = " | ";
		string logTitle;

		if (logType == LoggerType.Separator)
		{
			logTitle = separator;
			AddToFileQueue(logTitle);
			return;
		}
		else if (logType == LoggerType.Line)
		{
			// Ustawienie tytułu loga
			// Format: [                       | Unity Runtine | Log message]
			if (titleLogEntry == "")
			{
				logTitle = string.Concat(
					"".SetLength(LogEntryLeftMargin),
					sep,
					messageLogEntry);
			}
			else
			{
				logTitle = string.Concat(
									"".SetLength(LogEntryTimeLength),
									"".SetLength(LogEntrySeparatorLength),
									"".SetLength(LogEntryTypeLength),
									sep,
									titleLogEntry.SetLength(LogEntryTitleLength),
									sep,
									messageLogEntry);
			}
		}
		else
		{
			// Ustawienie tytułu loga
			// Format: [12:00:00.000 | LogType | Unity Runtine | Log message]
			logTitle = string.Concat(
								GetCurrentTime().SetLength(LogEntryTimeLength),
								sep,
								logType.ToString().SetLength(LogEntryTypeLength),
								sep,
								titleLogEntry.SetLength(LogEntryTitleLength),
								sep,
								messageLogEntry);
		}

		// Dodaj log do listy
		AddToFileQueue(logTitle);

		// Jeśli nie ma śladu stosu, zakończ działanie metody
		if (string.IsNullOrEmpty(stackTrace))
			return;

		AddToFileQueue(FormatStackTrace(stackTrace));
	}

	private static string FormatStackTrace(string stackTrace)
	{
		// Sformatuj ślad stosu
		string stack = $"{stackTrace}";

		// stwórz tablicę linii ze śladu stosu
		string[] stackLines = stack.Split('\n');

		stackLines = stackLines
			.Where(line => !string.IsNullOrWhiteSpace(line)                      // Usuń puste linie ze śladu stosu
						&& !line.StartsWith("UnityEngine.Debug:")                // Usuń linie z Unity
						&& !line.StartsWith("UnityEngine.StackTraceUtility:")    // Usuń linie z Unity
						&& !line.StartsWith("UnityEditor")                       // Usuń linie z Unity Editor
						&& !line.StartsWith("DL")).ToArray();                    // Usuń linie z debugera

		for (int i = 0; i < stackLines.Length; i++)
		{
			string[] parts = stackLines[i].Split(new string[] { " (at " }, StringSplitOptions.None);
			if (parts.Length == 2)
			{
				// Pierwsza część zawiera nazwę klasy i metodę
				string classAndMethod = parts[0];

				// Druga część zawiera ścieżkę pliku i numer linii
				string filePathAndLine = parts[1];

				// Usuń zbędne nawiasy i nawiasy kwadratowe
				// classAndMethod = classAndMethod.Replace(" ()", "");
				filePathAndLine = filePathAndLine.Replace(")", "").Replace("]", "");

				// Sformatuj linię
				stackLines[i] = classAndMethod + "\n"
								+ new string(' ', LogEntryLeftMargin) + " | "
								+ "".SetLength(Settings.MarginForStackTraceInFile)
								+ filePathAndLine + "\n"
								+ new string(' ', LogEntryLeftMargin) + " | ";
			}
		}

		stackLines = stackLines.Select(line =>
						new string(' ', LogEntryLeftMargin)
						+ " | "
						+ "".SetLength(Settings.MarginForStackTraceInFile)
						+ line)    // Dodaj wcięcie do każdej linii
						.ToArray();

		stack = string.Join("\n", stackLines);
		return stack;
	}

	private static string FormatStackTraceForUnity(string stackTrace)
	{
		// Sformatuj ślad stosu
		string stack = $"{stackTrace}";

		// stwórz tablicę linii ze śladu stosu
		string[] stackLines = stack.Split('\n');

		stackLines = stackLines
			.Where(line => !string.IsNullOrWhiteSpace(line)                      // Usuń puste linie ze śladu stosu
						&& !line.StartsWith("UnityEngine.Debug:")                // Usuń linie z Unity
						&& !line.StartsWith("UnityEngine.StackTraceUtility:")    // Usuń linie z Unity
						&& !line.StartsWith("UnityEditor")                       // Usuń linie z Unity Editor
						&& !line.StartsWith("DL")).ToArray();                    // Usuń linie z debugera

		for (int i = 0; i < stackLines.Length; i++)
		{
			string[] parts = stackLines[i].Split(new string[] { " (at " }, StringSplitOptions.None);
			if (parts.Length == 2)
			{
				// Pierwsza część zawiera nazwę klasy i metodę
				string classAndMethod = parts[0];

				// Druga część zawiera ścieżkę pliku i numer linii
				string filePathAndLine = parts[1];

				// Usuń zbędne nawiasy i nawiasy kwadratowe
				// classAndMethod = classAndMethod.Replace(" ()", "");
				filePathAndLine = filePathAndLine.Replace(")", "").Replace("]", "");

				// Sformatuj linię
				stackLines[i] = classAndMethod + "\n"
								+ "".SetLength(Settings.MarginForStackTraceInFile)
								+ filePathAndLine + "\n";
			}
		}

		stackLines = stackLines.Select(line =>
						"".SetLength(Settings.MarginForStackTraceInFile)
						+ line)    // Dodaj wcięcie do każdej linii
						.ToArray();

		stack = string.Join("\n", stackLines);
		return stack;
	}

	/// <summary>
	/// Inicjalizacja debugera
	/// </summary>
	public static void Initialize(DLSettings settings)
	{
		Settings = settings;

		logList.Clear();

		if (Settings.GenerateFileHeader)
		{
			logList.Insert(0, separator);
			logList.Insert(0, $" Application Start at: {GetCurrentTimeForFilename()}");
			logList.Insert(0, separator);
		}

		filePath = Application.persistentDataPath + $@"\{GetCurrentTimeForFilename()}.log";
		RemoveExcessLogFiles();
	}

	/// <summary>
	/// Czyszczenie listy logów
	/// </summary>
	public static void Clear()
	{
		// Jeśli debugowanie jest wyłączone, zakończ działanie metody
		if (!isActive)
			return;

		logList.Clear();
	}

	/// <summary>
	/// Metoda zapisjuąca do pliku logi z listy stosująca strumieniowanie
	/// z możliwością zapisu do pliku w trakcie działania aplikacji
	/// </summary>
	private static void SaveToFile()
	{
		// Sprawdzenie, czy lista logów jest pusta
		// Jeśli tak, zakończ działanie metody
		if (logList.Count == 0)
			return;

		File.AppendAllLines(filePath, logList);
		logList.Clear();
		bufferIndex = 0;
	}

	private static void CheckBufferSaveFile()
	{
		if (logList.Count == 0)
			return;

		if (Settings.IsBuffered && bufferIndex >= Settings.BufferSize)
		{
			SaveToFile();
			bufferIndex = 0;
		}
	}

	private static void StreamToFile(string log)
	{
		File.AppendAllLines(filePath, logList);
	}

	/// <summary>
	/// Usuwanie nadmiarowych plików z logami.
	/// </summary>
	private static void RemoveExcessLogFiles()
	{
		// Sprawdzenie, czy istnieje folder z logami
		DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);

		// Pobranie wszystkich plików z rozszerzeniem .log
		FileInfo[] logFiles = directoryInfo.GetFiles("*.log", SearchOption.TopDirectoryOnly);

		// Sprawdzenie, czy liczba plików z logami jest większa niż maksymalna liczba plików
		if (logFiles.Length <= Settings.File_MaximumFilesCount)
		{
			// Jeśli nie, to zakończ metodę
			return;
		}

		var filesToRemove = logFiles
			.OrderBy(file => file.CreationTime)
			.Take(logFiles.Length - Settings.File_MaximumFilesCount);

		foreach (var file in filesToRemove)
		{
			file.Delete();
		}
	}

	private static void AddToFileQueue(string log)
	{
		logList.Add(log);
		bufferIndex++;
		CheckBufferSaveFile();
	}
}
