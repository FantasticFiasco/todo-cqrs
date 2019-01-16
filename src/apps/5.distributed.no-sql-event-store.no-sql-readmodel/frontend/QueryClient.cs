using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReadModel;

namespace Frontend
{
    public class QueryClient
    {
        private readonly HttpClient httpClient;

        public QueryClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<TodoItem[]> GetAllAsync()
        {
            var response = await httpClient.GetAsync("/todo-items");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TodoItem[]>(content);
        }

        public async Task<TodoItem> GetAsync(Guid id)
        {
            var response = await httpClient.GetAsync($"/todo-items/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TodoItem>(content);
        }
    }
}
