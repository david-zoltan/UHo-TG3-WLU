using Octokit;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Get GitHub token from environment
            var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Error: GITHUB_TOKEN environment variable is not set");
                Environment.Exit(1);
            }

            // Get repository and PR number from environment
            var repository = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY");
            var prNumber = Environment.GetEnvironmentVariable("PR_NUMBER");

            if (string.IsNullOrEmpty(repository) || string.IsNullOrEmpty(prNumber))
            {
                Console.WriteLine("Error: GITHUB_REPOSITORY or PR_NUMBER environment variables are not set");
                Environment.Exit(1);
            }

            var parts = repository.Split('/');
            if (parts.Length != 2)
            {
                Console.WriteLine("Error: Invalid repository format");
                Environment.Exit(1);
            }

            var owner = parts[0];
            var repo = parts[1];

            // Initialize GitHub client
            var client = new GitHubClient(new ProductHeaderValue("PrintPullRequest"))
            {
                Credentials = new Credentials(token)
            };

            // Get pull request details
            var pullRequest = await client.PullRequest.Get(owner, repo, int.Parse(prNumber));
            
            Console.WriteLine("=== Pull Request Information ===");
            Console.WriteLine($"Title: {pullRequest.Title}");
            Console.WriteLine($"Number: {pullRequest.Number}");
            Console.WriteLine($"State: {pullRequest.State}");
            Console.WriteLine($"Created At: {pullRequest.CreatedAt}");
            Console.WriteLine($"Updated At: {pullRequest.UpdatedAt}");
            Console.WriteLine($"Author: {pullRequest.User.Login}");
            Console.WriteLine($"Base Branch: {pullRequest.Base.Ref}");
            Console.WriteLine($"Head Branch: {pullRequest.Head.Ref}");
            Console.WriteLine($"Body: {pullRequest.Body}");
            Console.WriteLine($"Commits: {pullRequest.Commits}");
            Console.WriteLine($"Changed Files: {pullRequest.ChangedFiles}");
            Console.WriteLine($"Additions: {pullRequest.Additions}");
            Console.WriteLine($"Deletions: {pullRequest.Deletions}");

            // Get pull request files
            var files = await client.PullRequest.Files(owner, repo, int.Parse(prNumber));
            
            Console.WriteLine("\n=== Changed Files ===");
            foreach (var file in files)
            {
                Console.WriteLine($"\nFile: {file.FileName}");
                Console.WriteLine($"Status: {file.Status}");
                Console.WriteLine($"Additions: {file.Additions}");
                Console.WriteLine($"Deletions: {file.Deletions}");
                Console.WriteLine($"Changes: {file.Changes}");
                
                // Get the diff content using the raw diff URL
                var diffUrl = $"https://api.github.com/repos/{owner}/{repo}/pulls/{prNumber}/files/{file.FileName}";
                var diffResponse = await client.Connection.Get<object>(new Uri(diffUrl), new Dictionary<string, string>());
                var diffContent = diffResponse.Body.ToString();
                
                Console.WriteLine("\nDiff:");
                Console.WriteLine(diffContent);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Environment.Exit(1);
        }
    }
} 