using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ExtendedSearch.Data;
using ExtendedSearch.Models;

namespace ExtendedSearch.Controllers
{
	public class SearchController : Controller
	{
		private readonly ExtendedSearchEntities _extendedSearchContext;

		public SearchController()
		{
			_extendedSearchContext = new ExtendedSearchEntities();
		}

		public SearchController(ExtendedSearchEntities extendedSearchContext)
		{
			_extendedSearchContext = extendedSearchContext;
		}

		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<JsonResult> GetResult(SearchInput term)
		{
			try
			{
				var searchDataContext = new SearchData(_extendedSearchContext);
				var listOfSe = searchDataContext.GetSearchEngines();
				var downloadTasks = listOfSe.Select(searchEngine => ProcessSe(term.Searchinput, searchEngine.EngineName, searchEngine.Url)).ToList();
				var firstFinishedTask = await Task.WhenAny(downloadTasks);
				var responseList = await firstFinishedTask;
				var searchResult = responseList.Response;
				var msg = responseList.EngName;
				var engine = searchDataContext.GetEngineByName(msg);
				searchDataContext.StoreSearchResult(term.Searchinput, engine.Id, searchResult);
				return Json(new { msg, searchResult });
			}
			catch (Exception ex)
			{
				return Json(new { ex.Message, string.Empty });
			}
		}

		private async Task<SearchResponce> ProcessSe(string term, string se, string url)
		{

			//if (se == "GoogleSe")
			//{
			//	await Task.Delay(1000);
			//}

			//if (se == "GoogleSe" || se == "BingSe")
			//{
			//	await Task.Delay(1000);
			//}

			return await Process(term, se, url);
		}

		private async Task<SearchResponce> Process(string term, string se, string url)
		{

			var gs = string.Format(url, term);

			var response = string.Empty;
			var httpWebRequest = WebRequest.Create(gs) as HttpWebRequest;

			if (httpWebRequest != null)
			{
				httpWebRequest.Method = WebRequestMethods.Http.Get;
				httpWebRequest.Headers.Add("Accept-Language", "en-US");
				httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Win32)";
				httpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");

				using (var httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse)
				{
					Stream stream;
					if (httpWebResponse == null) return new SearchResponce(response, se);
					using (stream = httpWebResponse.GetResponseStream())
					{
						if (httpWebResponse.ContentEncoding.ToLower().Contains("gzip"))
							stream = new GZipStream(stream, CompressionMode.Decompress);
						else if (httpWebResponse.ContentEncoding.ToLower().Contains("deflate"))
							stream = new DeflateStream(stream, CompressionMode.Decompress);

						var streamReader = new StreamReader(stream, Encoding.UTF8);
						response = streamReader.ReadToEnd();
					}
				}
			}

			//if (se == "GoogleSe")
			//{
			//	await Task.Delay(1000);
			//}

			//if (se == "GoogleSe" || se == "BingSe")
			//{
			//	await Task.Delay(1000);
			//}

			return new SearchResponce(response, se);
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_extendedSearchContext.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}