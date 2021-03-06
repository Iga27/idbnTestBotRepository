﻿using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Bot_App1.Service;
using System.Collections.Generic;
using Bot_App1.FormFlow;
using System.Linq;
using System.Threading;

namespace Bot_App1.Dialogs
{
     
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        //Parameters formParameters;
        static TownCodeLoader townLoader;
        public static string Url { get; set; } = "https://realt.by/sale/flats/search/";
        static string phraseForParsing;

        static RootDialog()
        {
            townLoader = new TownCodeLoader(Url);
            townLoader.LoadTowns();
        }

        /*public RootDialog(Parameters formParameters)
        {
            this.formParameters = formParameters;
        }*/


        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            phraseForParsing = activity.Text;   
            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();

            var isCorrect = ShowHeroCard(reply);
            if (reply.Attachments.Count != 0 && isCorrect)   
                await context.PostAsync(reply);
            else 
                await context.PostAsync("ничего не найдено");

            //context.Done(this);
            context.Wait(MessageReceivedAsync);
        }


         


          private bool ShowHeroCard(IMessageActivity reply)
        {
             
                var parser = new Parser(Url);
                //formParameters.Town = townLoader.CodeDictionary[formParameters.Town];

                string text = parser.Load(GetAllParameters(phraseForParsing));
                var flats = parser.Parse(text);

                //var array = new CardAction[] { new CardAction() { } };

                foreach (var flat in flats)
                {
                    var imageAction = new CardAction()
                    {
                        Value = flat.Link,
                        Type = "openUrl",
                    };
                    var cardImages = new CardImage[] { new CardImage(url: flat.ImageSrc, tap: imageAction) };

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


        public static string GetBetween(string source, string startString, string endString) //public for tests
        {
            int start, end;
            if (source.Contains(startString) && source.Contains(endString))
            {
                start = source.IndexOf(startString, 0) + startString.Length;
                end = source.IndexOf(endString, start);
                return source.Substring(start, end - start);
            }
            else
            {
                return "";
            }
        }

        public static FlatParameters GetAllParameters(string text)
        {
            //город Брест, 1-комнатная квартира, год постройки не позднее 1996 г и не дороже 900$ за кв.м. 
            var parameters = new FlatParameters();
            string town = GetBetween(text, "город ", ",");
            if (townLoader.CodeDictionary.ContainsKey(town))
            {
                parameters.Town = townLoader.CodeDictionary[town];
            }
            else
            {
                parameters.Town = townLoader.CodeDictionary["Минск"];
            }
            //parameters.Town = townLoader.CodeDictionary[town];
            parameters.Quantity = GetBetween(text, ", ", "-комнатная");
            parameters.StartYear = GetBetween(text, "не позднее ", " г");
            parameters.Price = GetBetween(text, "не дороже ", "$");
            return parameters;
        }
    }
}