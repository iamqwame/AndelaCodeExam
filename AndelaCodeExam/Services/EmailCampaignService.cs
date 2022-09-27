using AndelaCodeExam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndelaCodeExam.Services
{
	public sealed class EmailCampaignService
	{
		private readonly Dictionary<string, int> _distanceDictionary = new Dictionary<string, int>();

		/// <summary>
		/// 1.1
		/// </summary>
		/// <param name="customer"></param>
		/// <param name="upComingEvents"></param>
		/// <returns></returns>
		public Task<IEnumerable<Event>> GetCustomerCityEvents(Customer customer, IEnumerable<Event> upComingEvents)
		{
			var events = upComingEvents.Where(e => e.City == customer.City);
			return Task.FromResult(events);
		}

		/// <summary>
		/// 1.2
		/// </summary>
		/// <param name="customer"></param>
		/// <param name="upComingCustomerEvents"></param>
		/// <param name="defaultPrice"></param>
		/// <returns></returns>
		public async Task SendEmail(Customer customer, IEnumerable<Event> upComingCustomerEvents, int? defaultPrice = null)
		{
			foreach (var upComingCustomerEvent in upComingCustomerEvents)
			{
				var price = GetPrice(upComingCustomerEvent);
				if (defaultPrice.HasValue)
				{
					// todo: hard coded as an example, we can explore expression trees
					if (price <= defaultPrice.Value)
					{
						AddToEmail(customer, upComingCustomerEvent, price).GetAwaiter().GetResult();
					}
				}
				else
				{
					AddToEmail(customer, upComingCustomerEvent, price).GetAwaiter().GetResult();
				}
			}

			await Task.CompletedTask;

		}


		/// <summary>
		/////2.1
		/// </summary>
		/// <param name="customer"></param>
		/// <param name="upComingEvents"></param>
		/// <param name="numberOfEvents"></param>
		/// <returns></returns>
		public Task<IEnumerable<Event>> GetNearestEvents(Customer customer, IEnumerable<Event> upComingEvents, int numberOfEvents = 5)
		{
			var nearestEvents = upComingEvents.OrderBy(x => GetDistanceWrapper(customer.City, x.City)).Take(numberOfEvents);
			return Task.FromResult(nearestEvents);
		}



		/// <summary>
		/// 3 and 4
		/// </summary>
		/// <param name="fromCity"></param>
		/// <param name="toCity"></param>
		/// <returns></returns>
		private int GetDistanceWrapper(string fromCity, string toCity)
		{
			int retryCount = 0;
		retryPoint:

			try
			{

				var key = $"{fromCity}-{toCity}";
				var alreadyComputed = _distanceDictionary.TryGetValue(key, out int distance);
				if (!alreadyComputed)
				{
					distance = GetDistance(fromCity, toCity);
					_distanceDictionary[key] = distance;
				}

				return distance;
			}
			catch (Exception)
			{
				if (retryCount <= 3)
					return GetDistanceWrapper(fromCity, toCity);
				else
					goto retryPoint;
			}

		}



		private Task AddToEmail(Customer c, Event e, int? price = null)
		{
			var distance = GetDistanceWrapper(c.City, e.City);
			Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}" + (distance > 0 ? $" ({distance} miles away)" : "")
																	+ (price.HasValue ? $" for ${price}" : ""));
			return Task.CompletedTask;
		}
		private int GetPrice(Event e)
		{
			return (AlphebiticalDistance(e.City, "") + AlphebiticalDistance(e.Name, "")) / 10;
		}



		private int GetDistance(string fromCity, string toCity)
		{
			return AlphebiticalDistance(fromCity, toCity);

		}

		private int AlphebiticalDistance(string s, string t)
		{
			var result = 0;
			var i = 0;
			for (i = 0; i < Math.Min(s.Length, t.Length); i++)
			{
				// Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
				result += Math.Abs(s[i] - t[i]);
			}
			for (; i < Math.Max(s.Length, t.Length); i++)
			{
				// Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
				result += s.Length > t.Length ? s[i] : t[i];
			}
			return result;
		}

	}
}
