using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using ParserLibrary;

namespace Bot_App1.Dialogs
{
     
    [Serializable]
    public class RootDialog : IDialog<object>
    {

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
             var parser = new Parser();
            //  var images=await parser.GetSrc();

            var titles = await parser.GetTitles();

            //должен спрашивать у пользователя параметры



            // return our reply to the user


          


            //foreach(var title in parser.Titles)
            // await context.PostAsync(title);
            await context.PostAsync("hi from bot");

            context.Wait(MessageReceivedAsync);
        }
    }
}