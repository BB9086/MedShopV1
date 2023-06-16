using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MedShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Session;
using MedShop.Hubs;
using MedShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace MedShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProductRepository productRepository;
        private readonly IStoreRepository storeRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IOpeningHoursRepository openingHoursRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IOrderItemRepository orderItemRepository;
        private readonly IChatMessageRepository chatMessageRepository;
        private readonly ILogger<HomeController> _logger;
        private readonly ISession session;


        public HomeController(UserManager<ApplicationUser> userManager, IProductRepository productRepository, ICategoryRepository categoryRepository, IStoreRepository storeRepository, IOpeningHoursRepository openingHoursRepository, ICustomerRepository customerRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IChatMessageRepository chatMessageRepository, ILogger<HomeController> logger,
                              IHttpContextAccessor httpContextAccessor)


        {
            this.userManager = userManager;
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.storeRepository = storeRepository;
            this.openingHoursRepository = openingHoursRepository;
            this.customerRepository = customerRepository;
            this.orderRepository = orderRepository;
            this.orderItemRepository = orderItemRepository;
            this.chatMessageRepository = chatMessageRepository;
            _logger = logger;
            this.session = httpContextAccessor.HttpContext.Session;

        }

        public ViewResult Index()
        {
            
            session.SetString("Name", "Borijana");
        
            var model = storeRepository.GetStores().ToList();
         
            return View(model);
        }

        [HttpGet]
        public IActionResult GetProductAndCategoriesByStoreId(int storeId)
        {

            var categories = categoryRepository.GetCategoriesByStoreType(storeId).ToList();

            var products = productRepository.GetProductsByStoreType(storeId).Select(

               x =>
               {
                   x.DescriptionId = (x.Name.Replace(" ", "_") + "_" + x.QuantityOfProduct + "_" + x.ProductKey);
                   return x;
               }

                ).ToList();

            Dictionary<List<Category>, List<Product>> model = new Dictionary<List<Category>, List<Product>>();

            model.Add(categories, products);

            ViewBag.StoreId = storeId;

            return View(model);
        }

        [HttpGet]
        public IActionResult MyShoppingCart()
        {
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();

            MyHub hub = new MyHub(httpContextAccessor, chatMessageRepository, userManager);
            List<string> listOfProducts = hub.GetListOfProducts();
            List<Product> model = GetListOfProductInShoopingCart(listOfProducts).OrderBy(x => x.Name).ToList();

            List<int> storesIds = new List<int>();
            foreach (var product in model)
            {
                storesIds.Add(Convert.ToInt32(product.StoreType));
            }

            ViewBag.StoresIds = storesIds;

            return View(model);
        }


        [HttpPost]
        public JsonResult GetSortedListItemInShopingCartByProductKey(string prodKey)
        {
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();


            List<decimal> list = new List<decimal>();
            MyHub hub = new MyHub(httpContextAccessor, chatMessageRepository, userManager);
            List<string> listOfProducts = hub.GetListOfProducts();
            List<Product> model = GetListOfProductInShoopingCart(listOfProducts).OrderBy(x => x.Name).ToList();
            int numberOfSpecificProductKey = model.FirstOrDefault(x => x.ProductKey == prodKey).NumberOfSameProductInShoppingCart;

            decimal sum = 0.0m;

            foreach (var product in model)
            {
                sum = sum + product.CurrentPrice * product.NumberOfSameProductInShoppingCart;
            }

            list.Add(numberOfSpecificProductKey);
            list.Add(sum);

            return Json(list);
        }

        [Authorize]
        [HttpGet]

        public IActionResult CreateCustomer(string valueOfShippingMethod, string storeId)
        {
           
            ViewBag.ShippingMethod = valueOfShippingMethod;
            ViewBag.StoreId = storeId;
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();

            List<decimal> list = new List<decimal>();
            MyHub hub = new MyHub(httpContextAccessor, chatMessageRepository, userManager);
            List<string> listOfProducts = hub.GetListOfProducts();
            List<Product> model = GetListOfProductInShoopingCart(listOfProducts).OrderBy(x => x.Name).ToList();

            if (model.Count() == 0)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [Authorize]
        [HttpPost]

        public RedirectToActionResult CreateCustomer(CreateCustomerViewModel createCustomerViewModel, string shippingMethod, string storeId)
        {

            return RedirectToAction("DeliveryDetails", new { shippingMethod = shippingMethod, storeId = storeId });
        }

        [HttpPost]
        public JsonResult SaveCustomerInfo(Customer customer)
        {
            Customer cus = customer;

            return Json(customer);
        }



        [Authorize]
        [HttpGet]
        public IActionResult DeliveryDetails(string shippingMethod, string storeId)
        {

            var sessionId = HttpContext.Session.Id;

            DeliveryDetailsViewModel deliveryDetailsViewModel = new DeliveryDetailsViewModel();

            var sumOfOrderedProducts = MyHub.dictionaryOfSumOfAllProductsInShoppingCart[sessionId].ToString();

            ViewBag.TotalPrice = sumOfOrderedProducts;

            if (Convert.ToDecimal(sumOfOrderedProducts) == 0.00m)
            {
                return RedirectToAction("Index");
            }

            return View(deliveryDetailsViewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeliveryDetails(DeliveryDetailsViewModel deliveryDetailsViewModel, string btnOrderProduct)
        {

            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();

            var sessionId = HttpContext.Session.Id;

            List<decimal> list = new List<decimal>();
            MyHub hub = new MyHub(httpContextAccessor, chatMessageRepository, userManager);
            List<string> listOfProducts = hub.GetListOfProducts();
            List<Product> model = GetListOfProductInShoopingCart(listOfProducts).OrderBy(x => x.Name).ToList();

            Customer customer = new Customer();
            customer.FirstName = deliveryDetailsViewModel.FirstName;
            customer.LastName = deliveryDetailsViewModel.LastName;
            customer.Email = deliveryDetailsViewModel.Contact;
            customer.ZipCode = deliveryDetailsViewModel.ZipCode;
            customer.Country = deliveryDetailsViewModel.Country;
            customer.City = deliveryDetailsViewModel.City;
            customer.TelephoneNumber = deliveryDetailsViewModel.TelephoneNumber;
            customer.Address = deliveryDetailsViewModel.Address;

            customer.LoginUserId = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result.Id;

            if (!TryValidateModel(customer))
            {
                return RedirectToAction("CreateCustomer", new { storeId = deliveryDetailsViewModel.StoreId, valueOfShippingMethod = deliveryDetailsViewModel.ShippingMethod });
            }

            Customer cus = customerRepository.AddCustomer(customer);

            Order order = new Order();
            order.CustomerId = cus.CustomerId;
            order.StoreId = deliveryDetailsViewModel.StoreId;
            order.ShippingMethod = deliveryDetailsViewModel.ShippingMethod;
            order.CreatedOn = DateTime.Now;
            order.OrderStatus = 10;
            order.ShippingStatus = 10;
            order.PaymentStatus = 10;
            order.CustomerIpAddress = System.Net.IPAddress.Loopback.ToString();
            order.ShippingDate = Convert.ToDateTime(session.GetString("reservationDateTime"));
            order.OrderTotal = Convert.ToDecimal(btnOrderProduct);

            orderRepository.AddOrder(order);

            foreach (var product in model)
            {
                OrderItem orderItem = new OrderItem();
                orderItem.OrderId = order.OrderId;
                orderItem.ProductId = product.ProductId;
                orderItem.Quantity = Convert.ToInt32(product.NumberOfSameProductInShoppingCart);

                orderItemRepository.AddOrderItem(orderItem);

            }

            MyHub.dictionaryOfNumberOfOrdersInShoppingCart[sessionId] = 0;
            MyHub.dictionaryOfSumOfAllProductsInShoppingCart[sessionId] = "0.00";
            MyHub.dictionaryOfListOfProductsInShoppingCart[sessionId] = new List<string>();

            return RedirectToAction("ThankYou");
        }

        //metoda InsertOrderAndCustomerInfo ukoliko odlucis da koristis ajax metodu koja je zakomentarisana u DeliveryDetails.cshtml

        //[HttpPost]
        //public JsonResult InsertOrderAndCustomerInfo(DeliveryDetailsViewModel deliveryDetailsViewModel, string btnOrderProduct)
        //{
        //    IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();

        //    var sessionId = HttpContext.Session.Id;

        //    List<decimal> list = new List<decimal>();
        //    MyHub hub = new MyHub(httpContextAccessor);
        //    List<string> listOfProducts = hub.GetListOfProducts();
        //    List<Product> model = GetListOfProductInShoopingCart(listOfProducts).OrderBy(x => x.Name).ToList();

        //    //add new customer:
        //    Customer customer = new Customer();
        //    customer.FirstName = deliveryDetailsViewModel.FirstName;
        //    customer.LastName = deliveryDetailsViewModel.LastName;
        //    customer.Email = deliveryDetailsViewModel.Contact;
        //    customer.ZipCode = deliveryDetailsViewModel.ZipCode;
        //    customer.Country = deliveryDetailsViewModel.Country;
        //    customer.City = deliveryDetailsViewModel.City;
        //    customer.TelephoneNumber = deliveryDetailsViewModel.TelephoneNumber;
        //    customer.Address = deliveryDetailsViewModel.Address;

        //    customer.LoginUserId = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result.Id;
        //    Customer cus = customerRepository.AddCustomer(customer);


        //    //add new order

        //    Order order = new Order();
        //    order.CustomerId = cus.CustomerId;
        //    order.StoreId = deliveryDetailsViewModel.StoreId;
        //    order.ShippingMethod = deliveryDetailsViewModel.ShippingMethod;
        //    order.CreatedOn = DateTime.Now;
        //    order.OrderStatus = 10;
        //    order.ShippingStatus = 10;
        //    order.PaymentStatus = 10;
        //    order.CustomerIpAddress = System.Net.IPAddress.Loopback.ToString();
        //    order.ShippingDate = reservationDateTime;
        //    order.OrderTotal = Convert.ToDecimal("1968");

        //    orderRepository.AddOrder(order);
        //    //add new orderitem
        //    foreach (var product in model)
        //    {
        //        OrderItem orderItem = new OrderItem();
        //        orderItem.OrderId = order.OrderId;
        //        orderItem.ProductId = product.ProductId;
        //        orderItem.Quantity = Convert.ToInt32(product.NumberOfSameProductInShoppingCart);

        //        orderItemRepository.AddOrderItem(orderItem);

        //    }

        //    //anuliram sve u korpi kada upisem narudzbinu u bazu:
        //    MyHub.dictionaryOfNumberOfOrdersInShoppingCart[sessionId] = 0;
        //    MyHub.dictionaryOfSumOfAllProductsInShoppingCart[sessionId] = "0.00";
        //    MyHub.dictionaryOfListOfProductsInShoppingCart[sessionId] = new List<string>();


        //    return Json("true");

        //}



        [HttpPost]
        public JsonResult GetReservationDateTime(string selectedDate, string selectedHour, string selectedMinutes)
        {
            var selDate = selectedDate;
            var selHour = Convert.ToInt32(selectedHour);
            var selMinutes = Convert.ToInt32(selectedMinutes);

            TimeSpan timeSpan = new TimeSpan(selHour, selMinutes, 0);

            DateTime reservationDT = Convert.ToDateTime(selectedDate).Add(timeSpan);
            session.SetString("reservationDateTime", reservationDT.ToString());

            return Json("true");
        }


        [Authorize]
        [HttpGet]
        public ViewResult ThankYou()
        {
            return View();
        }


        [HttpGet]
        public IActionResult RezervacijaIPlacanje(int storeId)
        {
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();


            MyHub hub = new MyHub(httpContextAccessor, chatMessageRepository, userManager);
            List<string> listOfProducts = hub.GetListOfProducts();
            List<Product> model = GetListOfProductInShoopingCart(listOfProducts);

            ViewBag.StoreId = storeId;
            ViewBag.NumberOfListInShoopingCart = model.Count();

            int bufferTime = storeRepository.GetBufferTimeByStoreTypeAndShippingMethod(storeId, "Pickup");

            Dictionary<int, List<int>> dictionaryOfAvailableHoursAndMinutes = new Dictionary<int, List<int>>();

            dictionaryOfAvailableHoursAndMinutes.Clear();


            var currentDate = DateTime.Now.Date;


            var nextDate = currentDate;

            //check if closed:

            while (openingHoursRepository.IsStoreClosed((int)nextDate.DayOfWeek, storeId))
            {

                nextDate = nextDate.AddDays(1);
            }


            int nextOpeningHour = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId((int)nextDate.DayOfWeek, storeId).OpenFrom.Split(":")[0]);
            int nextOpeningMinute = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId((int)nextDate.DayOfWeek, storeId).OpenFrom.Split(":")[1].Split(":")[0]);
            TimeSpan timeEarliest = new TimeSpan(nextOpeningHour, nextOpeningMinute, 0);
            var nextOpeningtDateTime = nextDate.Add(timeEarliest);


            int nextClosingHour = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId((int)nextDate.DayOfWeek, storeId).OpenTo.Split(":")[0]);
            int nextClosingMinute = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId((int)nextDate.DayOfWeek, storeId).OpenTo.Split(":")[1].Split(":")[0]);
            TimeSpan timeLatest = new TimeSpan(nextClosingHour, nextClosingMinute, 0);
            var nextClosingDateTime = nextDate.Add(timeLatest);

            if (nextDate == currentDate)
            {
                var startingDateTimeWithBufferTime = DateTime.Now.AddMinutes(bufferTime);
                int startingHour = startingDateTimeWithBufferTime.Hour;
                int startingMinute = startingDateTimeWithBufferTime.Minute;

                if (startingDateTimeWithBufferTime.Date == currentDate)
                {
                    if (startingDateTimeWithBufferTime <= nextOpeningtDateTime)
                    {
                        for (int h = nextOpeningHour; h <= nextClosingHour; h++)
                        {
                            List<int> minutes = new List<int>();
                            if (h == nextOpeningHour)
                            {
                                for (int minute = nextOpeningMinute; minute < 55; minute = minute + 15)
                                {
                                    minutes.Add(minute);
                                }
                            }
                            else
                            {
                                if (h == nextClosingHour)
                                {
                                    //if (nextClosingMinute != 0)
                                    //{
                                    for (int minute = 0; minute < nextClosingMinute; minute = minute + 15)
                                    {
                                        minutes.Add(minute);
                                    }
                                    //}
                                }
                                else
                                {
                                    for (int minute = 0; minute < 55; minute = minute + 15)
                                    {
                                        minutes.Add(minute);
                                    }

                                }
                            }
                            if (minutes.Any())
                            {
                                dictionaryOfAvailableHoursAndMinutes.Add(h, minutes);
                            }

                        }
                    }

                    else if (nextOpeningtDateTime < startingDateTimeWithBufferTime && startingDateTimeWithBufferTime <= nextClosingDateTime)

                    {
                        for (int h = startingHour; h <= nextClosingHour; h++)
                        {
                            List<int> minutes = new List<int>();
                            if (h == startingHour)
                            {
                                if (h == nextClosingHour)
                                {
                                    for (int minute = startingMinute; minute < nextClosingMinute; minute = minute + 15)
                                    {
                                        minutes.Add(minute);
                                    }
                                }
                                else
                                {
                                    for (int minute = startingMinute; minute < 55; minute = minute + 15)
                                    {
                                        minutes.Add(minute);
                                    }

                                }
                            }
                            else
                            {
                                if (h != nextClosingHour)
                                {
                                    for (int minute = 0; minute < 55; minute = minute + 15)
                                    {
                                        minutes.Add(minute);
                                    }
                                }
                                else
                                {
                                    for (int minute = 0; minute < nextClosingMinute; minute = minute + 15)
                                    {
                                        minutes.Add(minute);
                                    }
                                }

                            }

                            if (minutes.Any())
                            {
                                dictionaryOfAvailableHoursAndMinutes.Add(h, minutes);
                            }
                        }
                    }

                }
                else
                {
                    nextDate = nextDate.AddDays(1);

                    while (openingHoursRepository.IsStoreClosed((int)nextDate.DayOfWeek, storeId))
                    {

                        nextDate = nextDate.AddDays(1);
                    }

                    int nextOpeningHourNew = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId((int)nextDate.DayOfWeek, storeId).OpenFrom.Split(":")[0]);
                    int nextOpeningMinuteNew = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId((int)nextDate.DayOfWeek, storeId).OpenFrom.Split(":")[1].Split(":")[0]);
                    TimeSpan timeEarliestNew = new TimeSpan(nextOpeningHourNew, nextOpeningMinuteNew, 0);
                    var nextOpeningtDateTimeNew = nextDate.Add(timeEarliestNew);


                    int nextClosingHourNew = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId((int)nextDate.DayOfWeek, storeId).OpenTo.Split(":")[0]);
                    int nextClosingMinuteNew = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId((int)nextDate.DayOfWeek, storeId).OpenTo.Split(":")[1].Split(":")[0]);
                    TimeSpan timeLatestNew = new TimeSpan(nextClosingHourNew, nextClosingMinuteNew, 0);
                    var nextClosingDateTimeNew = nextDate.Add(timeLatestNew);

                    if (startingDateTimeWithBufferTime <= nextOpeningtDateTimeNew)
                    {
                        for (int h = nextOpeningHourNew; h <= nextClosingHourNew; h++)
                        {
                            List<int> minutes = new List<int>();
                            if (h == nextOpeningHourNew)
                            {
                                if (h == nextClosingHourNew)
                                {
                                    for (int minute = nextOpeningMinuteNew; minute < nextClosingMinuteNew; minute = minute + 15)
                                    {
                                        minutes.Add(minute);
                                    }
                                }
                                else
                                {
                                    for (int minute = nextOpeningMinuteNew; minute < 55; minute = minute + 15)
                                    {
                                        minutes.Add(minute);
                                    }
                                }


                            }
                            else
                            {
                                if (h == nextClosingHourNew)
                                {
                                    for (int minute = 0; minute < nextClosingMinuteNew; minute = minute + 15)
                                    {
                                        minutes.Add(minute);
                                    }
                                }
                                else
                                {
                                    for (int minute = 0; minute < 55; minute = minute + 15)
                                    {
                                        minutes.Add(minute);
                                    }
                                }
                            }

                            if (minutes.Any())
                            {
                                dictionaryOfAvailableHoursAndMinutes.Add(h, minutes);
                            }

                        }

                    }
                    else if (startingDateTimeWithBufferTime > nextOpeningtDateTimeNew && startingDateTimeWithBufferTime <= nextClosingDateTimeNew)
                    {
                    
                        for (int h = startingHour; h <= nextClosingHourNew; h++)
                        {
                            List<int> minutes = new List<int>();
                            if (h == startingHour)
                            {
                                if (h == nextClosingHourNew)
                                {
                                    for (int minute = startingMinute; minute < nextClosingMinuteNew; minute = minute + 15)
                                    {
                                        minutes.Add(minute);
                                    }
                                }
                                else
                                {
                                    for (int minute = startingMinute; minute < 55; minute = minute + 15)
                                    {
                                        minutes.Add(minute);
                                    }
                                }


                            }

                            else
                            {
                                if (h == nextClosingHourNew)
                                {
                                    for (int minute = 0; minute < nextClosingMinuteNew; minute = minute + 15)
                                    {
                                        minutes.Add(minute);
                                    }
                                }
                                else
                                {
                                    for (int minute = 0; minute < 55; minute = minute + 15)
                                    {
                                        minutes.Add(minute);
                                    }
                                }
                            }

                            if (minutes.Any())
                            {
                                dictionaryOfAvailableHoursAndMinutes.Add(h, minutes);
                            }

                        }

                    }

                }


            }
            else
            {

                while (openingHoursRepository.IsStoreClosed((int)nextDate.DayOfWeek, storeId))
                {

                    nextDate = nextDate.AddDays(1);
                }


                int nextOpeningHourNew = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId((int)nextDate.DayOfWeek, storeId).OpenFrom.Split(":")[0]);
                int nextOpeningMinuteNew = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId((int)nextDate.DayOfWeek, storeId).OpenFrom.Split(":")[1].Split(":")[0]);
                TimeSpan timeEarliestNew = new TimeSpan(nextOpeningHourNew, nextOpeningMinuteNew, 0);
                var nextOpeningtDateTimeNew = nextDate.Add(timeEarliestNew);


                int nextClosingHourNew = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId((int)nextDate.DayOfWeek, storeId).OpenTo.Split(":")[0]);
                int nextClosingMinuteNew = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId((int)nextDate.DayOfWeek, storeId).OpenTo.Split(":")[1].Split(":")[0]);
                TimeSpan timeLatestNew = new TimeSpan(nextClosingHourNew, nextClosingMinuteNew, 0);
                var nextClosingDateTimeNew = nextDate.Add(timeLatestNew);

                for (int h = nextOpeningHourNew; h <= nextClosingHourNew; h++)
                {
                    List<int> minutes = new List<int>();
                    if (h == nextOpeningHourNew)
                    {
                        if (h == nextClosingHourNew)
                        {
                            for (int minute = nextOpeningMinuteNew; minute < nextClosingMinuteNew; minute = minute + 15)
                            {
                                minutes.Add(minute);
                            }
                        }
                        else
                        {
                            for (int minute = nextOpeningMinuteNew; minute < 55; minute = minute + 15)
                            {
                                minutes.Add(minute);
                            }
                        }


                    }
                    else
                    {
                        if (h == nextClosingHourNew)
                        {
                            for (int minute = 0; minute < nextClosingMinuteNew; minute = minute + 15)
                            {
                                minutes.Add(minute);
                            }
                        }
                        else
                        {
                            for (int minute = 0; minute < 55; minute = minute + 15)
                            {
                                minutes.Add(minute);
                            }
                        }
                    }

                    if (minutes.Any())
                    {
                        dictionaryOfAvailableHoursAndMinutes.Add(h, minutes);
                    }

                }

            }
         
            if (dictionaryOfAvailableHoursAndMinutes.Count() == 0)
            {

                nextDate = nextDate.AddDays(1);
      
                while (openingHoursRepository.IsStoreClosed((int)nextDate.DayOfWeek, storeId))
                {
                    nextDate = nextDate.AddDays(1);
                }

                int nextOpeningHourNEW = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId((int)nextDate.DayOfWeek, storeId).OpenFrom.Split(":")[0]);
                int nextOpeningMinuteNEW = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId((int)nextDate.DayOfWeek, storeId).OpenFrom.Split(":")[1].Split(":")[0]);
                TimeSpan timeEarliestNEW = new TimeSpan(nextOpeningHourNEW, nextOpeningMinuteNEW, 0);
                var nextOpeningtDateTimeNEW = nextDate.Add(timeEarliestNEW);

                int nextClosingHourNEW = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId((int)nextDate.DayOfWeek, storeId).OpenTo.Split(":")[0]);
                int nextClosingMinuteNEW = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId((int)nextDate.DayOfWeek, storeId).OpenTo.Split(":")[1].Split(":")[0]);
                TimeSpan timeLatestNEW = new TimeSpan(nextClosingHourNEW, nextClosingMinuteNEW, 0);
                var nextClosingDateTimeNEW = nextDate.Add(timeLatestNEW);

                for (int h = nextOpeningHourNEW; h <= nextClosingHourNEW; h++)
                {
                    List<int> minutes = new List<int>();
                    if (h == nextOpeningHourNEW)
                    {
                        if (h == nextClosingHourNEW)
                        {
                            for (int minute = nextOpeningMinuteNEW; minute < nextClosingMinuteNEW; minute = minute + 15)
                            {
                                minutes.Add(minute);
                            }
                        }
                        else
                        {
                            for (int minute = nextOpeningMinuteNEW; minute < 55; minute = minute + 15)
                            {
                                minutes.Add(minute);
                            }
                        }


                    }
                    else
                    {
                        if (h == nextClosingHourNEW)
                        {
                            for (int minute = 0; minute < nextClosingMinuteNEW; minute = minute + 15)
                            {
                                minutes.Add(minute);
                            }
                        }
                        else
                        {
                            for (int minute = 0; minute < 55; minute = minute + 15)
                            {
                                minutes.Add(minute);
                            }
                        }
                    }

                    if (minutes.Any())
                    {
                        dictionaryOfAvailableHoursAndMinutes.Add(h, minutes);
                    }

                }
            }

            ViewBag.Date = nextDate.Date.ToString("MM/dd/yyyy");

            string dictionaryString = "{";
            foreach (KeyValuePair<int, List<int>> keyValues in dictionaryOfAvailableHoursAndMinutes)
            {
                if (keyValues.Value != null)
                {
                    dictionaryString += keyValues.Key + " : " + "[" + string.Join(", ", keyValues.Value) + "]" + ", ";
                }
            }
            dictionaryString = dictionaryString.TrimEnd(',', ' ') + "}";
            session.SetString("dictionaryOfAvailableHoursAndMinutesForSession", dictionaryString);


            return View(dictionaryOfAvailableHoursAndMinutes);
        }

        [HttpPost]
        public JsonResult GetAvailableHoursAndMinutesBySelectedHour(string selectedDate, string selectedHour)
        {
            string str = session.GetString("dictionaryOfAvailableHoursAndMinutesForSession");

            var dictionaryOfAvailableHoursAndMinutes = JsonConvert.DeserializeObject<Dictionary<int, List<int>>>(str);

            return Json(dictionaryOfAvailableHoursAndMinutes[Convert.ToInt32(selectedHour)]);
        }


        [HttpPost]
        public JsonResult GetAvailableHoursAndMinutesBySelectedDate(string selectedDate, string shippingMethod, string storeId)
        {
            var bufferTime = storeRepository.GetBufferTimeByStoreTypeAndShippingMethod(Convert.ToInt32(storeId), shippingMethod);

            Dictionary<int, List<int>> dictionaryOfAvailableHoursAndMinutes = new Dictionary<int, List<int>>();

            dictionaryOfAvailableHoursAndMinutes.Clear();

            var selectedDayOfWeek = (int)Convert.ToDateTime(selectedDate).Date.DayOfWeek;

            int openingHour = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId(selectedDayOfWeek, Convert.ToInt32(storeId)).OpenFrom.Split(":")[0]);
            int openingMinute = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId(selectedDayOfWeek, Convert.ToInt32(storeId)).OpenFrom.Split(":")[1].Split(":")[0]);
            TimeSpan timeEarliest = new TimeSpan(openingHour, openingMinute, 0);
            var openingDateTime = Convert.ToDateTime(selectedDate).Date.Add(timeEarliest);

            int closingHour = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId(selectedDayOfWeek, Convert.ToInt32(storeId)).OpenTo.Split(":")[0]);
            int closingMinute = Convert.ToInt32(openingHoursRepository.GetOpeningHourByDayOfWeekAndStoreId(selectedDayOfWeek, Convert.ToInt32(storeId)).OpenTo.Split(":")[1].Split(":")[0]);
            TimeSpan timeLatest = new TimeSpan(closingHour, closingMinute, 0);
            var closingDateTime = Convert.ToDateTime(selectedDate).Date.Add(timeLatest);

            if (openingHoursRepository.IsStoreClosed(selectedDayOfWeek, Convert.ToInt32(storeId)))
            {
                dictionaryOfAvailableHoursAndMinutes.Add(-1, null);
            }

            else
            {
                if (Convert.ToDateTime(selectedDate).Date != DateTime.Now.Date)
                {

                    for (int h = openingHour; h <= closingHour; h++)
                    {
                        List<int> minutes = new List<int>();
                        if (h == openingHour)
                        {

                            for (int minute = openingMinute; minute < 55; minute = minute + 15)
                            {
                                minutes.Add(minute);
                            }
                        }
                        else
                        {
                            if (h == closingHour)
                            {

                                for (int minute = 0; minute < closingMinute; minute = minute + 15)
                                {
                                    minutes.Add(minute);
                                }


                            }
                            else
                            {
                                for (int minute = 0; minute < 55; minute = minute + 15)
                                {
                                    minutes.Add(minute);
                                }
                            }
                        }
                        if (minutes.Any())
                        {
                            dictionaryOfAvailableHoursAndMinutes.Add(h, minutes);
                        }

                    }

                }
                else
                {

                    var startingDateTimeWithBufferTime = DateTime.Now.AddMinutes(bufferTime);
                    int startingHour = startingDateTimeWithBufferTime.Hour;
                    int startingMinute = startingDateTimeWithBufferTime.Minute;

                    if (startingDateTimeWithBufferTime.Date == DateTime.Now.Date)
                    {
                        if (startingDateTimeWithBufferTime <= openingDateTime)
                        {
                            for (int h = openingHour; h <= closingHour; h++)
                            {
                                List<int> minutes = new List<int>();
                                if (h == openingHour)
                                {
                                    for (int minute = openingMinute; minute < 55; minute = minute + 15)
                                    {
                                        minutes.Add(minute);
                                    }
                                }
                                else
                                {
                                    if (h == closingHour)
                                    {
                                        //if (nextClosingMinute != 0)
                                        //{
                                        for (int minute = 0; minute < closingMinute; minute = minute + 15)
                                        {
                                            minutes.Add(minute);
                                        }
                                        //}
                                    }
                                    else
                                    {
                                        for (int minute = 0; minute < 55; minute = minute + 15)
                                        {
                                            minutes.Add(minute);
                                        }

                                    }
                                }
                                if (minutes.Any())
                                {
                                    dictionaryOfAvailableHoursAndMinutes.Add(h, minutes);
                                }

                            }
                        }

                        else if (openingDateTime < startingDateTimeWithBufferTime && startingDateTimeWithBufferTime <= closingDateTime)

                        {
                            for (int h = startingHour; h <= closingHour; h++)
                            {
                                List<int> minutes = new List<int>();
                                if (h == startingHour)
                                {
                                    if (h == closingHour)
                                    {
                                        for (int minute = startingMinute; minute < closingMinute; minute = minute + 15)
                                        {
                                            minutes.Add(minute);
                                        }
                                    }
                                    else
                                    {
                                        for (int minute = startingMinute; minute < 55; minute = minute + 15)
                                        {
                                            minutes.Add(minute);
                                        }

                                    }
                                }
                                else
                                {
                                    if (h != closingHour)
                                    {
                                        for (int minute = 0; minute < 55; minute = minute + 15)
                                        {
                                            minutes.Add(minute);
                                        }
                                    }
                                    else
                                    {
                                        for (int minute = 0; minute < closingMinute; minute = minute + 15)
                                        {
                                            minutes.Add(minute);
                                        }
                                    }

                                }

                                if (minutes.Any())
                                {
                                    dictionaryOfAvailableHoursAndMinutes.Add(h, minutes);
                                }
                            }
                        }

                    }
                    else
                    {
                        dictionaryOfAvailableHoursAndMinutes.Add(-1, null);
                    }


                }
            }

            if (dictionaryOfAvailableHoursAndMinutes.Count() == 0)
            {
                dictionaryOfAvailableHoursAndMinutes.Add(-1, null);

            }

            string dictionaryString = "{";
            foreach (KeyValuePair<int, List<int>> keyValues in dictionaryOfAvailableHoursAndMinutes)
            {
                if (keyValues.Value != null)
                {
                    dictionaryString += keyValues.Key + " : " + "[" + string.Join(", ", keyValues.Value) + "]" + ", ";

                }

            }
            dictionaryString = dictionaryString.TrimEnd(',', ' ') + "}";
            session.SetString("dictionaryOfAvailableHoursAndMinutesForSession", dictionaryString);

            return Json(dictionaryOfAvailableHoursAndMinutes.ToArray());

        }

        [HttpGet]
        //[Authorize]
        public ViewResult Products(string id)
        {
            var str = id.Split('_');
            var productKey = str[str.Length - 1];
            Product model = productRepository.GetProductByProductCode(productKey);
            return View(model);
        }


        [HttpPost]
        public List<Product> GetListOfProductInShoopingCart(List<string> productKeys)
        {
            List<Product> products = new List<Product>();
            if (productKeys != null)
            {
                foreach (string productKey in productKeys)
                {
                    if (products.Any(x => x.ProductKey == productKey))
                    {
                        Product product = productRepository.GetProductByProductCode(productKey);
                        product.NumberOfSameProductInShoppingCart++;
                        product.DescriptionId = (product.Name.Replace(" ", "_") + "_" + product.QuantityOfProduct + "_" + product.ProductKey);
                        products.Remove(product);
                        products.Add(product);
                    }
                    else
                    {
                        Product product = productRepository.GetProductByProductCode(productKey);
                        product.NumberOfSameProductInShoppingCart = 1;
                        product.DescriptionId= (product.Name.Replace(" ", "_") + "_" + product.QuantityOfProduct + "_" + product.ProductKey);
                        products.Add(product);
                    }

                }
            }

            return products;
        }


        [HttpPost]
        public IActionResult CheckIfUserIsOnline(string chatWith)
        {

            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();

            MyHub hub = new MyHub(httpContextAccessor, chatMessageRepository, userManager);

            var dictionaryOfLoggedUsers = MyHub.dictionaryOfLoggedConnections;

            if (dictionaryOfLoggedUsers.ContainsKey(chatWith))
            {
                return Json("true");
            }
            else
            {
                return Json("false");
            }
        }


        public IActionResult Privacy()
        {
            //var connectionId=  HttpContext.Connection.Id;
            //HttpContext.Session.SetString("Name", "Borijana");
            //var x = HttpContext.Session.Id;
            //ViewBag.SessionId = x;
            ////var x = context.Request.Cookies[".TokenAuthCookies"];
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[HttpGet]
        //public ViewResult Primer(string name)
        //{

        //    return View();
        //}

        //[HttpPost]
        //public ViewResult Primer(string name, int i, string butt)
        //{
        //    var nam = name;
        //    var ix = i;
        //    if (nam == "Boki")
        //    {
        //        ModelState.AddModelError("", "Desila se neka greska");
        //        return View();
        //    }
        //    return View();
        //}

    }
}
