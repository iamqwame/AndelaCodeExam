using AndelaCodeExam.Models;
using AndelaCodeExam.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndelaCodeExam.Tests
{
	public class EmailCampaignServiceTest
	{
		private List<Event> _events;
		private EmailCampaignService _emailCampaignService;


		[SetUp]
		public void Setup()
		{
			_events = new List<Event>
			{
				new Event { Name = "Phantom of the Opera", City = "New York" },
				new Event { Name = "Metallica", City = "Los Angeles" },
				new Event { Name = "Metallica", City = "New York" },
				new Event { Name = "Metallica", City = "Boston" },
				new Event { Name = "LadyGaGa", City = "New York" },
				new Event { Name = "LadyGaGa", City = "Boston" },
				new Event { Name = "LadyGaGa", City = "Chicago" },
				new Event { Name = "LadyGaGa", City = "San Francisco" },
				new Event { Name = "LadyGaGa", City = "Washington" }
			};
			_emailCampaignService = new EmailCampaignService();
		}


		[Test]
		public async Task GetCustomerCityEvents_Should_Return_Data()
		{
			var customer = new Customer { Name = "Mr. Fake", City = "New York" };
			var events = (await _emailCampaignService.GetCustomerCityEvents(customer, _events)).ToList();

			Assert.IsTrue(events.Count > 0);
		}

		[Test]
		public async Task GetNearestEvents_Should_Return_Data()
		{
			var customer = new Customer { Name = "Mr. Fake", City = "New York" };
			var events = (await _emailCampaignService.GetNearestEvents(customer, _events)).ToList();

			Assert.IsTrue(events.Count > 0);
		}
	}
}