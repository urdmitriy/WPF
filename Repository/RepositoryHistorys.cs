using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ApiDog.Dto;
using ApiDog.Services.Interfaces;

namespace DesktopApp.Repository
{
    public class RepositoryHistorys : IRepositoryHistory
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RepositoryHistorys(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<HistoryPersonEventDto>? Get(int personId)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/History?idPerson={personId}");
            
            var result = Services.GetData<List<HistoryPersonEventDto>>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }

        public async Task<List<HistoryPersonEventDto>?> GetAsync(int personId)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/History?idPerson={personId}");
            
            var result = await Services.GetDataAsync<List<HistoryPersonEventDto>>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }

        public int Add(HistoryPersonEventDto historyPersonEvent)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/History");
            var json = JsonSerializer.Serialize(historyPersonEvent);
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Post, requestUri, json);

            return result;
        }

        public async Task<int> AddAsync(HistoryPersonEventDto historyPersonEvent)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/History");
            var json = JsonSerializer.Serialize(historyPersonEvent);
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Post, requestUri, json);

            return result;
        }
    }
}