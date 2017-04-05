using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ExtendedSearch.Data;
using PagedList;

namespace ExtendedSearch.Controllers
{
	public class SearchConfigurationController : Controller
	{
		private readonly ExtendedSearchEntities _extendedSearchContext;

		public SearchConfigurationController()
		{
			_extendedSearchContext = new ExtendedSearchEntities();
		}

		public SearchConfigurationController(ExtendedSearchEntities extendedSearchContext)
		{
			_extendedSearchContext = extendedSearchContext;
		}

		public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
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
			var engines = searchDataContext.GetSearchEngines();

			if (!string.IsNullOrEmpty(searchString))
			{
				engines = engines.Where(s => s.EngineName.ToLower().Contains(searchString.ToLower()));
			}
			switch (sortOrder)
			{
				case "name_desc":
					engines = engines.OrderByDescending(s => s.EngineName);
					break;
				default:
					engines = engines.OrderBy(s => s.EngineName);
					break;
			}

			int pageSize = 5;
			int pageNumber = (page ?? 1);
			return View(engines.ToPagedList(pageNumber, pageSize));
		}

		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var searchDataContext = new SearchData(_extendedSearchContext);
			var engine = searchDataContext.GetEngine(id.Value);
			
			if (engine == null)
			{
				return HttpNotFound();
			}
			return View(engine);
		}

		public ActionResult Create()
		{
			return View();
		}

		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "EngineName, Url")]Engine engine)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var searchDataContext = new SearchData(_extendedSearchContext);
					var engineList = searchDataContext.AddEngine(engine);
					if (engineList != null)
					{
						return RedirectToAction("Index");
					}
					ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
				}
			}
			catch (RetryLimitExceededException)
			{
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return View(engine);
		}


		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var searchDataContext = new SearchData(_extendedSearchContext);
			var engine = searchDataContext.GetEngine(id.Value);

			if (engine == null)
			{
				return HttpNotFound();
			}
			return View(engine);
		}

		
		[HttpPost, ActionName("Edit")]
		[ValidateAntiForgeryToken]
		public ActionResult EditPost(Engine model)
		{
			if (model == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			try
			{
				var searchDataContext = new SearchData(_extendedSearchContext);
				var engineList = searchDataContext.UpdateEngine(model);
				if (engineList != null)
				{
					return RedirectToAction("Index");
				}

				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
				
			}
			catch (RetryLimitExceededException)
			{
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
			}
			
			return View(model);
		}

		
		public ActionResult Delete(int? id, bool? saveChangesError = false)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			if (saveChangesError.GetValueOrDefault())
			{
				ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
			}

			var searchDataContext = new SearchData(_extendedSearchContext);
			var engine = searchDataContext.GetEngine(id.Value);
			
			if (engine == null)
			{
				return HttpNotFound();
			}
			return View(engine);
		}

		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id)
		{
			try
			{
				var searchDataContext = new SearchData(_extendedSearchContext);
				var engineList = searchDataContext.DeleteEngine(id);
				return engineList != null ? RedirectToAction("Index") : RedirectToAction("Delete", new { id, saveChangesError = true });
			}
			catch (RetryLimitExceededException)
			{
				return RedirectToAction("Delete", new { id, saveChangesError = true });
			}
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