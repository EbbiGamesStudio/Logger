using System;
using System.Collections.Generic;
using System.IO;
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
		Initialize();
	}

	/// <summary>
	/// Statyczna metoda, która obsługuje zdarzenie otrzymania loga z aplikacji
	/// </summary>
	private static void Application_logMessageReceived(string condition, string stackTrace, LogType type)
	{
		// W tym miejscu wywołane najczęściej błędy, są przez Unity a nie z kodu przez użytkownika.
		// dlatego nie ma potrzeby wyświetlać ich w unity, a wystarczy zapisać do pliku.

		//Sprawdzenie, czy zapisywanie logów do pliku jest włączone
		if (SaveLogToFile)
		{
			// Wywołanie metody Logger z odpowiednimi parametrami, aby dodać log do listy
			// Regex zamienia znak nowej lini na na znak \n + długość wszystkich znaków w logu + 1
			// dzięki temu treść  loga nie nachodzi na datę i czas oraz na typ loga i kanał

			// **Tego nie ma potrzeby narazie ruszać, wcześniej było to wywoływane jeśli Debugger był zajęty**
			if (type == LogType.Exception)
				Logger((LoggerType)(int)type, "Runtime Error", Regex.Replace(condition, "\r\n", "\n".SetLength(allLengthInLog + 1)));

			// Sprawdzenie, czy typ loga jest różny od Log
			// W przypadku typu Log nie ma potrzeby dodawać linii stosu wywołań, ponieważ jest to log z kodu użytkownika.
			//if (type != LogType.Log)
			if (type == LogType.Exception)
			{
				// Przejście po wszystkich liniach stosu wywołań rozdzielonych znakiem nowej lini
				// Regex.Split(stackTrace, "\n") zwraca tablicę stringów, które są rozdzielone znakiem nowej lini
				foreach (string item in Regex.Split(stackTrace, "\n"))
				{
					// Sprawdzenie, czy linia stosu wywołań nie zaczyna się od "UnityEngine.Debug:" lub "Debug:Log" lub "DL"
					// Jeśli tak, to pomijamy tę linię bo jest to linia z kodu Debugera oraz z pozycji wywołania metody Logger
					// czego nie chcemy wyświetlać w logach
					if (item.StartsWith("UnityEngine.Debug:") ||
						item.StartsWith("Debug:Log") ||
						item.StartsWith("DL") ||
						item == "")
					{
						continue;
					}
					// Wywołanie metody Logger z typem StackTrace i pustym kanałem, aby dodać linię stosu wywołań do listy
					Logger(LoggerType.StackTrace, "", item);
				}
				// Wywołanie metody Logger z typem StackTrace i pustym kanałem, aby dodać linię pustą do listy
				// Dzięki temu logi będą lepiej oddzielone od siebie
				Logger(LoggerType.StackTrace, "", " ");
			}
		}
	}

	/// <summary>
	/// Statyczna metoda, która dodaje logi bezpośrednio do listy, 
	/// do której nie powinno być dostępu z innych metod
	/// </summary>
	private static void Logger(LoggerType type, string channel, string message)
	{
		//x logList.Add("##  DEBUG   ## " + type.ToString() + "#" + channel + "#" + message+ "#");
		// Najpierw sprawdzamy, czy jest to log wywołany z metody Application_logMessageReceived
		// oraz czy jest wywołany przez użytkownika z kodu typem LoggerType.Log i czy kanał jest pusty
		if ((type == LoggerType.Log || type == LoggerType.StackTrace) && channel == "")
		{
			// Jeśli tak, to dodajemy do listy logList tylko wiadomość
			logList.Add("".SetLength(9 + 12 + 9 + 30) + message);
		}
		else
		{
			// Jeśli nie, to dodajemy do listy logList sformatowaną wiadomość z aktualnym czasem, typem loga i kanałem
			logList.Add($"{GetCurrentTime().SetLength(12)} | {type.ToString().SetLength(9)} | {channel.SetLength(30)} | {message}");
		}
	}

	private static void Application_quitting() => SaveToFile();

	/// <summary>
	/// Wersja Debugera
	/// </summary>
	public const string Version = "v0.3";

	/// <summary>
	/// Pełna nazwa aplikacji
	/// </summary>
	public const string FullNameApplication = "Debuger " + Version;
	private const string separator = "======================================================================================";

	private const int timeLengthInLog = 12;
	private const int typeLengthInLog = 9;
	private const int channelLengthInLog = 30;
	private const int separatorsLengthInLog = 9;
	private const int allLengthInLog = timeLengthInLog + typeLengthInLog + channelLengthInLog + separatorsLengthInLog;

	private static bool isFirstRun = true;
	private static bool isActive = true;

	private static List<string> logList = new List<string>();
	private static string logFilePath = "";
	private const int maxLogFiles = 20;

	private static Colors defaultColor => Colors.white;
	public static bool DevelopmentBuild { get { return UnityEngine.Debug.isDebugBuild; } }

	//private static bool isBusy = false;
	public static void SetActive(bool active) => isActive = active;

	public static bool ShowLogInUnity = true;
	public static bool ShowLinesInUnity = true;
	public static bool ShowInfoInUnity = true;
	public static bool ShowWarningInUnity = true;
	public static bool ShowErrorInUnity = true;
	public static bool ShowExceptionInUnity = true;

	public static bool SaveLogToFile = true;
	public static bool GenerateHeaderFile = false;

	/// <summary>
	/// Metoda inicjalizująca Debuger
	/// </summary>
	public static void Initialize()
	{
		logList = new List<string>();
		logFilePath = Application.persistentDataPath + $@"\{GetCurrentTimeForFilename()}.log";
		InsertFileHeader($"[{GetCurrentTimeAndDate()}]");
		RemoveExcessLogFiles();
	}

	/// <summary>
	/// Usuwanie nadmiarowych plików z logami.
	/// </summary>
	private static void RemoveExcessLogFiles()
	{
		// Sprawdzenie, czy istnieje folder z logami
		DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);

		// Pobranie wszystkich plików z rozszerzeniem .log
		FileInfo[] filesArray = directoryInfo.GetFiles("*.log", SearchOption.TopDirectoryOnly);

		// Sprawdzenie, czy liczba plików z logami jest większa niż maksymalna liczba plików
		if (filesArray.Length <= maxLogFiles)
		{
			// Jeśli nie, to zakończ metodę
			return;
		}

		// Obliczenie liczby plików do usunięcia
		int countFileToRemove = filesArray.Length > maxLogFiles ? filesArray.Length - maxLogFiles : 0;

		// Posortowanie plików po dacie utworzenia
		Array.Sort(filesArray, (x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.CreationTime, y.CreationTime));

		// Usunięcie plików
		for (int i = 0; i < countFileToRemove; i++)
		{
			filesArray[i].Delete();
		}
	}

	/// <summary>
	/// Metoda czyszcząca listę logów
	/// </summary>
	public static void Clear() => logList.Clear();

	/// <summary>
	/// Metoda zapisjuąca do pliku logi z listy stosująca strumieniowanie
	/// z możliwością zapisu do pliku w trakcie działania aplikacji
	/// </summary>
	public static void SaveToFile()
	{
		// Sprawdzenie, czy zapisywanie logów do pliku jest włączone
		if (SaveLogToFile)
		{
			// Sprawdzenie, czy metoda nie jest zajęta innym zapytaniem
			// Sprawdzenie, czy lista logów nie jest pusta
			if (logList.Count > 0)
			{
				// Sprawdzenie, czy plik z logami istnieje
				if (File.Exists(logFilePath))
				{
					// Jeśli tak, to otwórz plik w trybie dopisywania
					using (StreamWriter streamWriter = File.AppendText(logFilePath))
					{
						// Zapisz wszystkie logi z listy do pliku
						logList.ForEach(item => streamWriter.WriteLine(item));
					}
				}
				else
				{
					// Jeśli nie, to utwórz nowy plik i zapisz w nim wszystkie logi z listy
					using (StreamWriter streamWriter = File.CreateText(logFilePath))
					{
						logList.ForEach(item => streamWriter.WriteLine(item));
					}
				}
			}
		}
	}

	public static void AddSeparator() => logList.Add(separator);

	private static void InsertFileHeader(string currentDateAndTime)
	{
		if (GenerateHeaderFile)
		{
			logList.Add(separator);
			logList.Add(" Application Start at: " + currentDateAndTime);
			logList.Add(separator);
			// DebugParametrs
			logList.Add(separator);
			logList.Add("Application");
			logList.Add(separator);
			logList.Add(string.Format("DataPath              :{0}", Application.dataPath));
			logList.Add(string.Format("PersistentDataPath    :{0}", Application.persistentDataPath));
			logList.Add(string.Format("StreamingAssetsPath   :{0}", Application.streamingAssetsPath));
			logList.Add(string.Format("TemporaryCachePath    :{0}", Application.temporaryCachePath));
			logList.Add(string.Format("Engine Version        :{0}", Application.unityVersion));
			logList.Add(string.Format("platform              :{0}", Application.platform));
			logList.Add(string.Format("SystemLanguage        :{0}", Application.systemLanguage.ToString()));
			logList.Add(string.Format("TargetFrameRate       :{0}", Application.targetFrameRate));
			logList.Add(separator);
			logList.Add("QualitySettings");
			logList.Add(separator);
			logList.Add(string.Format("Current Quality       :{0}", QualitySettings.names[QualitySettings.GetQualityLevel()]));
			logList.Add(string.Format("anisotropicFiltering  :{0}", QualitySettings.anisotropicFiltering));
			logList.Add(string.Format("antiAliasing          :{0}", QualitySettings.antiAliasing));
			logList.Add(string.Format("vSyncCount            :{0}", QualitySettings.vSyncCount));
			logList.Add(separator);
			logList.Add("SystemInfo");
			logList.Add(separator);
			logList.Add(string.Format("Resolution                :{0}x{1}", Screen.width, Screen.height));
			logList.Add(string.Format("deviceName                :{0}", SystemInfo.deviceName));
			logList.Add(string.Format("deviceType                :{0}", SystemInfo.deviceType));
			logList.Add(string.Format("deviceModel               :{0}", SystemInfo.deviceModel));
			logList.Add(string.Format("graphicsDeviceID          :{0}", SystemInfo.graphicsDeviceID));
			logList.Add(string.Format("graphicsDeviceName        :{0}", SystemInfo.graphicsDeviceName));
			logList.Add(string.Format("graphicsDeviceVendor      :{0}", SystemInfo.graphicsDeviceVendor));
			logList.Add(string.Format("graphicsDeviceVendorID    :{0}", SystemInfo.graphicsDeviceVendorID));
			logList.Add(string.Format("graphicsDeviceVersion     :{0}", SystemInfo.graphicsDeviceVersion));
			logList.Add(string.Format("graphicsMemorySize        :{0}", SystemInfo.graphicsMemorySize));
			logList.Add(string.Format("graphicsShaderLevel       :{0}", SystemInfo.graphicsShaderLevel));
			logList.Add(string.Format("operatingSystem           :{0}", SystemInfo.operatingSystem));
			logList.Add(string.Format("processorCount            :{0}", SystemInfo.processorCount));
			logList.Add(string.Format("processorType             :{0}", SystemInfo.processorType));
			logList.Add(string.Format("systemMemorySize          :{0}", SystemInfo.systemMemorySize));
			logList.Add(separator);
		}
		else
		{
			logList.Add(separator);
			logList.Add(" Application Start at: " + currentDateAndTime);
			logList.Add(separator);
		}
	}
}
