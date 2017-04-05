using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ExtendedSearch.Data;
using ExtendedSearch.Models;
using PagedList;

namespace ExtendedSearch.Controllers
{
	public class SearchResultsController : Controller
	{
		private readonly ExtendedSearchEntities _extendedSearchContext;

		public SearchResultsController()
		{
			_extendedSearchContext = new ExtendedSearchEntities();
		}

		public SearchResultsController(ExtendedSearchEntities extendedSearchContext)
		{
			_extendedSearchContext = extendedSearchContext;
		}

		public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
		{
			ViewBag.CurrentSort = sortOrder;
			ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			
			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;

			var searchDataContext = new SearchData(_extendedSearchContext);
			var searches = searchDataContext.GetSearchResults();

			if (!string.IsNullOrEmpty(searchString))
			{
				searches = searches.Where(s => s.SearchFor != null).Where(s => s.SearchFor.ToLower().Contains(searchString.ToLower())).ToList();
			}
			switch (sortOrder)
			{
				case "name_desc":
					searches = searches.OrderByDescending(s => s.SearchFor).ToList();
					break;
				default:
					searches = searches.OrderBy(s => s.SearchFor).ToList();
					break;
			}

			var pageSize = 5;
			var pageNumber = (page ?? 1);

			var viewResultModel = (from search in searches
				let engine = search.Engine.HasValue 
					? searchDataContext.GetEngine(search.Engine.Value) 
					: new Engine()
				where engine != null
				select new SearchViewModel
				{
					Id = search.Id,
					SearchFor = search.SearchFor,
					SearchResult = search.SearchResult,
					EngineName = search.Engine != null && searchDataContext.GetEngine(search.Engine.Value) != null 
						? engine.EngineName 
						: string.Empty
				}).ToList();

			return View(viewResultModel.ToPagedList(pageNumber, pageSize));
		}

		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var searchDataContext = new SearchData(_extendedSearchContext);
			var search = searchDataContext.GetSearchResultById(id.Value);
			
			if (search == null)
			{
				return HttpNotFound();
			}
			return View(search);
		}
	}
}