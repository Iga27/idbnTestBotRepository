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
        [Prompt("Введите название города")]
        public string Town { get; set; }

        [Prompt("Количество комнат?")]
        public string Quantity { get; set; }

        [Prompt("Постройка не позднее какого года?")]
        public string StartYear { get; set; }

        [Prompt("Цена $ за кв.м. Не дороже: ")]
        public string Price { get; set; }

        public static IForm<Parameters> Build()
        {
            OnCompletionAsyncDelegate<Parameters> processOrder = async (context, state) =>
            {
                 await context.Forward(new RootDialog(state), onChildDialogCompleted, state, CancellationToken.None); //call a child dialog
                 
            };

            return new FormBuilder<Parameters>()
                .AddRemainingFields()
                .OnCompletion(processOrder)
                .Build();
        }

        private static async Task onChildDialogCompleted(IDialogContext context, IAwaitable<object> result)
        {
            var value = await result;
        }

    }
}