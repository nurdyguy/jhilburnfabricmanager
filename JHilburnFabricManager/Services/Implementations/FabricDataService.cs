using JHilburnFabricManager.Models;
using JHilburnFabricManager.Services.Contracts;
using JHilburnFabricManager.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JHilburnFabricManager.Services.Implementations
{
    public class FabricDataService : IFabricDataService
    {

        private readonly ApiProperties _apiProperties;

        public FabricDataService(JFMOptions options)
        {
            _apiProperties = options.FabricApi;
        }
        
        //[GET] /fabrics/:id - Get a fabric by id
        public async Task<Fabric> Get(int fabricId)
        {
            var fab = await ApiClient.GetById<Fabric>($"{_apiProperties.BaseUrl}/fabrics/{fabricId}", _apiProperties.Tokens);
            return fab;
        }

        //[GET] /fabrics - Get a list of all fabrics -- paged response
        public async Task<ApiPagedResponse<Fabric>> Get(int page, int perPage, string search, string sortBy, string sortDir)
        {
            var queryStr = BuildQueryString(search, sortBy, sortDir, page, perPage);
            var fabs = await ApiClient.Get<Fabric>($"{_apiProperties.BaseUrl}/fabrics{queryStr}", _apiProperties.Tokens, page, perPage);
            return fabs;
        }

        //[POST] /fabrics - Create a new fabric
        public async Task<Fabric> Create(Fabric fabric)
        {
            var fab = await ApiClient.Post<Fabric>($"{_apiProperties.BaseUrl}/fabrics", fabric, _apiProperties.Tokens);
            return fab;
        }

        //[PUT] /fabrics/:id - Update fabric record
        public async Task<Fabric> Update(Fabric fabric)
        {
            var fab = await ApiClient.Put<Fabric>($"{_apiProperties.BaseUrl}/fabrics/{fabric.id}", fabric, _apiProperties.Tokens);
            return fab;
        }

        //[PATCH] /fabrics/:id - Update specifc fields of a fabric record
        public async Task<bool> Patch(Fabric fabric)
        {
            await ApiClient.Patch<Fabric>($"{_apiProperties.BaseUrl}/fabrics/{fabric.id}", fabric, _apiProperties.Tokens);
            return true;
        }

        //[DELETE] /fabrics/:id - Delete a fabric
        public async Task<bool> Delete(int fabricId)
        {
            await ApiClient.Delete<Fabric>($"{_apiProperties.BaseUrl}/fabrics/{fabricId}", _apiProperties.Tokens);
            return true;
        }

        private string BuildQueryString(string search, string sortBy, string sortDir, int page, int perPage)
        {
            // search
            var searchQS = string.Empty;
            if(!string.IsNullOrWhiteSpace(search))
            {
                searchQS = $"{search}&";



            }
            
            // sort
            if (string.IsNullOrWhiteSpace(sortBy))
                sortBy = "id";
            if (string.IsNullOrWhiteSpace(sortDir))
                sortDir = "asc";

            var sortQS = $"_sort={sortBy}&_order={sortDir}&";

            // page
            if(page <= 0)
                page = 1;
            if(perPage <= 0)            
                perPage = 10;
            
            var pageQS = $"_page={page}&_limit={perPage}";
            
            var qs = $"?{searchQS}{sortQS}{pageQS}";
            return qs;
        }
    }
}
