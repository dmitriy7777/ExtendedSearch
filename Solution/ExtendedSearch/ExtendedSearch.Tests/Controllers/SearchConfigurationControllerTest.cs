using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web.Mvc;
using Effort;
using ExtendedSearch.Controllers;
using ExtendedSearch.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendedSearch.Tests.Controllers
{
	[TestClass]
	public class SearchConfigurationControllerTest
	{
		[TestMethod]
		public void IndexTest()
		{
			// Arrange
			var controller = new SearchConfigurationController();

			// Act
			ViewResult result = controller.Index(string.Empty, string.Empty, string.Empty, 0);

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void DetailsTest()
		{
			// Arrange
			var connection = EntityConnectionFactory.CreateTransient("name=ExtendedSearchEntities");
			var entities = new ExtendedSearchEntities(connection);

			// Arrange
			var controller = new SearchConfigurationController(entities);

			// Act
			var engine = new Engine
			{
				EngineName = "EngineName1",
				Url = "EngineName1",
				Id = 0
			};

			var result = controller.Details(engine.Id);

			// Assert
			Assert.IsNotNull(result);


		}

		[TestMethod]
		public void CreateTest()
		{
			// Arrange
			var connection = EntityConnectionFactory.CreateTransient("name=ExtendedSearchEntities");
			var entities = new ExtendedSearchEntities(connection);

			// Arrange
			var controller = new SearchConfigurationController(entities);

			// Act
			var engine = new Engine
			{
				EngineName = "EngineName1",
				Url = "EngineName1"
			};

			var result = controller.Create(engine);

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void EditTest()
		{

			// Arrange
			var connection = EntityConnectionFactory.CreateTransient("name=ExtendedSearchEntities");
			var entities = new ExtendedSearchEntities(connection);

			// Arrange
			var controller = new SearchConfigurationController(entities);

			// Act
			var result = controller.Edit(0);

			// Assert
			Assert.AreNotEqual(new HttpStatusCodeResult(HttpStatusCode.BadRequest), result);
		}

		[TestMethod]
		public void EditPostTest()
		{
			// Arrange
			var connection = EntityConnectionFactory.CreateTransient("name=ExtendedSearchEntities");
			var entities = new ExtendedSearchEntities(connection);

			// Arrange
			var controller = new SearchConfigurationController(entities);

			// Act
			var engine = new Engine {EngineName = "EngineName", Url = "Url", Id = 1};
			entities.Engines.Add(engine);
			var result = controller.EditPost(engine);

			// Assert
			Assert.AreNotEqual(new HttpStatusCodeResult(HttpStatusCode.BadRequest), result);
			
		}

		[TestMethod]
		public void DeleteGetTest()
		{
			// Arrange
			var connection = EntityConnectionFactory.CreateTransient("name=ExtendedSearchEntities");
			var entities = new ExtendedSearchEntities(connection);

			// Arrange
			var controller = new SearchConfigurationController(entities);

			// Act
			var engine = new Engine { EngineName = "EngineName", Url = "Url", Id = 1 };
			entities.Engines.Add(engine);
			var result = controller.Delete(engine.Id, null);

			// Assert
			Assert.AreNotEqual(new HttpStatusCodeResult(HttpStatusCode.BadRequest), result);

		}

		[TestMethod]
		public void DeletePostTest()
		{
			// Arrange
			var connection = EntityConnectionFactory.CreateTransient("name=ExtendedSearchEntities");
			var entities = new ExtendedSearchEntities(connection);

			// Arrange
			var controller = new SearchConfigurationController(entities);

			// Act
			var engine = new Engine { EngineName = "EngineName", Url = "Url", Id = 1 };
			entities.Engines.Add(engine);
			var result = controller.Delete(engine.Id);

			// Assert
			Assert.AreNotEqual(new RetryLimitExceededException(), result);
		}
	}
}

