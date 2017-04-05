using System;
using System.Collections.Generic;
using System.Linq;
using NMemory.Linq;

namespace ExtendedSearch.Data
{
	public class SearchData
	{
		private ExtendedSearchEntities _context;

		public SearchData(ExtendedSearchEntities context)
		{
			_context = context;
		}

		public void StoreSearchResult(string searchFor, int engId, string searchResult)
		{
			try
			{
				_context = new ExtendedSearchEntities();

				using (var context = _context)
				{
					var search = new Search
					{
						SearchFor = searchFor,
						Engine = engId,
						SearchResult = searchResult
					};
					
					context.Searches.Add(search);
					context.SaveChanges();
				}
			}
			catch (Exception)
			{
				// ignored
			}
		}
		
		public IEnumerable<Engine> GetSearchEngines()
		{
			using (var context = _context)
			{
				return context.Engines.ToList();
			}
		}

		public IEnumerable<Search> GetSearchResults()
		{
			using (var context = _context)
			{
				return context.Searches.ToList();
			}
		}

		public List<Engine> AddEngine(Engine engine)
		{
			List<Engine> engineList;

			using (var context = _context)
			{
				context.Engines.Add(engine);
				context.SaveChanges();

				engineList = context.Engines.ToList();
			}

			return engineList;
		}

		public Engine GetEngine(int id)
		{
			Engine returnVal;
			_context = new ExtendedSearchEntities();

			using (var context = _context)
			{
				returnVal = context.Engines.Find(id);
			}

			return returnVal;
		}

		public Search GetSearchResultById(int id)
		{
			Search returnVal;
			using (var context = _context)
			{
				returnVal = context.Searches.Find(id);
			}

			return returnVal;
		}

		public Engine GetEngineByName(string name)
		{
			Engine returnVal;
			_context = new ExtendedSearchEntities();
			using (var context = _context)
			{
				returnVal = context.Engines.FirstOrDefault(e => e.EngineName.Equals(name));
			}

			return returnVal;
		}

		public List<Engine> UpdateEngine(Engine engineTo)
		{
			List<Engine> engineList;
			using (var context = _context)
			{
				var engineFrom = context.Engines.Find(engineTo.Id);
				context.Entry(engineFrom).CurrentValues.SetValues(engineTo);
				context.SaveChanges();
				engineList = context.Engines.ToList();
			}
			return engineList;
		}

		public List<Engine> DeleteEngine(int id)
		{
			List<Engine> engineList;
			using (var context = _context)
			{
				var engine = context.Engines.Find(id);
				context.Engines.Remove(engine);
				context.SaveChanges();
				engineList = context.Engines.ToList();
			}

			return engineList;
		}
	}
}