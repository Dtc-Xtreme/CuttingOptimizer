using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Services
{
    public class ApiService : IApiService
    {
        private HttpClient client = new HttpClient();

        public ApiService()
        {
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
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

        public async Task<Quotation?> SaveQuotation(Quotation quotation)
        {
            try
            {
                HttpResponseMessage httpResponseMessage;
                Quotation? result = null;

                if (quotation.ID == 0)
                {
                    httpResponseMessage = await client.PostAsJsonAsync("https://localhost:44397/Quotation", quotation);
                    result = httpResponseMessage.Content.ReadFromJsonAsync<Quotation>().Result;
                }
                else
                {
                    httpResponseMessage = await client.PutAsJsonAsync("https://localhost:44397/Quotation", quotation);
                    var xasd = httpResponseMessage.Content.ReadAsStringAsync();
                }

                return httpResponseMessage.StatusCode == HttpStatusCode.OK ? result : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Quotation?> GetQuotationById(int id)
        {
            try
            {
                return await client.GetFromJsonAsync<Quotation>("https://localhost:44397/Quotation/id?id=" + id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
