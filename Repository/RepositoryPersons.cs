using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ApiDog.Dto;
using ApiDog.Services.Interfaces;

namespace DesktopApp.Repository
{
    public class RepositoryPersons : IRepositoryPerson
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RepositoryPersons(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<PersonDto>? Get(string? searchValue, int? customerId, int? eventId)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Person?searchValue={searchValue}&customerId={customerId}&eventId={eventId}");
            
            var result = Services.GetData<List<PersonDto>>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }

        public async Task<List<PersonDto>?> GetAsync(string? searchValue, int? customerId, int? eventId)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Person?searchValue={searchValue}&customerId={customerId}&eventId={eventId}");
            
            var result = await Services.GetDataAsync<List<PersonDto>>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }

        public int Add(PersonDto person)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Person");
            var json = JsonSerializer.Serialize(person);
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Post, requestUri, json);

            return result;
        }

        public async Task<int> AddAsync(PersonDto person)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Person");
            var json = JsonSerializer.Serialize(person);
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Post, requestUri, json);

            return result;
        }

        public int Update(PersonDto person)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Person");
            var json = JsonSerializer.Serialize(person);
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Put, requestUri, json);
            
            return result;
        }

        public async Task<int> UpdateAsync(PersonDto person)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Person");
            var json = JsonSerializer.Serialize(person);
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Put, requestUri, json);
            
            return result;
        }

        public int Delete(int id)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Person?id={id}");
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Delete, requestUri);

            return result;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Person?id={id}");
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Delete, requestUri);

            return result;
        }

        public int CountPersonWithEvent(int eventId)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Person/countPersonWithEvent?idEvent={eventId}");
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Get, requestUri);

            return result;
        }

        public async Task<int> CountPersonWithEventAsync(int eventId)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Person/countPersonWithEvent?idEvent={eventId}");
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Get, requestUri);

            return result;
        }

        public int GetCountPersonForCustomer(int customerId)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Person/countPersonWithCustomer?idCustomer={customerId}");
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Get, requestUri);

            return result;
        }

        public async Task<int> GetCountPersonForCustomerAsync(int customerId)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Person/countPersonWithCustomer?idCustomer={customerId}");
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Get, requestUri);

            return result;
        }

        public int EventSet(int personId, int eventId)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Person/SetEventId?personId={personId}&eventId={eventId}");
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Post, requestUri);

            return result;
        }

        public async Task<int> EventSetAsync(int personId, int eventId)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Person/SetEventId?personId={personId}&eventId={eventId}");
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Post, requestUri);

            return result;
        }
    }
}