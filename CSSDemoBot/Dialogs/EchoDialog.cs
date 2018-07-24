using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using System.Collections.Generic;


namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        protected int count = 1;
        
        private Dictionary<string, string> converstationDict = new Dictionary<string, string>();
        
        public EchoDialog()
        {
            InitConversationTable();
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            
            var replyMessage = ProcessMeassgeUsingDict(message.Text);
          
            if (replyMessage.StartsWith("Is this"))
            {
                PromptDialog.Confirm(
                    context,
                    AfterResetAsync,
                    replyMessage,
                    "Sorry, did not get that.",
                    promptStyle: PromptStyle.Auto);
            }
            else
            {
                await context.PostAsync(replyMessage);
                context.Wait(MessageReceivedAsync);
            }
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                this.count = 1;
                await context.PostAsync("Here is the Segmentation counts from each filter.\n" +
                    "Segmentation size: 563\n" +
                    "Sent to ICCA: 563\n" +
                    "Received ICCS: 563\n" +
                    "User Profile Found: 563\n" +
                    "Sent to ICP: 563\n" +
                    "ICP Opt Out: 559\n" +
                    "Sent to SFMC: 4");
            }
            else
            {
                await context.PostAsync("Sorry, I cannot locate your information.");
            }
            context.Wait(MessageReceivedAsync);
        }

        private string ProcessMessage(string message)
        {
            if (String.IsNullOrWhiteSpace(message)) 
            {
                return "please enter a non-empty message .";
            }
            
            message = message.ToLower().Trim();
                        
            if (message.Contains("segmentation"))
            {
                return "do you have a question related to the segmentation data ?";
            }
            
            return "sorry, but I don't understand your question. please try again .";
        }
        
        private void InitConversationTable()
        {
            converstationDict.Add("question 1", "Hi, Jane, I am Iris Support Bot. How can I help you?");
            converstationDict.Add("question 2", "There is the document for the segmentation configuration: \n" +  
                "https://en.wikipedia.org/wiki/Market_segmentation \n");
            converstationDict.Add("question 3", "I see you have a question about segmentation. What is your Iris Studio URL?");
            converstationDict.Add("question 4", "What is your interaction link");
            converstationDict.Add("question 5", "Is this the campaign you are asking about? [Jane's Campagin 2018 ...]");
            converstationDict.Add("question 6", "ICP is CPM (Customer Permission Management). Here is the definition of ICP: www.icpdefinition.com");
            converstationDict.Add("question 7", "Hope I answered your question. Thanks for using Iris Support Service.");
        }
        
        private string ProcessMeassgeUsingDict(string message)
        {
            if (String.IsNullOrWhiteSpace(message)) 
            {
                return "please enter a non-empty message .";
            }
            
            message = message.ToLower().Trim();
                        
            if (message.Contains("hi"))
            {
                return converstationDict["question 1"];
            }
            
            if (message.Contains("segmentation"))
            {
                return converstationDict["question 2"];
            }
            
            if (message.Contains("audience"))
            {
                return converstationDict["question 3"];
            }
            
            if (message.Contains("irisstudio"))
            {
                return converstationDict["question 4"];
            }
            
            if (message.Contains("interaction"))
            {
                return converstationDict["question 5"];
            }
            
            if (message.Contains("opt out"))
            {
                return converstationDict["question 6"];
            }
            
            if (message.Contains("got it"))
            {
                return converstationDict["question 7"];
            }
            
            return "sorry, but I don't understand your question. please try again .";            
        }

        private IMessageActivity ProcessMessageWithRichResponse(string message)
        {
            IMessageActivity responseMessage =  Activity.CreateMessageActivity();
            responseMessage.Text = "";
            if (String.IsNullOrWhiteSpace(message)) 
            {
                responseMessage.Text = "please enter a non-empty message .";
                return responseMessage;
            }

            message = message.ToLower().Trim();
                
            if (message.Contains("campaign"))
            {
                ProcessCampaignQuestion(message, ref responseMessage);
                return responseMessage;
            }
                
            if (message.Contains("segmentation"))
            {
                ProcessCampaignQuestion(message, ref responseMessage);
                return responseMessage;
            }

            responseMessage.Text = "sorry, but I don't understand your question. please try again.";
            return responseMessage;
        }     
        
        private void ProcessCampaignQuestion(string message, ref IMessageActivity response) 
        {
            response.Text = "Here is a wiki document for campaign related questions: ";
            
            //response.Attachments.Add(new Attachment()
            //{
            //    ContentUrl = "https://en.wikipedia.org/wiki/Campaign",
            //    ContentType = "text/html",      
            //});           
        }   
        
        private void ProcessSegmentationQuestion(string message, ref IMessageActivity response)
        {
            response.Text = "Here is a wiki document for segmentation related questions: ";
            
            //response.Attachments.Add(new Attachment()
            //{
            //    ContentUrl = "https://en.wikipedia.org/wiki/Market_segmentation",
            //    ContentType = "text/html",      
            //});             
        }
        
    }
}