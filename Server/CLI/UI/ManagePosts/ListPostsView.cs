using RepositoryContracts;

namespace CLI.UI.ManagePosts;
using InMemoryRepositories;
public class ListPostsView
{
    private readonly IPostRepository postRepository;
    public ListPostsView( IPostRepository postRepository)
    {
        this.postRepository= postRepository;
    }

    public void ListPosts()
    {
        postRepository.GetManyAsync().ToList().ForEach(p=>
            Console.WriteLine($"Title: {p.Title} Body: {p.Body} Post Id: {p.Id}"));
    }

    
    
}