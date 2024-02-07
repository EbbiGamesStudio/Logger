public static partial class DL
{
	/// <summary>
	/// Metoda rozszerzająca, która zwraca ciąg znaków w kolorze określonym przez parametr color
	/// </summary>
	private static string Colored(this string source, Colors color) => string.Format("<color={0}>{1}</color>", color.ToString(), source);
	/// <summary>
	/// Metoda rozszerzająca, która zwraca ciąg znaków w kolorze określonym przez parametr colorCode
	/// </summary>
	private static string Colored(this string source, string colorCode) => string.Format("<color={0}>{1}</color>", colorCode, source);
	/// <summary>
	/// Metoda rozszerzająca, która zwraca ciąg znaków w rozmiarze określonym przez parametr size
	/// </summary>
	private static string Sized(this string source, int size) => string.Format("<size={0}>{1}</size>", size, source);
	/// <summary>
	/// Metoda rozszerzająca, która zwraca ciąg znaków wytłuszczony
	/// </summary>
	private static string Bold(this string source) => string.Format("<b>{0}</b>", source);
	/// <summary>
	/// Metoda rozszerzająca, która zwraca ciąg znaków pochylony
	/// </summary>
	private static string Italics(this string source) => string.Format("<i>{0}</i>", source);

	/// <summary>
	/// Metoda rozszerzająca, która ustawia długość ciągu znaków
	/// </summary>
	/// <param name="source">tekst do edycji</param>
	/// <param name="length">ilość wymaganych znaków</param>
	/// <returns></returns>
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

	public enum Colors
	{
		aqua,
		black,
		blue,
		brown,
		cyan,
		darkblue,
		fuchsia,
		green,
		grey,
		lightblue,
		lime,
		magenta,
		maroon,
		navy,
		olive,
		purple,
		red,
		silver,
		teal,
		white,
		yellow
	}
}