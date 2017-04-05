using System.Web.Mvc;
using Effort;
using ExtendedSearch.Controllers;
using ExtendedSearch.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendedSearch.Tests.Controllers
{
	[TestClass]
	public class SearchResultsControllerTest
	{
		[TestMethod]
		public void IndexTest()
		{
			// Arrange
			var controller = new SearchResultsController();

			// Act
			var result = controller.Index(string.Empty, string.Empty, string.Empty, 1) as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void DetailsTest()
		{
			
			// Arrange
			var connection = EntityConnectionFactory.CreateTransient("name=ExtendedSearchEntities");
			var entities = new ExtendedSearchEntities(connection);
			var controller = new SearchResultsController(entities);

			entities.Searches.Add(new Search { Engine = 1, Id = 1, SearchFor = "SearchFor1", SearchResult = "SearchResult1" });
			entities.SaveChanges();
			
			// Act
			var result = controller.Details(1) as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}
	}
}

