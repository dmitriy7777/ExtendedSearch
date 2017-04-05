using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using Effort;
using ExtendedSearch.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ExtendedSearch.Tests.Data
{
	
	[TestClass]
	public class SearchDataTest
	{

		[TestMethod]
		public void StoreSearchResultTest()
		{
			// Arrange
			var connection = EntityConnectionFactory.CreateTransient("name=ExtendedSearchEntities");
			var entities = new ExtendedSearchEntities(connection);

			entities.Engines.Add(new Engine {EngineName = "EngineName10", Id=1, Url = "Url" });
			entities.SaveChanges();

			var searchDataEntities = new SearchData(entities);

			// Act
			var searchEngines = searchDataEntities.GetSearchEngines();

			// Assert
			Assert.AreEqual("EngineName10", searchEngines.FirstOrDefault().EngineName);
		}

		[TestMethod]
		public void GetSearchEnginesTest()
		{
			// Arrange
			var connection = EntityConnectionFactory.CreateTransient("name=ExtendedSearchEntities");
			var entities = new ExtendedSearchEntities(connection);

			entities.Engines.Add(new Engine { EngineName = "EngineName", Id = 1, Url = "Url" });
			entities.Engines.Add(new Engine { EngineName = "EngineName1", Id = 2, Url = "Url" });
			entities.Engines.Add(new Engine { EngineName = "EngineName2", Id = 3, Url = "Url" });
			entities.SaveChanges();

			var searchDataEntities = new SearchData(entities);

			// Act
			var searchEngines = searchDataEntities.GetSearchEngines();

			// Assert
			Assert.AreEqual(3, searchEngines.Count());
		}

		[TestMethod]
		public void GetSearchResultsTest()
		{
			// Arrange
			var connection = EntityConnectionFactory.CreateTransient("name=ExtendedSearchEntities");
			var entities = new ExtendedSearchEntities(connection);

			entities.Searches.Add(new Search { Engine = 1, Id = 1, SearchFor = "SearchFor1", SearchResult = "SearchResult1" });
			entities.Searches.Add(new Search { Engine = 2, Id = 2, SearchFor = "SearchFor2", SearchResult = "SearchResult2" });
			entities.Searches.Add(new Search { Engine = 3, Id = 3, SearchFor = "SearchFor3", SearchResult = "SearchResult3" });
			entities.SaveChanges();

			var searchDataEntities = new SearchData(entities);

			// Act
			var searchResults = searchDataEntities.GetSearchResults();

			// Assert
			Assert.AreEqual(3, searchResults.Count());
		}

		[TestMethod]
		public void AddEngineTest()
		{
			// Arrange
			var connection = EntityConnectionFactory.CreateTransient("name=ExtendedSearchEntities");
			var entities = new ExtendedSearchEntities(connection);

			var searchDataEntities = new SearchData(entities);

			// Act
			var listOfEngines = searchDataEntities.AddEngine(new Engine { EngineName = "EngineName10", Id = 1, Url = "Url" });
			
			Assert.IsNotNull(listOfEngines);
			Assert.AreEqual(1, listOfEngines.Count);
		}

		[TestMethod]
		public void GetEngineTest()
		{
			// Arrange
			var connection = EntityConnectionFactory.CreateTransient("name=ExtendedSearchEntities");
			var entities = new ExtendedSearchEntities(connection);

			var searchDataEntities = new SearchData(entities);

			// Act
			var listOfEngines = searchDataEntities.AddEngine(new Engine { EngineName = "EngineName10", Id = 1, Url = "Url" });
			var getlistOfEngines = searchDataEntities.GetEngine(1);

			Assert.IsNotNull(listOfEngines);
			Assert.IsNotNull(getlistOfEngines);
			Assert.AreEqual(1, getlistOfEngines.Id);
		}

		[TestMethod]
		public void GetSearchResultByIdTest()
		{
			// Arrange
			var connection = EntityConnectionFactory.CreateTransient("name=ExtendedSearchEntities");
			var entities = new ExtendedSearchEntities(connection);

			entities.Searches.Add(new Search { Engine = 1, Id = 1, SearchFor = "SearchFor1", SearchResult = "SearchResult1" });
			entities.Searches.Add(new Search { Engine = 2, Id = 2, SearchFor = "SearchFor2", SearchResult = "SearchResult2" });
			entities.Searches.Add(new Search { Engine = 3, Id = 3, SearchFor = "SearchFor3", SearchResult = "SearchResult3" });
			entities.SaveChanges();

			var searchDataEntities = new SearchData(entities);

			// Act
			var searchResults = searchDataEntities.GetSearchResultById(1);

			// Assert
			Assert.AreEqual(1, searchResults.Id);
			Assert.AreEqual("SearchFor1", searchResults.SearchFor);
			Assert.AreEqual("SearchResult1", searchResults.SearchResult);
		}

		[TestMethod]
		public void GetEngineByNameTest()
		{

			// Arrange
			var connection = EntityConnectionFactory.CreateTransient("name=ExtendedSearchEntities");
			var entities = new ExtendedSearchEntities(connection);

			var searchDataEntities = new SearchData(entities);

			// Act
			var listOfEngines = searchDataEntities.AddEngine(new Engine { EngineName = "EngineName10", Id = 1, Url = "Url" });
			var searchEngines = listOfEngines.FirstOrDefault(se => se.EngineName.Equals("EngineName10"));

			Assert.IsNotNull(searchEngines);
			Assert.AreEqual("EngineName10", searchEngines.EngineName);
		}

		[TestMethod]
		public void UpdateEngineTest()
		{
			// Arrange
			var connection = EntityConnectionFactory.CreateTransient("name=ExtendedSearchEntities");
			var entities = new ExtendedSearchEntities(connection);

			var searchDataEntities = new SearchData(entities);
			entities.Engines.Add(new Engine { EngineName = "EngineName10", Id = 1, Url = "Url" });
			entities.SaveChanges();

			// Act
			var enginrTo = new Engine {EngineName = "EngineName11", Id = 1, Url = "Url11"};
			var listOfEngines = searchDataEntities.UpdateEngine(enginrTo);
			var searchEngines = listOfEngines.FirstOrDefault(se => se.EngineName.Equals("EngineName11"));

			Assert.IsNotNull(searchEngines);
			Assert.AreEqual("EngineName11", searchEngines.EngineName);
			Assert.AreEqual("Url11", searchEngines.Url);
		}

		[TestMethod]
		public void DeleteEngineTest()
		{
			// Arrange
			var connection = EntityConnectionFactory.CreateTransient("name=ExtendedSearchEntities");
			var entities = new ExtendedSearchEntities(connection);

			var searchDataEntities = new SearchData(entities);
			entities.Engines.Add(new Engine { EngineName = "EngineName10", Id = 1, Url = "Url" });
			entities.SaveChanges();

			// Act
			var listOfEngines = searchDataEntities.DeleteEngine(1);

			Assert.AreEqual(0, listOfEngines.Count);
		}
	}
}

