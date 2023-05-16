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
using static System.Net.WebRequestMethods;

namespace CuttingOptimizer.AppLogic.Services
{
    public class ApiService : IApiService
    {
        private HttpClient client = new HttpClient();
        private string url = "https://localhost:7181";

        public ApiService()
        {
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
        }

        public async Task<List<Saw>?> GetAllSaws()
        {
            try
            {
                return await client.GetFromJsonAsync<List<Saw>>(url + "/Saw");

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
                return await client.GetFromJsonAsync<List<Saw>>(url + "/" + search.Trim());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Plate>?> GetAllPlates()
        {
            try
            {
                return await client.GetFromJsonAsync<List<Plate>>(url + "/Plate");

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Blueprint?> SaveBlueprint(Blueprint blueprint)
        {
            try
            {
                HttpResponseMessage httpResponseMessage;

                if (blueprint.ID == 0)
                {
                    httpResponseMessage = await client.PostAsJsonAsync(url + "/Blueprint", blueprint);
                }
                else
                {
                    httpResponseMessage = await client.PutAsJsonAsync(url + "/Blueprint", blueprint);
                }

                Blueprint? result = httpResponseMessage.Content.ReadFromJsonAsync<Blueprint>().Result;

                return httpResponseMessage.StatusCode == HttpStatusCode.OK ? result : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Blueprint?> GetBlueprintById(int id)
        {
            try
            {
                return await client.GetFromJsonAsync<Blueprint>(url + "/Blueprint/id?id=" + id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
