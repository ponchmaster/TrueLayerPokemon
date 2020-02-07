using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace TrueLayerPokemon.Controllers
{

    public class PokemonController : ApiController
    {
        private const string pokeApiEndPoint = "https://pokeapi.co/api/v2/pokemon-species/";
        private const string shakespeareTranslatorEndPoint = "https://api.funtranslations.com/translate/shakespeare";

        public IHttpActionResult Get(string id)
        {
            if (GetPokeApiPayload(id) is var pokeApiResponse && pokeApiResponse.result)
            {
                if (GetShakespeareDescription(pokeApiResponse.content) is var shakespeareResponse && shakespeareResponse.result)
                {
                    var content = new GetResponseBody
                    {
                        name = id,
                        description = shakespeareResponse.content
                    };
                    return Ok(content);
                }
                else
                {
                    return Content(shakespeareResponse.statusCode, shakespeareResponse.content);
                }
            }
            return Content(pokeApiResponse.statusCode, pokeApiResponse.content);
        }

        private (bool result, string content, HttpStatusCode statusCode) GetShakespeareDescription(string description)
        {
            bool result = false;
            string content = string.Empty;
            HttpStatusCode statusCode;
            using (HttpClient c = new HttpClient())
            {
                var payload = new Dictionary<string, string>
                {
                    { "text", description }
                };
                var response = c.PostAsync(shakespeareTranslatorEndPoint, new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")).Result;
                statusCode = response.StatusCode;
                var responsePayload = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (responsePayload.TryGetValue("success", out JToken _))
                    {
                        result = true;
                        content = responsePayload["contents"]["translated"].ToString();
                    }
                }
                else
                {
                    content = responsePayload["error"]["message"].ToString();
                }
            }
            return (result, content, statusCode);
        }

        private (bool result, string content, HttpStatusCode statusCode) GetPokeApiPayload(string pokemonName)
        {
            bool result = false;
            string content = null;
            HttpStatusCode statusCode;

            using (HttpClient c = new HttpClient())
            {
                var response = c.GetAsync($"{pokeApiEndPoint}{pokemonName}").Result;
                statusCode = response.StatusCode;
                content = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responsePayload = JObject.Parse(content);
                    if (responsePayload.TryGetValue("flavor_text_entries", out JToken flavorTextEntries))
                    {
                        content = flavorTextEntries.Where(w => w["language"]["name"].ToString() == "en").Select(s => s["flavor_text"].ToString()).FirstOrDefault();
                        if (!string.IsNullOrEmpty(content))
                        {
                            result = true;
                        }
                    }
                }
            }
            return (result, content, statusCode);
        }
    }

    public class GetResponseBody
    {
        public string name { get; set; }
        public string description { get; set; }
    }
}
