using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Bot_App1.Service;
using System.Collections.Generic;
using Bot_App1.FormFlow;

namespace Bot_App1.Dialogs
{
     
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        Parameters formParameters;
        static TownCodeLoader townLoader;
        public static ISettings SettingsProperty { get; set; }

        static RootDialog()
        {
            SettingsProperty = new Settings("https://realt.by/sale/flats/search/");
            townLoader = new TownCodeLoader(SettingsProperty);
            townLoader.LoadTowns();
        }

        public RootDialog(Parameters formParameters)
        {
            this.formParameters = formParameters;
        }


        public Task StartAsync(IDialogContext context)  
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

         private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();

            var isCorrect = ShowHeroCard(reply);

            if (reply.Attachments.Count != 0 && isCorrect)
                await context.PostAsync(reply);
            else
                await context.PostAsync("ничего не найдено");

            context.Done(this);
        } 

        /*public async Task TownReceivedAsync(IDialogContext context, IAwaitable<object> result)
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

        }*/

        private bool ShowHeroCard(IMessageActivity reply) 
        {
            try
            {
                var parser = new Parser(SettingsProperty);
                formParameters.Town = townLoader.CodeDictionary[formParameters.Town];
                string text = parser.Load(formParameters); 
                var flats = parser.Parse(text);

                var array = new CardAction[] { new CardAction() { } };
               
                foreach (var flat in flats)
                {
                    var imageAction = new CardAction()
                    {
                        Value = flat.Link,
                        Type = "openUrl",
                    };
                    var cardImages = new CardImage[] { new CardImage(url: flat.ImageSrc,tap:imageAction) };

                    var card = new HeroCard()
                    {
                        Title = flat.Title,
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