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
            try
            {
                return await client.GetFromJsonAsync<List<Saw>>("https://localhost:44397/saw");

            }
            catch (Exception ex)
            {
               return null;
            }
        }
        public async Task<List<Saw>?> SearchSaws(string search)
        {
            try
            {
                return await client.GetFromJsonAsync<List<Saw>>("https://localhost:44397/saw/search/" + search.Trim());
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
