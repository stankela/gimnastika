using System;

namespace Gimnastika
{
	public class DateUtilities
	{
		public static string serbianDateStr(DateTime date, char delimiter)
		{
			int d = date.Day;
			int m = date.Month;
			int y = date.Year;

			string dd, mm, yyyy;
			dd = d.ToString();
			if (d < 10)
				dd = "0" + dd;
			mm = m.ToString();
			if (m < 10)
				mm = "0" + mm;
			yyyy = y.ToString();

			return dd + delimiter + mm + delimiter + yyyy;
		}

		public static DateTime serbianDateToDateTime(string datum)
		{
			datum = datum.Trim();
			int i;
			for (i = 0; i < datum.Length; i++)
			{
				if (!Char.IsDigit(datum[i]))
					break;
			}
			char delimiter = datum[i];
			int i2 = datum.IndexOf(delimiter, i + 1);

			string dd = datum.Substring(0, i);
			string mm = datum.Substring(i + 1, i2 - (i + 1));
			string yyyy = datum.Substring(i2 + 1);

			int d = Convert.ToInt32(dd);
			int m = Convert.ToInt32(mm);
			int y = Convert.ToInt32(yyyy);

			return new DateTime(y, m, d);
		}

	}
}
