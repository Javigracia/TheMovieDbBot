using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Newtonsoft.Json;
using RestSharp;

namespace TheMovieDbBot.Dialogs
{
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {
        private string APIKey;
        public RootDialog() : base(GetNewService())
        {
            APIKey = ConfigurationManager.AppSettings.Get("APIkey");
        }

        private static ILuisService[] GetNewService()
        {
            var modelId = ConfigurationManager.AppSettings.Get("Luis.ModelId");
            var subscriptionKey = ConfigurationManager.AppSettings.Get("Luis.SubscriptionKey");
            var domain = ConfigurationManager.AppSettings.Get("Luis.Domain");
            var luisModel = new LuisModelAttribute(modelId, subscriptionKey, domain: string.IsNullOrEmpty(domain) ? null : domain);
            return new ILuisService[] { new LuisService(luisModel) };
        }

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            var message = "Lo que se ha escrito entra en none";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("")]
        public async Task UnknownIntent(IDialogContext context, LuisResult result)
        {
            var message = "Lo que se ha escrito entra en none" + string.Join(", ", result.Intents.Select(i => i.Intent));
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Menu")]
        public async Task Menu(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Make a question like: who is the actor of Speed movie?");
            await Task.CompletedTask;
        }

        [LuisIntent("Reset")]
        public async Task Reset(IDialogContext context, LuisResult result)
        {
            context.Call(new HelpDialog(), null);
            await Task.CompletedTask;
        }

        [LuisIntent("MoviesMostPopular")]
        public async Task MoviesMostPopular(IDialogContext context, LuisResult result)
        {
            var client = new RestClient("https://api.themoviedb.org/3/discover/movie?sort_by=popularity.desc&api_key=" + APIKey);
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "b11d1c1b-61b7-0aa7-eaec-0948d45d5a45");
            request.AddHeader("cache-control", "no-cache");
            //IRestResponse response = client.Execute(request);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var movies = JsonConvert.DeserializeObject<Movies>(content);
            await context.PostAsync("The most popular movies at the moment are:");
            foreach (var moviesResult in movies.results)
            {
                context.PostAsync(moviesResult.title);
            }
            await Task.CompletedTask;
        }

        [LuisIntent("Actores")]
        public async Task Actores(IDialogContext context, LuisResult result)
        {
            var actor = result.Entities.FirstOrDefault(recommendation => recommendation.Type == "Actor").Entity;
            var client = new RestClient("https://api.themoviedb.org/3/search/person?api_key=" + APIKey + "&query=" + actor);
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "b11d1c1b-61b7-0aa7-eaec-0948d45d5a45");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var tempActor = JsonConvert.DeserializeObject<Actor>(content);
            client = new RestClient("https://api.themoviedb.org/3/discover/movie?api_key=" + APIKey + "&with_cast=" + tempActor.results.FirstOrDefault().id + "&sort_by=vote_average.desc");
            request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "b11d1c1b-61b7-0aa7-eaec-0948d45d5a45");
            request.AddHeader("cache-control", "no-cache");
            response = client.Execute(request);
            content = response.Content;
            var tempActorMovies = JsonConvert.DeserializeObject<ActorMovies>(content);
            await context.PostAsync("The most popular movies of this actor are:");
            foreach (var y in tempActorMovies.results)
            {
                context.PostAsync(y.title);
            }
            await Task.CompletedTask;
        }
        private async Task Resume(IDialogContext context, IAwaitable<object> result)
        {
            await Menu(context, null);
        }
    }
}