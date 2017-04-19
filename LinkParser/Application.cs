using System;
using System.IO;
using System.Net;
using System.Threading;

namespace LinkParser
{

	public class Application
	{
		#region public methods
		static void Main(string[] args)
		{
			Console.WriteLine("This app helps you parse URLs and emails from file or website. ");
			Console.WriteLine(@"If you want to close an app type ""stop"" ");
			while (true)
			{				
				Parse();
			}
		}

		public static string ReadSourceFromFile(string source)
		{
			return File.ReadAllText(source);
		}

		public static string ReadSourceFromWebPage(string theURL)
		{
			Console.WriteLine("Downloading data...");
			using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
			{
				return client.DownloadString(new Uri(theURL));
			}
		}
		#endregion

		#region private methods
		private static void Parse()
		{
			Console.WriteLine(@"Input full filepath or full URL (http:// or https://)");
			string data = GetData();
			if (data != null)
			{
				DataExtractor.ExtractData(data);
				Console.WriteLine();
				Console.WriteLine("Done. Press any key to continue...");
				Console.ReadKey();
			}
			else
			{
				Console.WriteLine("Source is empty, check it and try again");
			}
		}

		private static string GetData()
		{
			string source = null;
			FileType ft = GetDataFromConsole(out source);
			switch (ft)
			{
				case FileType.File:
					return ReadSourceFromFile(source);
				case FileType.WebSite:
					return ReadSourceFromWebPage(source);
				default:
					return null;
			}
		}

		private static FileType GetDataFromConsole(out string source)
		{
			do
			{
				source = Console.ReadLine().Trim();
				if (source.Equals("stop", StringComparison.OrdinalIgnoreCase))
				{
					CloseApplication();
				}
				if (Uri.IsWellFormedUriString(source, UriKind.Absolute))
				{
					if (CheckIfWebSiteExists(source))
					{
						return FileType.WebSite;
					}
				}
				else if (File.Exists(source))
				{
					return FileType.File;
				}
				else
				{
					Console.WriteLine("Wrong link or path!");
				}
			}
			while (true);
		}

		private static bool CheckIfWebSiteExists(string url)
		{
			Console.WriteLine("Verifying URL...");
			try
			{
				var request = WebRequest.Create(url) as HttpWebRequest;
				if (request == null)
				{
					return false;
				}
				request.Method = "HEAD";
				using (var response = (HttpWebResponse)request.GetResponse())
				{
					return response.StatusCode == HttpStatusCode.OK;
				}
			}
			catch (UriFormatException ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			catch (WebException ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}


		private static void CloseApplication()
		{
			Console.WriteLine("App is closing...");
			Thread.Sleep(2000);
			Environment.Exit(0);
		}
		#endregion

	}
}
