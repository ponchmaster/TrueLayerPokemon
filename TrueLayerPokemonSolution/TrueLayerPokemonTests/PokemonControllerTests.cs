using System;
using System.Net;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrueLayerPokemon.Controllers;

namespace TrueLayerPokemonTests
{
    [TestClass]
    public class PokemonControllerTests
    {
        [TestMethod]
        public void GetOkTests()
        {
            using (var controller = new PokemonController())
            {
                string pokemonName = "charizard";
                var response = controller.Get(pokemonName) as OkNegotiatedContentResult<GetResponseBody>;
                Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<GetResponseBody>));
                Assert.AreEqual(pokemonName, response.Content.name);
                Assert.IsTrue(!string.IsNullOrEmpty(response.Content.description));
            }
        }

        [TestMethod]
        public void GetNotFoundTests()
        {
            using (var controller = new PokemonController())
            {
                string pokemonName = "some_non_existing_pokemon";
                var response = controller.Get(pokemonName) as NegotiatedContentResult<string>;
                Assert.IsInstanceOfType(response, typeof(NegotiatedContentResult<string>));
                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }
}
