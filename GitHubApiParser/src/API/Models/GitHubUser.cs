using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace GithubApi.Models
{
    public class GitHubUser
    {

        [JsonProperty("name")]
        public string Name { get; internal set; }
        [JsonProperty("login")]
        public string Login { get; internal set; }
        [JsonProperty("location")]
        public string Location { get; internal set; }
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; internal set; }
        [JsonProperty("repos_url")]
        public string ReposUrl { get; internal set; }
        public IList<GitHubRepo> Repos { get; set; }

        public IEnumerable<GitHubRepo> MostPopular(int amount = 5)
        {
            if (Repos == null)
                return null;

            return Repos.OrderByDescending(repo => repo.StargazersCount).Take(amount);
        }
    }
}
