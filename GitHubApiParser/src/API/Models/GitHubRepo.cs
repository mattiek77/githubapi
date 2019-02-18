using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GithubApi.Models
{
    public class GitHubRepo
    {
        [JsonProperty("name")]
        public string Name { get; internal set; }
        [JsonProperty("full_name")]
        public string FullName { get; internal set; }
        [JsonProperty("stargazers_count")]
        public int StargazersCount { get; internal set; }
    }
}
