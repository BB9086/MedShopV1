using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using MedShop.Models;
using Microsoft.AspNetCore.Identity;

namespace MedShop.Hubs
{
    public class MyHub:Hub
    {
        private readonly IChatMessageRepository chatMessageRepository;
        private readonly UserManager<ApplicationUser> userManager;

        private readonly HttpContext httpContext;

        public static Dictionary<string, List<string>> dictionaryOfConnections = new Dictionary<string, List<string>>();

        public static Dictionary<string, List<string>> dictionaryOfLoggedConnections = new Dictionary<string, List<string>>();

        public static Dictionary<string, int> dictionaryOfNumberOfOrdersInShoppingCart = new Dictionary<string, int>();

        public static Dictionary<string, List<string>> dictionaryOfListOfProductsInShoppingCart = new Dictionary<string, List<string>>();

        public static Dictionary<string, string> dictionaryOfSumOfAllProductsInShoppingCart = new Dictionary<string, string>();
        
        public MyHub(IHttpContextAccessor httpContextAccessor, IChatMessageRepository chatMessageRepository, UserManager<ApplicationUser> userManager)
        {
            this.httpContext = httpContextAccessor.HttpContext;
            this.chatMessageRepository = chatMessageRepository;
            this.userManager = userManager;
        }

        //1.nacin:
        public async Task Announce(string currentPrice)
        {
            var sessionId = httpContext.Session.Id;
            var connectionId = Context.ConnectionId;
            int countOfOrdersInShoppingCart = Convert.ToInt32(dictionaryOfNumberOfOrdersInShoppingCart[sessionId]);
           decimal sumOfProducts = Convert.ToDecimal(dictionaryOfSumOfAllProductsInShoppingCart[sessionId]);
            countOfOrdersInShoppingCart++;
            dictionaryOfNumberOfOrdersInShoppingCart.Remove(sessionId);
            dictionaryOfNumberOfOrdersInShoppingCart.Add(sessionId, countOfOrdersInShoppingCart);

            sumOfProducts =sumOfProducts+ Convert.ToDecimal(currentPrice);
            string sumOfProductsString = sumOfProducts.ToString();
            dictionaryOfSumOfAllProductsInShoppingCart.Remove(sessionId);
            dictionaryOfSumOfAllProductsInShoppingCart.Add(sessionId, sumOfProductsString);

            List<string> connectionIds = dictionaryOfConnections[sessionId];

            //var connectionId= httpContext.Connection.Id;
            await Clients.Clients(connectionIds).SendAsync("Announce", countOfOrdersInShoppingCart, sumOfProductsString);
        }

        //1.nacin:
        public async Task AnnounceSummary()
        {
            var sessionId = httpContext.Session.Id;
            //var connectionId = Context.ConnectionId;
            List<string> connectionIds = dictionaryOfConnections[sessionId];

            int numberOfOrdersInShoppingcart = 0;
            //!!!

            decimal sumOfProducts= 0.00m;

            if (dictionaryOfNumberOfOrdersInShoppingCart.ContainsKey(sessionId))
            {
                numberOfOrdersInShoppingcart = Convert.ToInt32(dictionaryOfNumberOfOrdersInShoppingCart[sessionId]);
            }
            else
            {
                dictionaryOfNumberOfOrdersInShoppingCart.Add(sessionId, numberOfOrdersInShoppingcart);
            }


            if (dictionaryOfSumOfAllProductsInShoppingCart.ContainsKey(sessionId))
            {
                sumOfProducts = Convert.ToDecimal(dictionaryOfSumOfAllProductsInShoppingCart[sessionId]);
            }
            else
            {
                var sumOfProductsString = sumOfProducts.ToString();
                
                dictionaryOfSumOfAllProductsInShoppingCart.Add(sessionId, sumOfProductsString);
            }

            //var connectionId= httpContext.Connection.Id;
            await Clients.Clients(connectionIds).SendAsync("Announce", numberOfOrdersInShoppingcart, sumOfProducts.ToString());
        }

        //addProduct when button clicked:

        public void AddProduct(string productKey)
        {
            var sessionId = httpContext.Session.Id;
            var connectionId = Context.ConnectionId;
            if (dictionaryOfListOfProductsInShoppingCart.ContainsKey(sessionId))
            {

              List<string> listOfProductsKeys=  dictionaryOfListOfProductsInShoppingCart[sessionId];
                listOfProductsKeys.Add(productKey);
                dictionaryOfListOfProductsInShoppingCart.Remove(sessionId);
                dictionaryOfListOfProductsInShoppingCart.Add(sessionId, listOfProductsKeys);
            }
            else
            {
                List<string> listOfProductsKeys = new List<string>();
                listOfProductsKeys.Add(productKey);
                dictionaryOfListOfProductsInShoppingCart.Add(sessionId, listOfProductsKeys);
            }
        }

        public async Task RemoveProduct(string productKey, string currentPrice)
        {
            var sessionId = httpContext.Session.Id;
            var connectionId = Context.ConnectionId;
            decimal currentPriceDecimal = Convert.ToDecimal(currentPrice);
            if (dictionaryOfListOfProductsInShoppingCart.ContainsKey(sessionId) && dictionaryOfNumberOfOrdersInShoppingCart.ContainsKey(sessionId))
            {

                List<string> listOfProductsKeys = dictionaryOfListOfProductsInShoppingCart[sessionId];
                var numberOfSameProductsInShoppingCart = listOfProductsKeys.Where(x => x == productKey).Count();
                //!!!
                decimal priceForRemoving = currentPriceDecimal * numberOfSameProductsInShoppingCart;

                listOfProductsKeys.RemoveAll(x=>x==productKey);
 
                dictionaryOfListOfProductsInShoppingCart.Remove(sessionId);
                dictionaryOfListOfProductsInShoppingCart.Add(sessionId, listOfProductsKeys);

                var numberOfProductsInShoppingCart = Convert.ToInt32(dictionaryOfNumberOfOrdersInShoppingCart[sessionId]);
                numberOfProductsInShoppingCart = numberOfProductsInShoppingCart - numberOfSameProductsInShoppingCart;

                dictionaryOfNumberOfOrdersInShoppingCart.Remove(sessionId);
                dictionaryOfNumberOfOrdersInShoppingCart.Add(sessionId, numberOfProductsInShoppingCart);

                //!!!
                var sumOfAllProductsInShoppingCart = Convert.ToDecimal(dictionaryOfSumOfAllProductsInShoppingCart[sessionId]);
                sumOfAllProductsInShoppingCart = sumOfAllProductsInShoppingCart - priceForRemoving;

                string sumOfAllProductsInShoppingCartStringString = sumOfAllProductsInShoppingCart.ToString();
                dictionaryOfSumOfAllProductsInShoppingCart.Remove(sessionId);
                dictionaryOfSumOfAllProductsInShoppingCart.Add(sessionId, sumOfAllProductsInShoppingCartStringString);

                await AnnounceSummary();
            }
        }

      //remove one when - button on MyShoppingCart.cshtml is clicked
      public async Task RemoveProduct_oneByOne(string productKey, string currentPrice)
         {
            var sessionId = httpContext.Session.Id;
            var connectionId = Context.ConnectionId;
            //!!!
            decimal currentPriceDecimal = Convert.ToDecimal(currentPrice);

            if (dictionaryOfListOfProductsInShoppingCart.ContainsKey(sessionId) && dictionaryOfNumberOfOrdersInShoppingCart.ContainsKey(sessionId))
            {

                List<string> listOfProductsKeys = dictionaryOfListOfProductsInShoppingCart[sessionId];
                var numberOfSameProductsInShoppingCart = listOfProductsKeys.Where(x => x == productKey).Count();

                listOfProductsKeys.Remove(productKey);

                dictionaryOfListOfProductsInShoppingCart.Remove(sessionId);
                dictionaryOfListOfProductsInShoppingCart.Add(sessionId, listOfProductsKeys);

                var numberOfProductsInShoppingCart = Convert.ToInt32(dictionaryOfNumberOfOrdersInShoppingCart[sessionId]);
                numberOfProductsInShoppingCart = numberOfProductsInShoppingCart - 1;

                dictionaryOfNumberOfOrdersInShoppingCart.Remove(sessionId);
                dictionaryOfNumberOfOrdersInShoppingCart.Add(sessionId, numberOfProductsInShoppingCart);

                //!!!
                var sumOfAllProductsInShoppingCart = Convert.ToDecimal(dictionaryOfSumOfAllProductsInShoppingCart[sessionId]);
                sumOfAllProductsInShoppingCart = sumOfAllProductsInShoppingCart - currentPriceDecimal;

                string sumOfAllProductsInShoppingCartStringString = sumOfAllProductsInShoppingCart.ToString();
                dictionaryOfSumOfAllProductsInShoppingCart.Remove(sessionId);
                dictionaryOfSumOfAllProductsInShoppingCart.Add(sessionId, sumOfAllProductsInShoppingCartStringString);

                await AnnounceSummary();
            }
      }

        //send list of products_za popup window:

        public async Task SendListOfProducts()
        {
            var sessionId = httpContext.Session.Id;
            var connectionId = Context.ConnectionId;
            List<string> listOfProductsKeys = null;
            if (dictionaryOfConnections.ContainsKey(sessionId))
            {
                if (dictionaryOfListOfProductsInShoppingCart.Count !=0)
                {
                    listOfProductsKeys = dictionaryOfListOfProductsInShoppingCart[sessionId];
                }
            }

            await Clients.Client(connectionId).SendAsync("ReceiverOfListOfProduct", listOfProductsKeys);
        }

        //send list of products_kada kliknem na dugme "Pogledaj korpu":

        public List<string> GetListOfProducts()
        {
            var sessionId = httpContext.Session.Id;
            //var connectionId = Context.ConnectionId;
            List<string> listOfProductsKeys = null;
            if (dictionaryOfListOfProductsInShoppingCart.ContainsKey(sessionId))
            {
                listOfProductsKeys = dictionaryOfListOfProductsInShoppingCart[sessionId];
            }

            return listOfProductsKeys;
            //await Clients.Client(connectionId).SendAsync("ReceiverOfListOfProduct_StranicaPlacanje", listOfProductsKeys);
        }

        public async Task SendMessageToManagerOfStore(string messageInput, string receiverUserName)
        {
            List<string> connectionIds = new List<string>();

            var receiverUser = await userManager.FindByNameAsync(receiverUserName);

            string receiverUserId = receiverUser.Id;

            if (dictionaryOfLoggedConnections.ContainsKey(receiverUserName))
            {
                connectionIds = dictionaryOfLoggedConnections[receiverUserName];
            }

        
            string senderUserName =  Context.User.Identity.Name;
            string senderUserId = (await userManager.FindByNameAsync(senderUserName)).Id;

            string dateAndTime = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");

            if (connectionIds.Count > 0)
            {
                await Clients.Clients(connectionIds).SendAsync("ReceiverOfMessage", messageInput, senderUserName);
            }


            ChatMessage newMessage = new ChatMessage();

            newMessage.Who_IsSending = senderUserId;
            newMessage.ToWhom__IsSending = receiverUserId;
            newMessage.DateOfSending = Convert.ToDateTime(dateAndTime);
            newMessage.MessageBody = messageInput;
            newMessage.Status = false;
            
            chatMessageRepository.InsertMessage(newMessage);

            //repo.InsertMessage(messenger);
        }



        public async Task UpdateNumberOfUnreadedMessagesAfterReading(string receiverName, string senderName)
        {
            List<string> connectionIds = new List<string>();

            var receiverUserName = (await userManager.FindByIdAsync(receiverName)).UserName;

            if (dictionaryOfLoggedConnections.ContainsKey(receiverUserName))
            {
                connectionIds = dictionaryOfLoggedConnections[receiverUserName];
            }

            string senderUserName = (await userManager.FindByIdAsync(senderName)).UserName;

            int count = chatMessageRepository.GetListOfChatMessageByUnreadMessage(receiverName, false).Count();

            if (connectionIds.Count > 0)
            {
                await Clients.Clients(connectionIds).SendAsync("UpdateNotificationBadge", count);
            }
        }




        public async Task SendNotificationAboutNumberOfUnreadedMessages(string userName, int count)
        {
            var sessionId = httpContext.Session.Id;
            var connectionId = Context.ConnectionId;
            
            List<string> listOfConnectionIds = new List<string>();

           
            if (dictionaryOfLoggedConnections.ContainsKey(userName))
            {
                listOfConnectionIds = dictionaryOfLoggedConnections[userName];
            }

            await Clients.Client(connectionId).SendAsync("UpdateNotificationBadge", count);

        }


        public override async Task OnConnectedAsync()
        {
           var sessionId = httpContext.Session.Id;
           var connectionId = Context.ConnectionId;

           var userName= Context.User.Identity.Name;
            
            if (userName != null)
            {
                var userId = (await userManager.FindByNameAsync(userName)).Id;
                //for chat:

                if (dictionaryOfLoggedConnections.ContainsKey(userName))
                {
                    List<string> listOfConnections = dictionaryOfLoggedConnections[userName];
                    listOfConnections.Add(connectionId);
                    dictionaryOfLoggedConnections.Remove(userName);
                    dictionaryOfLoggedConnections.Add(userName, listOfConnections);
                }
                else
                {
                    List<string> listOfConnections = new List<string>();
                    listOfConnections.Add(connectionId);
                    dictionaryOfLoggedConnections.Add(userName, listOfConnections);
                }

                if (chatMessageRepository.GetListOfChatMessageByUnreadMessage(userId, false).Count > 0)
                {
                    await SendNotificationAboutNumberOfUnreadedMessages(userName, chatMessageRepository.GetListOfChatMessageByUnreadMessage(userId, false).Count);
                }
            }


            if (dictionaryOfConnections.ContainsKey(sessionId))
            {
                List<string> listOfConnections = dictionaryOfConnections[sessionId];

                if (!listOfConnections.Contains(connectionId))
                {
                    listOfConnections.Add(connectionId);
                    dictionaryOfConnections.Remove(sessionId);
                    dictionaryOfConnections.Add(sessionId, listOfConnections);
                }
            }
            else
            {               
                List<string> listOfConnections = new List<string>();
                listOfConnections.Add(connectionId);
                dictionaryOfConnections.Add(sessionId, listOfConnections);
            }

             await base.OnConnectedAsync();
        }


        public override Task OnDisconnectedAsync(Exception exception)
        {
            var sessionId = httpContext.Session.Id;
            var connectionId = Context.ConnectionId;

            // for chat:
            var userName = Context.User.Identity.Name;

            List<string> listOfConnections = dictionaryOfConnections[sessionId];
            if (listOfConnections.Contains(connectionId))
            {
                listOfConnections.Remove(connectionId);

                if (listOfConnections.Count() != 0)
                {
                    dictionaryOfConnections.Remove(sessionId);
                    dictionaryOfConnections.Add(sessionId, listOfConnections);
                }
                else
                {
                    dictionaryOfConnections.Remove(sessionId);

                    //for chat:

                    if (userName != null)
                    {
                        if (dictionaryOfLoggedConnections.ContainsKey(userName))
                        {

                            dictionaryOfLoggedConnections.Remove(userName);

                        }

                    }
                }
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
