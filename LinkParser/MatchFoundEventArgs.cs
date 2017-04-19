using System;

namespace LinkParser
{
	public class MatchFoundEventArgs : EventArgs
	{
		public string Match { get; set; }

		public MatchFoundEventArgs(string match)
		{
			Match = match;
		}
	}
}
