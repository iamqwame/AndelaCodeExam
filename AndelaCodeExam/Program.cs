using AndelaCodeExam;
using AndelaCodeExam.Models;
using AndelaCodeExam.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndelaCodeExam
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var events = new List<Event>{
					new Event{ Name = "Phantom of the Opera", City = "New York"},
					new Event{ Name = "Metallica", City = "Los Angeles"},
					new Event{ Name = "Metallica", City = "New York"},
					new Event{ Name = "Metallica", City = "Boston"},
					new Event{ Name = "LadyGaGa", City = "New York"},
					new Event{ Name = "LadyGaGa", City = "Boston"},
					new Event{ Name = "LadyGaGa", City = "Chicago"},
					new Event{ Name = "LadyGaGa", City = "San Francisco"},
					new Event{ Name = "LadyGaGa", City = "Washington"}
					};

			var customer = new Customer { Name = "John Smith", City = "New York" };



			var emailService = new EmailCampaignService();

			//1.1
			var customerEvents = await emailService.GetCustomerCityEvents(customer,events);


			//1.2
			await emailService.SendEmail(customer, customerEvents);



			//1.3 we will send all event to John smith through email


			//1.4 Yes



			//2.1
			var nearestEvents =await emailService.GetNearestEvents(customer,events,5);


			//2.2
			await emailService.SendEmail(customer, nearestEvents);




			//2.3 John smith will recieve all the five closest event in his email


			//2.4 Yes


			//3. Implemented in method GetDistanceWrapper inside EmailCampaignService


			//4. Implemented in method GetDistanceWrapper inside EmailCampaignService

			//5.
			var mrFakeClosestEvents = emailService.GetNearestEvents(customer, events).GetAwaiter().GetResult().ToList();
			emailService.SendEmail(customer, mrFakeClosestEvents, 200).GetAwaiter().GetResult();



		}
	}
}
