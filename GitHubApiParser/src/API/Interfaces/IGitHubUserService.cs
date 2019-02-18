using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GithubApi.Models;

namespace GithubApi.Interfaces
{
    public interface IGitHubUserService
    {

        Task<GitHubUser> GetGitHubUserInformation(string userName);
        Task<List<GitHubRepo>> GetUserRepoInformation(string repoUrl, List<GitHubRepo> existingGitHubRepos = null);
    }
}
