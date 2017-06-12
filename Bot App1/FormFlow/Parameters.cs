using Bot_App1.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bot_App1.FormFlow
{
    [Serializable]
    public class Parameters
    {
        [Prompt("Название города")]
        public string Town { get; set; }

        [Prompt("Количество комнат?")]
        public string Quantity { get; set; }

        [Prompt("Год постройки? От ")]
        public string StartYear { get; set; }

        [Prompt("До")]
        public string EndYear { get; set; }

        public static IForm<Parameters> Build()
        {
            OnCompletionAsyncDelegate<Parameters> processOrder = async (context, state) =>
            {
                await context.Forward(new RootDialog(state), null, state, CancellationToken.None); //call a child dialog
                //await Conversation.SendAsync(activity, () => new RootDialog(state));
            };

            return new FormBuilder<Parameters>()
                .AddRemainingFields()
                .OnCompletion(processOrder)
                .Build();
        }

    }
}