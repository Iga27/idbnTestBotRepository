using Microsoft.Bot.Builder.FormFlow;
using System;

namespace Bot_App1.FormFlow
{
    [Serializable]
    public class Parameters
    {
        public string TownCode { get; set; }

        public string Quantity { get; set; }

        public string StartYear { get; set; }

        public string EndYear { get; set; }

        public static IForm<Parameters> BuildEnquiryForm()
        {
            return new FormBuilder<Parameters>()
                .AddRemainingFields()
                .Build();
        }
    }
}