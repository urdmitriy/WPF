using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using ApiDog.Dto;
using ApiDog.Services.Interfaces;
using DesktopApp.Forms;

namespace DesktopApp.Repository
{
    public class RepositoryContracts : IRepositoryContract

    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RepositoryContracts(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public int Add(ContractDto name)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Contract");
            var json = JsonSerializer.Serialize(name);
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Post, requestUri, json);

            return result;
        }

        public async Task<int> AddAsync(ContractDto name)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Contract");
            var json = JsonSerializer.Serialize(name);
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Post, requestUri, json);

            return result;
        }

        public List<ContractDto>? Get(string searchValue, DateTime fromDate, DateTime toDate, bool notPayed, 
            bool notSigned, bool notDocuments, bool notAct, bool percentIsTrue)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Contract?searchValue={searchValue}&" +
                                     $"fromDate={fromDate:yyyy-MM-dd}&toDate={toDate:yyyy-MM-dd}&notPayed={notPayed}&" +
                                     $"notSigned={notSigned}&notDocuments={notDocuments}&notAct={notAct}" +
                                     $"&percentIsTrue={percentIsTrue}");
            
            var result = Services.GetData<List<ContractDto>>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }

        public async Task<List<ContractDto>?> GetAsync(string searchValue, DateTime fromDate, DateTime toDate, bool notPayed, bool notSigned, bool notDocuments,
            bool notAct, bool percentIsTrue)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Contract?searchValue={searchValue}&" +
                                     $"fromDate={fromDate:yyyy-MM-dd}&toDate={toDate:yyyy-MM-dd}&notPayed={notPayed}&" +
                                     $"notSigned={notSigned}&notDocuments={notDocuments}&notAct={notAct}" +
                                     $"&percentIsTrue={percentIsTrue}");
            
            var result = await Services.GetDataAsync<List<ContractDto>>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }

        public ContractDto? GetById(int id)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Contract/name?id={id}");
            
            var result = Services.GetData<ContractDto>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }
        public async Task<ContractDto?> GetByIdAsync(int id)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Contract/name?id={id}");
            
            var result = await Services.GetDataAsync<ContractDto>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }

        public int GetIdByName(string name)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Contract/id?name={name}");
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }

        public async Task<int> GetIdByNameAsync(string name)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Contract/id?name={name}");
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Get, requestUri);
            
            return result;
        }

        public int Update(ContractDto updateObject)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Contract");
            var json = JsonSerializer.Serialize(updateObject);
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Put, requestUri, json);
            
            return result;
        }

        public async Task<int> UpdateAsync(ContractDto updateObject)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Contract");
            var json = JsonSerializer.Serialize(updateObject);
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Put, requestUri, json);
            
            return result;
        }

        public int Delete(int idDelete)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Contract?idDelete={idDelete}");
            
            var result = Services.GetData<int>(_httpClientFactory, HttpMethod.Delete, requestUri);

            return result;
        }

        public async Task<int> DeleteAsync(int idDelete)
        {
            var requestUri = new Uri($"http://{Services.AddressApi}/Contract?idDelete={idDelete}");
            
            var result = await Services.GetDataAsync<int>(_httpClientFactory, HttpMethod.Delete, requestUri);

            return result;
        }

        public int GetCountContractForCustomerId(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountContractForCustomerIdAsync(int customerId)
        {
            throw new NotImplementedException();
        }
    }
}