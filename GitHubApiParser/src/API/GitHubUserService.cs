using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GithubApi.Interfaces;
using GithubApi.Models;
using Newtonsoft.Json;

[assembly:InternalsVisibleTo("GitHubApi.Tests")]
namespace GithubApi
{
    public class GitHubUserService : IGitHubUserService
    {
        private static readonly string BaseUrl = "https://api.github.com/users/{0}";
        private readonly HttpClient _client;

        public GitHubUserService(HttpMessageHandler handler = null)
        {
            _client = new HttpClient(handler??new HttpClientHandler());
            _client.DefaultRequestHeaders.Add("User-Agent", "Api-Browser");
        }

        public async Task<GitHubUser> GetGitHubUserInformation(string userName)
        {
            var userRequestUrl = string.Format(BaseUrl, userName);
            var responseMessage = await _client.GetAsync(userRequestUrl);
            var responseBody = await responseMessage.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<GitHubUser>(responseBody);
            return user;
        }

        public async Task<List<GitHubRepo>> GetUserRepoInformation(string repoUrl, List<GitHubRepo> existingGitHubRepos = null)
        {
            if(existingGitHubRepos == null)
                existingGitHubRepos = new List<GitHubRepo>();

            var repoRequestUrl = repoUrl;
            var responseMessage = await _client.GetAsync(repoRequestUrl);
            var responseBody = await responseMessage.Content.ReadAsStringAsync();
            List<GitHubRepo> repos;
            try
            {
                repos = JsonConvert.DeserializeObject<List<GitHubRepo>>(responseBody);
            }
            catch
            {
                repos = new List<GitHubRepo>();
            }
            existingGitHubRepos.AddRange(repos);
            var next = GetNextRepoUrl(responseMessage);
            if (next != null)
                existingGitHubRepos = await GetUserRepoInformation(next, existingGitHubRepos);
            return existingGitHubRepos;
        }

        private string GetNextRepoUrl(HttpResponseMessage responseMessage)
        {
            if (responseMessage.Headers == null || !responseMessage.Headers.Contains("Link"))
                return null;

            var linkHeaders = responseMessage.Headers.GetValues("Link").ToList();
            if (linkHeaders== null || !linkHeaders.Any())
                return null;

            var navLinks = linkHeaders.First().Split(',');

            var nextLink = navLinks.FirstOrDefault(link => link.Contains("rel=\"next\""));

            if (string.IsNullOrWhiteSpace(nextLink))
                return null;

            var navLinkUrl = nextLink.Split(';')[0].Replace("<", string.Empty).Replace(">", string.Empty);

            return navLinkUrl;
        }
    }
}
