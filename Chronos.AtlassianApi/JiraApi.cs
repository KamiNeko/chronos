using Chronos.AtlassianApi.Dto.Jira;
using Chronos.AtlassianApi.Dto.Tempo;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Chronos.AtlassianApi
{
    public class JiraApi
    {
        public JiraApi(string username, string jiraUrlPrefix, string tempoBearerToken, string jiraBearerToken)
        {
            this.username = username ?? throw new ArgumentNullException(nameof(username));
            this.jiraUrlPrefix = jiraUrlPrefix ?? throw new ArgumentNullException(nameof(jiraUrlPrefix));
            this.tempoBearerToken = tempoBearerToken ?? throw new ArgumentNullException(nameof(tempoBearerToken));
            this.jiraBearerToken = jiraBearerToken ?? throw new ArgumentNullException(nameof(jiraBearerToken));
            tempoRestClient = CreateTempoClient();
            jiraRestClient = CreateJiraClient();
        }

        public async Task<MyselfQueryRoot> GetMyself()
        {
            var request = new RestRequest("myself");
            return await jiraRestClient.GetAsync<MyselfQueryRoot>(request).ConfigureAwait(false);
        }

        public async Task<TeamsRoot> GetTeams()
        {
            var request = new RestRequest("teams");
            return await tempoRestClient.GetAsync<TeamsRoot>(request).ConfigureAwait(false);
        }

        public async Task<TeamMembersRoot> GetTeamMembers(int teamId)
        {
            var request = new RestRequest($"teams/{teamId}/members");
            return await tempoRestClient.GetAsync<TeamMembersRoot>(request).ConfigureAwait(false);
        }

        public async Task<UsersRoot[]> GetUsers()
        {
            var request = new RestRequest($"users/search");
            return await jiraRestClient.GetAsync<UsersRoot[]>(request).ConfigureAwait(false);
        }

        public AccountsRoot GetAccounts()
        {
            var request = new RestRequest($"accounts");
            var response = tempoRestClient.Get<AccountsRoot>(request);
            return response.Data;
        }

        public string GetAccountKeyByAccountId(int accountId)
        {
            var accounts = GetAccounts();
            var account = accounts.Results.First(x => x.Id == accountId);
            return account.Key;
        }

        public WorkLogsRoot GetWorkLogs(DateTime from, DateTime to)
        {
            string fromDateText = from.ToString("yyyy-MM-dd");
            string toDateText = to.ToString("yyyy-MM-dd");

            var request = new RestRequest($"worklogs?from={fromDateText}&to={toDateText}");
            var response = tempoRestClient.Get<WorkLogsRoot>(request);
            return response.Data;
        }

        public WorkLogItemCreateResultRoot CreateWorklog(WorkLogItemCreate workLogItemCreate)
        {
            var request = new RestRequest("worklogs", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };

            request.AddJsonBody(workLogItemCreate);
            var response = tempoRestClient.Execute<WorkLogItemCreateResultRoot>(request);
            return response.Data;
        }

        public async Task<IssueRoot> GetJiraIssue(string issueName)
        {
            var request = new RestRequest($"issue/{issueName}");
            return await jiraRestClient.GetAsync<IssueRoot>(request).ConfigureAwait(false);
        }

        private RestClient CreateTempoClient()
        {
            const string url = "https://api.tempo.io/core/3";
            var client = new RestClient(url);
            client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", tempoBearerToken));
            client.UseNewtonsoftJson();
            return client;
        }

        private RestClient CreateJiraClient()
        {
            string url = $"https://{jiraUrlPrefix}.atlassian.net/rest/api/3/";
            var client = new RestClient(url);
            client.Authenticator = new HttpBasicAuthenticator(username, jiraBearerToken);
            client.UseNewtonsoftJson();
            return client;
        }

        public static async Task<bool> TestJiraConnection(string jiraUrlPrefix, string email, string jiraToken)
        {
            try
            {
                string url = $"https://{jiraUrlPrefix}.atlassian.net/rest/api/3/";
                var client = new RestClient(url);
                client.Authenticator = new HttpBasicAuthenticator(email, jiraToken);
                client.UseNewtonsoftJson();

                var request = new RestRequest($"myself");
                var response = await client.ExecuteGetAsync(request);
                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> TestTempoConnection(string tempoToken)
        {
            try
            {
                const string url = "https://api.tempo.io/core/3";
                var client = new RestClient(url);
                client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", tempoToken));
                client.UseNewtonsoftJson();

                var request = new RestRequest($"worklogs");
                var response = await client.ExecuteGetAsync(request);
                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        private readonly string username;
        private readonly string tempoBearerToken;
        private readonly string jiraBearerToken;
        private readonly string jiraUrlPrefix;

        private readonly RestClient tempoRestClient;
        private readonly RestClient jiraRestClient;
    }
}