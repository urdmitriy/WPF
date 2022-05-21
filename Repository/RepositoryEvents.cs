using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ApiDog.Dto;
using ApiDog.Services.Interfaces;

namespace DesktopApp.Repository
{
    public class RepositoryEvents : IRepositoryEvent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RepositoryEvents(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<EventDto>? Get(string searchValue)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Event?searchValue={searchValue}");
            
            var result = Services.GetData<List<EventDto>>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }

        public async Task<List<EventDto>?> GetAsync(string searchValue)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Event?searchValue={searchValue}");
            
            var result = await Services.GetDataAsync<List<EventDto>>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }

        public int Add(EventDto eventName)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Event");
            var json = JsonSerializer.Serialize(eventName);
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Post, requestUri, json);

            return result;
        }

        public async Task<int> AddAsync(EventDto eventName)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Event");
            var json = JsonSerializer.Serialize(eventName);
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Post, requestUri, json);

            return result;
        }

        public EventDto? GetNameById(int id)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Contract/name?id={id}");
            
            var result = Services.GetData<EventDto>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }

        public async Task<EventDto?> GetNameByIdAsync(int id)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Contract/name?id={id}");
            
            var result = await Services.GetDataAsync<EventDto>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }

        public int GetIdByName(string? name)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Event/GetIdByName?name={name}");
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }

        public async Task<int> GetIdByNameAsync(string? name)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Event/GetIdByName?name={name}");
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }

        public int Update(EventDto eventName)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Event");
            var json = JsonSerializer.Serialize(eventName);
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Put, requestUri, json);
            
            return result;
        }

        public async Task<int> UpdateAsync(EventDto eventName)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Event");
            var json = JsonSerializer.Serialize(eventName);
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Put, requestUri, json);
            
            return result;
        }

        public int Delete(int id)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Event?id={id}");
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Delete, requestUri);

            return result;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Event?id={id}");
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Delete, requestUri);

            return result;
        }

        public int HideEvent(int idEvent)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Event/setFlagHidden?idEvent={idEvent}");
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Post, requestUri);

            return result;
        }

        public async Task<int> HideEventAsync(int idEvent)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Event/setFlagHidden?idEvent={idEvent}");
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Post, requestUri);

            return result;
        }

        public int ShowEvent(int idEvent)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Event/resetFlagHidden?idEvent={idEvent}");
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Post, requestUri);

            return result;
        }

        public async Task<int> ShowEventAsync(int idEvent)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Event/resetFlagHidden?idEvent={idEvent}");
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Post, requestUri);

            return result;
        }
    }
}