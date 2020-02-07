# TrueLayerPokemon
This is Umberto Bar's response to the Engineering Challenge for the *"Senior Software Engineer - Italy Team"* position at **TrueLayer**.

##Notes on design
The API will respond with HTTP 200 if:
1. The PokeAPI service is reachable
1. The threshold of 100 requests per minute provided by the PokeAPI service has not been reached
1. The requested pokemon is known by the PokeAPI service
1. A flavor text in English language exists for the requested Pokemon
1. The funtranslations service is reachable
1. The threshold of 5 requests per hour provided by the funtranslations service has not been reached

The API will respond with HTTP 404 if:
1. The PokeAPI service is reachable
1. The threshold of 100 requests per minute provided by the PokeAPI service has not been reached
1. The requested pokemon is **not known** by the PokeAPI service

The API will respond with HTTP 429 if:
1. The PokeAPI service is reachable but threshold of 100 requests per minute has been reached
OR
1. The funtranslations service is reachable but the threshold of 5 requests per minute has been reached

**In case of any other error, the API will respond with the same HTTP code received as response by the PokeAPI or the funtranslations service.**

## Technology stack
This is Visual Studio 2019 solution containing:
* __TrueLayerPokemon__: an ASP.NET WebApi project to deliver the required GET API
* __TrueLayerPokemonTests__: a MSTest project to provide automated Unit Tests
Both projects are built using .NET Framework v.4.7.2

##Pre-Requisites to execute the solution:
The following software should be installed on your computer:
1. Visual Studio 2019 with WebApi development modules (including IISExpress)
1. .NET Framework 4.7.2
1. Docker Desktop for Windows (tested with v.2.2.0.0 - 42247)	

Please note that **all the external libraries referenced by the two projects are already included in this Git repository (installed via NuGet Package Manager).**

##How to run the solution in debug mode
1. Open TrueLayerPokemon.sln with Visual Studio
1. Start the debugger. The project will run on port 44370
1. You can execute queries from your browser. E.g. https://localhost:44370/api/pokemon/ditto

##Unit Tests
The solution includes simple Unit tests to verify HTTP 200 and HTTP 404 responses.
Please note that the API provided by this project relies on the funtranslations free API service, which allows a maximum of 5 requests per hour from the same IP. Depending on this, the Unit Test for HTTP 200 scenario will fail after this limit is reached.

##Docker
A docker container for the project is available here:
https://registry.hub.docker.com/r/ponchmaster/truelayerpokemon