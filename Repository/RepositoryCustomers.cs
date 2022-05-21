using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using ApiDog.Dto;
using ApiDog.Services.Interfaces;

namespace DesktopApp.Repository
{
    public class RepositoryCustomers : IRepositoryCustomer
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private int CountCustomers { get; set; } = 0;

        public RepositoryCustomers(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public int Add(CustomerDto name)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Customer");
            var json = JsonSerializer.Serialize(name);
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Post, requestUri, json);

            return result;
        }
        public async Task<int> AddAsync(CustomerDto name)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Customer");
            var json = JsonSerializer.Serialize(name);
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Post, requestUri, json);

            return result;
        }

        public List<CustomerDto>? Get(string searchValue, DateTime fromDate, DateTime toDate, int withCount)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Customer?searchValue={searchValue}&withCount=1");
            switch (withCount)
            {
                case 1:
                    requestUri = new Uri($"http://{Services.AddressApi}/Customer?searchValue={searchValue}&withCount=1");
                    break;
                case 0:
                    requestUri = new Uri($"http://{Services.AddressApi}/Customer?searchValue={searchValue}&withCount=0");
                    break;
            }
            
            var result = Services.GetData<List<CustomerDto>>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }

        public async Task<List<CustomerDto>?> GetAsync(string searchValue, DateTime fromDate, DateTime toDate, int withCount)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Customer?searchValue={searchValue}&withCount=1");
            switch (withCount)
            {
                case 1:
                    requestUri = new Uri($"http://{Services.AddressApi}/Customer?searchValue={searchValue}&withCount=1");
                    break;
                case 0:
                    requestUri = new Uri($"http://{Services.AddressApi}/Customer?searchValue={searchValue}&withCount=0");
                    break;
            }

            var result = await Services.GetDataAsync<List<CustomerDto>>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }

        public CustomerDto? GetById(int id)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Customer/name?id={id}");
            
            var result = Services.GetData<CustomerDto>(_httpClientFactory, HttpMethod.Get, requestUri);

            return result;
        }
        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Customer/name?id={id}");
            
            var result = await Services.GetDataAsync<CustomerDto>(_httpClientFactory, HttpMethod.Get, requestUri);

            return result;
        }

        public int GetIdByName(string name)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Customer/id?name={name}");
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Get, requestUri);

            return result;
        }
        public async Task<int> GetIdByNameAsync(string name)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Customer/id?name={name}");
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Get, requestUri);

            return result;
        }
        public int Update(CustomerDto updateObject)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Customer");
            var json = JsonSerializer.Serialize(updateObject);
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Put, requestUri, json);
            
            return result;
        }
        public async Task<int> UpdateAsync(CustomerDto updateObject)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Customer");
            var json = JsonSerializer.Serialize(updateObject);
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Put, requestUri, json);
            
            return result;
        }

        public int Delete(int idDelete)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Customer?idDelete={idDelete}");
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Delete, requestUri);

            return result;
        }
        public async Task<int> DeleteAsync(int idDelete)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Customer?idDelete={idDelete}");
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Delete, requestUri);

            return result;
        }
    }
}