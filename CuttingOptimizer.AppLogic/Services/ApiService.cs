using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Services
{
    public class ApiService : IApiService
    {
        private HttpClient client = new HttpClient();

        public ApiService()
        {
            
        }

        public async Task<List<Saw>?> GetAllSaws()
        {
            return await client.GetFromJsonAsync<List<Saw>>("https://localhost:44397/saw");
        }
        public async Task<List<Saw>?> SearchSaws(string search)
        {
        //   if(search == "")
        //    {
        //        return await client.GetFromJsonAsync<List<Saw>>("https://localhost:44397/saw");
        //    }
        //    else
        //    {
            if(search != "") return await client.GetFromJsonAsync<List<Saw>>("https://localhost:44397/saw/search/" + search.Trim());
            return null;
        }
    }
}
