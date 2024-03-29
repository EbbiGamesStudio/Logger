﻿public static partial class DL
{
	/// <summary>
	/// Typ wyliczeniowy, który reprezentuje różne rodzaje logów
	/// </summary>
	public enum LoggerType : int
	{
		Error = 0,      // Log błędu
		Info = 1,       // Log informacyjny
		Warning = 2,    // Log ostrzeżenia
		Log = 3,        // Log zwykły
		Exception = 4,  // Log wyjątku
		StackTrace = 5, // Log śladu stosu
	}
}