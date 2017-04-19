using System.Collections.Generic;

namespace LinkParser
{
	public class DataExtractor
	{
		private const string LINK_TEMPLATE = @"(((ht|f)tp(s?)\:\/\/)|(www.))([0-9a-z]([-.\w]*[0-9a-z])*(:(0-9)*)*(\/?)([a-z0-9\-\.\?\,\'\/\\\+&amp;%\$()=\*#_]*))";
		private const string EMAIL_TEMPLATE = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
		#region public methods
		public static List<string> ExtractData(string data)
		{
			System.Console.WriteLine();
			ParallelDataExtractor parallelLink = new ParallelDataExtractor(data);
			return parallelLink.ExtractDataAsParallel(LINK_TEMPLATE, EMAIL_TEMPLATE);
		}
		#endregion
	}
}
