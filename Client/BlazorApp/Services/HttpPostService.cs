using System.Text.Json;
using DTOContracts;

namespace BlazorApp.Services;

public class HttpPostService: IPostService
{
    private readonly HttpClient client;
 
     public HttpPostService(HttpClient client)
     {
         this.client = client;
     }
     
    public async  Task<PostDto> AddAsync(RequestPostDto postDto)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("posts", postDto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        )!;
    }

    public async Task UpdateAsync(RequestPostDto postDto)
    {
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"posts/{postDto.Id}", postDto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {

            throw new Exception(response);
        }
    }

    public async Task DeleteAsync(int postId)
    {
        HttpResponseMessage httpResponse = await client.DeleteAsync($"posts/{postId}");
        if (!httpResponse.IsSuccessStatusCode)
        {
            string response = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception(response);
        }
        
    }

    public async Task<PostDto> GetSingleAsync(int postId)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"posts/{postId}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        )!;
        
    }

    public IQueryable<PostDto> GetManyAsync()
    {
        HttpResponseMessage httpResponse =  client.GetAsync("posts").Result;
        string response =  httpResponse.Content.ReadAsStringAsync().Result;
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        var posts = JsonSerializer.Deserialize<List<PostDto>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        )!;

        return posts.AsQueryable();
        
    }
}