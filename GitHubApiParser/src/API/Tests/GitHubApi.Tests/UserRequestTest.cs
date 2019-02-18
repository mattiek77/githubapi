using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GithubApi;
using GithubApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GitHubApi.Tests
{
    [TestClass]
    public class UserRequestTest
    {
        private enum RequestType
        {
            User,
            Repos
        }

        [TestMethod]
        public async Task UserDataSerializationTest()
        {
            var apiClient = new GitHubUserService(new CustomHttpHandler());
            var data = await apiClient.GetGitHubUserInformation("test");
            Assert.AreEqual("Rob Conery", data.Name);
        }

        [TestMethod]
        public async Task RepoDataSerializationTest()
        {
            var user = await GetUserWithRepos();
            Assert.AreEqual(30, user.Repos.Count);
        }

        [TestMethod]
        public async Task MostPopularReposTest()
        {
            var user = await GetUserWithRepos();
            var mostPopularRepos = user.MostPopular(5);
            Assert.AreEqual(5, mostPopularRepos.Count());
            var mostPopular = mostPopularRepos.First();
            Assert.AreEqual(495, mostPopular.StargazersCount);
        }

        private async Task<GitHubUser> GetUserWithRepos()
        {
            var apiClient = new GitHubUserService(new CustomHttpHandler());
            var dummyUser = new GitHubUser
            {
                ReposUrl = "http://test.com/repos"
            };
            var repos = await apiClient.GetUserRepoInformation(dummyUser.ReposUrl);
            dummyUser.Repos = repos;
            return dummyUser;
        }

        private static string GetBodyData(RequestType requestType)
        {
            var filePath = requestType.Equals(RequestType.User) ? "Data/UserData.txt" : "Data/RepoData.txt";
            var filedata = System.IO.File.ReadAllText(filePath);
            return filedata;
        }

       
        private class CustomHttpHandler : HttpMessageHandler
        {
            public virtual HttpResponseMessage Send(HttpRequestMessage request)
            {
                var requestType = request.RequestUri.ToString().EndsWith("/repos")? RequestType.Repos:RequestType.User;
                var response = new HttpResponseMessage();
                response.Content = new StringContent(GetBodyData(requestType));
                return response;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult((Send(request)));
            }
        }
    }
}
