public static partial class DL
{
	private static string Colored(this string source, string colorCode)
	{
		return $"<color={colorCode}>{source}</color>";
	}
	private static string[] Colored(this string[] source, string colorCode)
	{
		string[] temp = new string[source.Length];
		for (int i = 0; i < source.Length; i++)
			temp[i] = source[i].Colored(colorCode);
		return temp;
	}

	private static string Sized(this string source, int size)
	{
		return string.Format("<size={0}>{1}</size>", size, source);
	}
	private static string[] Sized(this string[] source, int size)
	{
		string[] temp = new string[source.Length];
		for (int i = 0; i < source.Length; i++)
			temp[i] = source[i].Sized(size);
		return temp;
	}

	private static string Bold(this string source)
	{
		return string.Format("<b>{0}</b>", source);
	}

	private static string[] Bold(this string[] source)
	{
		string[] temp = new string[source.Length];
		for (int i = 0; i < source.Length; i++)
			temp[i] = source[i].Bold();
		return temp;
	}

	private static string Italics(this string source)
	{
		return string.Format("<i>{0}</i>", source);
	}

	private static string[] Italics(this string[] source)
	{
		string[] temp = new string[source.Length];
		for (int i = 0; i < source.Length; i++)
			temp[i] = source[i].Italics();
		return temp;
	}

	private static string SetLength(this string source, int length)
	{
		// Zmienna tymczasowa, która przechowuje źródłowy ciąg znaków
		string temp_s = source;

		// Sprawdź, czy długość źródłowego ciągu znaków jest większa lub równa docelowej długości
		if (source.Length >= length)
			// Jeśli tak, zwróć źródłowy ciąg znaków bez zmian
			return source;
		else
		{
			// Jeśli nie, dodaj spacje do końca ciągu znaków, aż osiągnie docelową długość
			for (int i = 0; i < length - source.Length; i++)
				temp_s += " ";

			// Zwróć zmodyfikowany ciąg znaków
			return temp_s;
		}
	}
	private static string[] SetLength(this string[] source, int length)
	{
		string[] temp = new string[source.Length];
		for (int i = 0; i < source.Length; i++)
			temp[i] = source[i].SetLength(length);
		return temp;
	}

	public struct Colors
	{
		public const string Red = "#ff0000ff";
		public const string Yellow = "#ffff00ff";
		public const string Green = "#008000ff";
		public const string Cyan = "#00ffffff";
		public const string White = "#ffffffff";
		public const string Black = "#000000ff";
		public const string Magenta = "#ff00ffff";
		public const string Blue = "#0000ffff";
		public const string Gray = "#808080ff";
		public const string Silver = "#c0c0c0ff";
		public const string Maroon = "#800000ff";
		public const string Olive = "#808000ff";
		public const string Purple = "#800080ff";
		public const string Teal = "#008080ff";
		public const string Navy = "#000080ff";
		public const string Lime = "#00ff00ff";
	}
}