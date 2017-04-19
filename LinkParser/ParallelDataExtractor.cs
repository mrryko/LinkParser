using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace LinkParser
{
	class ParallelDataExtractor
	{
		private event EventHandler<MatchFoundEventArgs> OnMatchFound;
		private readonly string _data;
		ConcurrentBag<string> conBag = new ConcurrentBag<string>();

		#region constructors, destructors
		public ParallelDataExtractor(string data)
		{
			_data = data;
			OnMatchFound += MatchFoundEventHandler;
		}

		~ParallelDataExtractor()
		{
			OnMatchFound -= MatchFoundEventHandler;
			OnMatchFound = null;
		}
		#endregion

		#region public methods
		public List<string> ExtractDataAsParallel(params string[] regexes)
		{
			List<Thread> threads = new List<Thread>();
			foreach (var regex in regexes)
			{
				var thread = new Thread(new ParameterizedThreadStart(Extract));
				thread.Start(regex);
				threads.Add(thread);
			}
			foreach (var thread in threads)
			{
				thread.Join();
			}
			return conBag.ToList();
		}
		#endregion

		#region private methods
		private void Extract(object rawRegex)
		{
			string regex = (string)rawRegex;
			Match m = Regex.Match(_data, regex, RegexOptions.IgnoreCase);
			while (m.Success)
			{
				OnMatchFound?.Invoke(this, new MatchFoundEventArgs(m.Value));
				m = m.NextMatch();
			}
		}

		private void MatchFoundEventHandler(object sender, MatchFoundEventArgs e)
		{
			Console.WriteLine($"Match found: {e.Match}");
			conBag.Add(e.Match);
		}
		#endregion
	}
}
