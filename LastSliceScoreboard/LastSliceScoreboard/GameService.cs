using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LastSliceScoreboard
{
    public class GameService
    {
        private const string CHALLENGE_ROOT_URL = "https://thelastslice.azurewebsites.net/";

        private const string SCORE_BOARD_URL = CHALLENGE_ROOT_URL + "api/ScoreBoard";

        private HttpClient client;

        public bool IsSuccessStatusCode { get; private set; }

        public GameService()
        {
            string tenant = "thelastslice.onmicrosoft.com";

            string clientId = "17315b0a-1d61-41c1-a614-aaa908fc6c3c";

            string webApiId = "webapi";

            string webApiResourceId = string.Format("https://{0}/{1}", tenant, webApiId);

            string signUpSignInPolicy = "B2C_1_SignUp_SignIn";

            string webApiScope = "default_scope";

            string[] scopes = new string[] { string.Format("https://{0}/{1}/{2}", tenant, webApiId, webApiScope) };

            client = new HttpClient(tenant, clientId, webApiResourceId, scopes, signUpSignInPolicy);
        }

        #region Login Methods

        public void ClearUserCache()
        {
            client.ClearCache();
        }

        public bool HasUserLoggedIn()
        {
            return client.CachedUserExists();
        }

        public Task<string> LoginAsync()
        {
            return client.GetAccessTokenAsync();
        }

        public void Logout()
        {
            client.Logout();
        }

        #endregion

        #region Service Methods

        public async Task<List<Score>> GetLeaderboardWithCurrentUserAsync()
        {
            HttpResponseMessage httpResponse = await client.GetUrlAsync(SCORE_BOARD_URL + "?maxItemCount=1000");

            var content = httpResponse.Content;

            string result = await content.ReadAsStringAsync();

            var scores = JsonConvert.DeserializeObject<Rootobject>(result);

            return new List<Score>(scores.Scores);
        }

        public class Rootobject
        {
            public Score[] Scores { get; set; }
        }

        #endregion
    }
}
