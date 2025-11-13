using System.Text.Json;
using DTOContracts;

namespace BlazorApp.Services;

public class HttpCommentService: ICommentService
{
    private  HttpClient _httpClient;
    public HttpCommentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<CommentDto> AddAsync(RequestCommentDto comment)
    {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("comments", comment);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        )!;
        
    }

    public async Task UpdateAsync(RequestCommentDto comment)
    {
        HttpResponseMessage httpResponse = await _httpClient.PutAsJsonAsync($"comments/{comment.Id}", comment);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {

            throw new Exception(response);
        }
        
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage httpResponse = await _httpClient.DeleteAsync($"comments/{id}");
        if (!httpResponse.IsSuccessStatusCode)
        {
            string response = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception(response);
        }
        
    }

    public async Task<CommentDto> GetSingleAsync(int id)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"comments/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        )!;
    }

    public IQueryable<CommentDto> GetManyAsync()
    {
        HttpResponseMessage httpResponse =  _httpClient.GetAsync("comments").Result;
        string response =  httpResponse.Content.ReadAsStringAsync().Result;
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        List<CommentDto> comments = JsonSerializer.Deserialize<List<CommentDto>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        )!;

        return comments.AsQueryable();
        
    }
}