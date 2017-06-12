using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using ParserLibrary;
using System.Collections.Generic;
using System.Linq;
using Bot_App1.FormFlow;
using Microsoft.Bot.Builder.FormFlow;

namespace Bot_App1.Dialogs
{
     
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        static TownCodeLoader townLoader;
        //static string town;
        public static ISettings Settings { get; set; }
        static FlatParameters parameters; //static?

        public Task StartAsync(IDialogContext context)
        {
            parameters = new FlatParameters();
            Settings = new Settings("https://realt.by/sale/flats/search/");
            townLoader = new TownCodeLoader(Settings);
            townLoader.LoadTowns();
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var message = activity.Text;
          
            await context.PostAsync("Введите город");

            context.Wait(TownReceivedAsync);
        }

        public async Task TownReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            //town = activity.Text;
            parameters.TownCode = townLoader.CodeDictionary[activity.Text];

            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();

            var isCorrect=ShowHeroCard(reply);

            if(reply.Attachments.Count!=0 && isCorrect)
            await context.PostAsync(reply);
            else
            await context.PostAsync("ничего не найдено");

            await context.PostAsync("Введите город");
            context.Wait(TownReceivedAsync);

        }

        private static bool ShowHeroCard(IMessageActivity reply)
        {
            try
            {
                var parser = new Parser(Settings);
                //string text=parser.Load(townLoader.CodeDictionary[flatParameters.Town]);
                string text = parser.Load( ); //parameters
                var flats = parser.Parse(text);

                var array = new CardAction[] { new CardAction() { } };
               
                foreach (var f in flats)
                {
                    var imageAction = new CardAction()
                    {
                        Value = f.Link,
                        Type = "openUrl",
                    };
                    var cardImages = new CardImage[] { new CardImage(url: f.ImageSrc,tap:imageAction) };

                    var card = new HeroCard()
                    {
                        Title = f.Title,
                        Images = cardImages,
                    };

                    var cardAttachment = card.ToAttachment();
                    reply.Attachments.Add(cardAttachment);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

          

        }
}