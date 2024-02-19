public static partial class DL
{
	public struct DLSettings
	{	
		// Ustawienia ogólne
		public int File_MaximumFilesCount { get; set; }
		public bool GenerateFileHeader { get; set; }

		public int MarginForStackTraceInFile { get; set; }

		// Ustawienia wyświetlania StackTrace z Unity
		public bool FromUnity_StackTrace_Enabled { get; set; }

		// Ustawienia buforowania zapisu
		public bool IsBuffered { get; set; }
		public int BufferSize { get; set; }
		public int SaveTimeInterval { get; set; }

		// Ustawienia główne
		public bool ShowInUnity { get; set; }
		public bool SaveToFileEnabled { get; set; }

		// Ustawienia Line
		public bool Line_ShowInUnity { get; set; }
		public bool Line_SaveToFile { get; set; }

		// Ustawienia Log
		public bool Log_ShowInUnity { get; set; }
		public bool Log_SaveToFile { get; set; }
		public bool Log_StackTrace_ShowInUnity { get; set; }
		public bool Log_StackTrace_SaveToFile { get; set; }
		
	}



}