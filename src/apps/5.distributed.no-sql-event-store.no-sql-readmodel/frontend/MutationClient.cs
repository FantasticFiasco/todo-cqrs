using System.Net.Http;
using System.Threading.Tasks;
using Todo;

namespace Frontend
{
    public class MutationClient
    {
        private readonly HttpClient httpClient;

        public MutationClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task HandleAddAsync(AddTodo command)
        {
            var response = await httpClient.PostAsJsonAsync("/add", command);
            response.EnsureSuccessStatusCode();
        }

        public async Task HandleRenameAsync(RenameTodo command)
        {
            var response = await httpClient.PostAsJsonAsync("/rename", command);
            response.EnsureSuccessStatusCode();
        }

        public async Task HandleCompleteAsync(CompleteTodo command)
        {
            var response = await httpClient.PostAsJsonAsync("/complete", command);
            response.EnsureSuccessStatusCode();
        }

        public async Task HandleIncompleteAsync(IncompleteTodo command)
        {
            var response = await httpClient.PostAsJsonAsync("/incomplete", command);
            response.EnsureSuccessStatusCode();
        }

        public async Task HandleRemoveAsync(RemoveTodo command)
        {
            var response = await httpClient.PostAsJsonAsync("/remove", command);
            response.EnsureSuccessStatusCode();
        }
    }
}
