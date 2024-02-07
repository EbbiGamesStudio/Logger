using System;

public static partial class DL
{
	private static string GetCurrentTime() =>
		string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
		DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);

	private static string GetCurrentTimeAndDate() =>
		string.Format("{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}.{6:D3}",
		DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
		DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);

	private static string GetCurrentTimeForFilename() =>
		string.Format("{0:D4}-{1:D2}-{2:D2} {3:D2}.{4:D2}.{5:D2}",
		DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
		DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
}