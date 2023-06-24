using System.Net.Http;
using Newtonsoft.Json;

namespace gpt;

class Program
{
    static async Task Main(string[] args)
    {
        switch (args[0]) {
            case "hoi": {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
                request.Headers.Add("Authorization", "Bearer API_KEY");
                var content = new StringContent("{\n    \"model\": \"gpt-3.5-turbo\",\n    \"messages\": [{\"role\": \"system\", \"content\": \"" + args[1] + "\"}, {\"role\": \"user\", \"content\": \"Hello!\"}]\n  }", null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var data = JsonConvert.DeserializeObject<GptChatResponse>(await response.Content.ReadAsStringAsync());
                Console.WriteLine(data.Choices[0].Message.Content);
                break;
            }

            case "anh": {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/images/generations");
                request.Headers.Add("Authorization", "Bearer API_KEY");
                var content = new StringContent("{\n    \"prompt\": \""+ args[1] +"\",\n    \"n\": 1,\n    \"size\": \"1024x1024\"\n  }", null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                var data = JsonConvert.DeserializeObject<GptImageResponse>(await response.Content.ReadAsStringAsync());
                Console.WriteLine(data.Data[0].Url);
                break;
            }
        }
    }
}

class Message {
    public string Content { get; set; }
}

class Choices {
    public Message Message { get; set; }
}

class GptChatResponse {
    public List<Choices> Choices { get; set; }
}

class Data {
    public string Url { get; set; }
}

class GptImageResponse {
    public List<Data> Data { get; set; }
}