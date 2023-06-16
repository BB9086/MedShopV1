using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MedShop.Models;
using MedShop.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Controllers
{
   [Authorize]
    public class ManageController : Controller
    {
        private readonly IStoreRepository storeRepository;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductRepository productRepository;
        private readonly IShippingRepository shippingRepository;
        private readonly IOrderItemRepository orderItemRepository;
        private readonly IChatMessageRepository chatMessageRepository;
        private readonly IOpeningHoursRepository openingHoursRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        public ManageController(IStoreRepository storeRepository, UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager, ICategoryRepository categoryRepository, IProductRepository productRepository, IShippingRepository shippingRepository, IOrderItemRepository orderItemRepository, IChatMessageRepository chatMessageRepository, IOpeningHoursRepository openingHoursRepository, IHostingEnvironment hostingEnvironment)
        {
            this.storeRepository = storeRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
            this.shippingRepository = shippingRepository;
            this.orderItemRepository = orderItemRepository;
            this.chatMessageRepository = chatMessageRepository;
            this.openingHoursRepository = openingHoursRepository;
            this.hostingEnvironment = hostingEnvironment;
        }


        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> ListOfStores()
        {
            var user = await userManager.FindByEmailAsync(User.Identity.Name);

            if (User.IsInRole("Admin"))
            {
                var model = storeRepository.GetStores();
                int count = model.Count();
                return View(model);
            }
            else
            {

                var model = storeRepository.GetStores().Where(x=>x.CreatedByUserId== user.Id);
                return View(model);
            }
            
            
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateStore()
        {
            List<string> managerList = new List<string>();
           var allUsers= userManager.Users.ToList();

            foreach(var user in allUsers)
            {
              if(await userManager.IsInRoleAsync(user, "Manager"))
                {
                    managerList.Add(user.Email);
                }
            }
            ViewBag.ManagerList = managerList;

            return View();

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateStore(CreateStoreViewModel createStoreViewModel)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                var storeManager = await userManager.FindByNameAsync(createStoreViewModel.ManagerEmail);
               string storeManagerId= storeManager.Id;

                if (createStoreViewModel.Photo != null)
                {
                   string uploadsFolder= Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + createStoreViewModel.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    createStoreViewModel.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Store newStore = new Store
                {   StoreAddress = createStoreViewModel.StoreAddress,
                    StoreDescription = createStoreViewModel.StoreDescription,
                    StoreGuid = Guid.NewGuid(),
                    StoreName=createStoreViewModel.StoreName,
                    WebSite=createStoreViewModel.WebSite,
                    Telephone=createStoreViewModel.Telephone,
                    ZipCodeAndCity=createStoreViewModel.ZipCodeAndCity,
                    ImageSource="~/images/"+uniqueFileName,
                    CreatedByUserId= storeManagerId
                };

                storeRepository.InsertStore(newStore);

                var stores = storeRepository.GetStores();
                int maxId = 0;
                foreach(var store in stores)
                {
                    if (store.StoreId > maxId)
                    {
                        maxId = store.StoreId;
                    }
                }

                Shipping newShippingDelivery = new Shipping()
                {
                    ShippingMethod = "Delivery",
                    BufferTime = createStoreViewModel.DeliveryShippingMethod,
                    StoreType = maxId
                };
                
                Shipping newShippingPickup = new Shipping()
                {
                    ShippingMethod = "Pickup",
                    BufferTime = createStoreViewModel.PickupShippingMethod,
                    StoreType = maxId
                };
                shippingRepository.AddShippingInfoForNewStore(newShippingDelivery);
                shippingRepository.AddShippingInfoForNewStore(newShippingPickup);


                return RedirectToAction("CreateWorkingHours", new {storeId= maxId });

            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditStore(string storeId)
        {
            var storeFromDB = storeRepository.GetStoreById(Convert.ToInt32(storeId));
            //EditProductViewModel editProductViewModel = new EditProductViewModel();

            if (storeFromDB != null)
            {
                return View(storeFromDB);
            }
            else
            {
                return View("NotFound");
            }
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditStore(Store store)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                string uniqueFilePath = null;

                if (store.Photo != null)
                {
                    //get phisical path of wwwroot, tj images folder:

                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + store.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    store.Photo.CopyTo(new FileStream(filePath, FileMode.Create));

                    uniqueFilePath = "~/images/" + uniqueFileName;

                    store.ImageSource = uniqueFilePath;
                }

                storeRepository.UpdateStore(store);

                return RedirectToAction("ListOfStores", "manage");
            }
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteStore(string storeId)
        {
            var store = storeRepository.GetStoreById(Convert.ToInt32(storeId));

            if (store != null)
            {
                var result = storeRepository.DeleteStore(Convert.ToInt32(storeId));

                return RedirectToAction("ListOfStores");     
            }
            else
            {
                return View("NotFound");
            }
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateWorkingHours(int storeId)
        {
          
            List<CreateWorkingHoursViewModel> listOfWorkingHours = new List<CreateWorkingHoursViewModel>();

            for(int i = 0; i < 7; i++)
            {
                CreateWorkingHoursViewModel model = new CreateWorkingHoursViewModel()
                {
                    DayOfWeek = i,
                    OpenFrom="00:00:00",
                    OpenTo="24:00:00",
                    Closed=true,
                    StoreId=storeId
                };

                listOfWorkingHours.Add(model);
                
            }

            return View(listOfWorkingHours);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult CreateWorkingHours(List<CreateWorkingHoursViewModel> example)
        {

           foreach(var createWorkingHoursViewModel in example)
            {
                OpeningHours openingHours = new OpeningHours
                {
                    StoreId=createWorkingHoursViewModel.StoreId,
                    Closed=createWorkingHoursViewModel.Closed,
                    DayOfWeek=createWorkingHoursViewModel.DayOfWeek,
                    OpenFrom=createWorkingHoursViewModel.OpenFrom,
                    OpenTo=createWorkingHoursViewModel.OpenTo
                };

              var openingInserted= openingHoursRepository.InsertOpeningHour(openingHours);

                if (openingInserted == null)
                {
                    return Json("false");
                }
            }

            return Json("true");
        }



        [HttpGet]
        [Authorize(Roles = "Manager")]
        public IActionResult CreateProduct(int storeId)
        {
            ViewBag.StoreId = storeId;
            List<string> categoriesList = new List<string>();
            var categories= categoryRepository.GetCategoriesByStoreType(storeId).ToList();
            foreach(var category in categories)
            {
                categoriesList.Add(category.CategoryName);
            }

            ViewBag.CategoriesList = categoriesList;
            
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public IActionResult CreateProduct(CreateProductViewModel createProductViewModel)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                
                if (createProductViewModel.Photo != null)
                {
                    //get phisical path of wwwroot, tj images folder:

                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + createProductViewModel.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    createProductViewModel.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                
              var categoryCheckFromDB=  categoryRepository.GetCategoriesByStoreType(createProductViewModel.StoreType).FirstOrDefault(x => x.CategoryName == createProductViewModel.CategoryName);

                //add new Category

                if (categoryCheckFromDB == null)
                {
                    Category newCategory = new Category
                    {
                        CategoryName = createProductViewModel.CategoryName,
                        StoreType=createProductViewModel.StoreType

                    };

                    categoryRepository.CreateCategory(newCategory);

                    //add new product

                   var category= categoryRepository.GetCategoriesByStoreType(createProductViewModel.StoreType).FirstOrDefault(x => x.CategoryName == createProductViewModel.CategoryName);

                    Product newProduct = new Product
                    {
                        Name = createProductViewModel.ProductName,
                        QuantityOfProduct = createProductViewModel.QuantityOfProduct,
                        Price = createProductViewModel.Price,
                        Producer = createProductViewModel.Producer,
                        ProductDescription = createProductViewModel.ProductDescription,
                        Place = createProductViewModel.Place,
                        CurrentPrice = createProductViewModel.CurrentPrice,
                        ImageSource = "~/images/" + uniqueFileName,
                        StoreType = createProductViewModel.StoreType,
                        CategoryType = category.CategoryId
    
                     };

                    productRepository.AddProduct(newProduct);
                }
                else if(categoryCheckFromDB != null)
                {
                    var categoryId = categoryCheckFromDB.CategoryId;
                    Product newProduct = new Product
                    {
                        Name = createProductViewModel.ProductName,
                        QuantityOfProduct = createProductViewModel.QuantityOfProduct,
                        Price = createProductViewModel.Price,
                        Producer = createProductViewModel.Producer,
                        ProductDescription = createProductViewModel.ProductDescription,
                        Place = createProductViewModel.Place,
                        CurrentPrice = createProductViewModel.CurrentPrice,
                        ImageSource = "~/images/" + uniqueFileName,
                        StoreType = createProductViewModel.StoreType,
                        CategoryType = categoryId

                    };

                    productRepository.AddProduct(newProduct);

                }

                return RedirectToAction("GetProductAndCategoriesByStoreId","home",new {storeId=createProductViewModel.StoreType});

            }
            return View();

        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public IActionResult EditProduct(string productId)
        {
          var productFromDB=  productRepository.GetProductById(Convert.ToInt32(productId));
            //EditProductViewModel editProductViewModel = new EditProductViewModel();

            if (productFromDB != null)
            {
                // editProductViewModel = new EditProductViewModel
                //{
                //    ProductId=productFromDB.ProductId,
                //    ProductName=productFromDB.Name,
                //    Price=productFromDB.Price,
                //    Place=productFromDB.Place,
                //    Producer=productFromDB.Producer,
                //    ProductDescription=productFromDB.ProductDescription,
                //    CurrentPrice=productFromDB.CurrentPrice,
                //    QuantityOfProduct=productFromDB.QuantityOfProduct,
                //    TotalQuantity_Kg=productFromDB.TotalQuantity_Kg,
                //    ImageSource=productFromDB.ImageSource,
                //    ProductKey=productFromDB.ProductKey,
                //    CategoryType=productFromDB.CategoryType,
                //    StoreType=productFromDB.StoreType

                //};

                return View(productFromDB);
            }
            else
            {
                return View("NotFound");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public IActionResult EditProduct(Product newProduct)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                string uniqueFilePath = null;

                if (newProduct.Photo != null)
                {
                    //get phisical path of wwwroot, tj images folder:

                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + newProduct.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    newProduct.Photo.CopyTo(new FileStream(filePath, FileMode.Create));

                    uniqueFilePath = "~/images/" + uniqueFileName;

                    newProduct.ImageSource = uniqueFilePath;
                }

                //Product editProduct = new Product
                //{
                //    Name=editProductViewModel.ProductName,
                //    Place=editProductViewModel.Place,
                //    Price=editProductViewModel.Price,
                //    CurrentPrice=editProductViewModel.CurrentPrice,
                //    Producer=editProductViewModel.Producer,
                //    ProductDescription=editProductViewModel.ProductDescription,
                //    QuantityOfProduct=editProductViewModel.QuantityOfProduct,
                //    TotalQuantity_Kg=editProductViewModel.TotalQuantity_Kg,
                //    ProductId=editProductViewModel.ProductId,
                //    ProductKey=editProductViewModel.ProductKey,
                //    CategoryType=editProductViewModel.CategoryType,
                //    StoreType=editProductViewModel.StoreType,
                //    ImageSource= uniqueFilePath ?? productFromDB.ImageSource

                //};

               productRepository.UpdateProduct(newProduct);

                return RedirectToAction("GetProductAndCategoriesByStoreId", "home", new { storeId = newProduct.StoreType });


            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public IActionResult DeleteProduct(string productId)
        {
            
           var deletedProduct= productRepository.DeleteProduct(Convert.ToInt32(productId));
            if (deletedProduct == null)
            {
                return View("NotFound");
            }

            return RedirectToAction("GetProductAndCategoriesByStoreId", "home", new { storeId = deletedProduct.StoreType });
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public IActionResult ListOfCategories(string storeId)
        {
           var categoryList = categoryRepository.GetCategoriesByStoreType(Convert.ToInt32(storeId));
            ViewBag.StoreId = storeId;
           return View(categoryList);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public IActionResult DeleteCategory(string categoryId)
        {
           var categoryForDelete= categoryRepository.DeleteCategory(Convert.ToInt32(categoryId));

            if (categoryForDelete == null)
            {
                return View("NotFound");
            }


            return RedirectToAction("ListOfCategories", new { storeId = categoryForDelete.StoreType });
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public IActionResult EditCategory(string categoryId)
        {
            var categoryFromDB = categoryRepository.GetCategoryById(Convert.ToInt32(categoryId));
            //EditProductViewModel editProductViewModel = new EditProductViewModel();

            if (categoryFromDB != null)
            {
                return View(categoryFromDB);
            }
            else
            {
                return View("NotFound");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public IActionResult EditCategory(Category newCategory)
        {
            if (ModelState.IsValid)
            {
                
                categoryRepository.UpdateCategory(newCategory);

                return RedirectToAction("ListOfCategories", new { storeId = newCategory.StoreType });


            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateCategory(string storeId)
        {
            if (storeId == null)
            {
                return View("NotFound");
            }

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var userId = user.Id;
            
            var createdUserId = storeRepository.GetStoreById(Convert.ToInt32(storeId)).CreatedByUserId;
            if(userId != createdUserId)
             {
                return View("NotFound");
             }

            ViewBag.StoreId = storeId;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public IActionResult CreateCategory(CreateCategoryViewModel createCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category
                {
                    CategoryName=createCategoryViewModel.CategoryName,
                    StoreType=createCategoryViewModel.StoreType
                };

               var categoryInserted= categoryRepository.AddCategory(category);

                if (categoryInserted == null)
                {
                    return View("Error");
                }

                return RedirectToAction("ListOfCategories", new { storeId = categoryInserted.StoreType });
            }
            
            return View();
        }

        [HttpGet]
        
        public IActionResult GoToChat(string storeId)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetListOfSenderOfMessages(string receiverName)
        {

            var listOfAllMessageWhoAreSentToReceiver = chatMessageRepository.GetListOfAllMessages().Where(x => x.ToWhom__IsSending == receiverName).ToList();

            Dictionary<string, string> dictionaryOfIdAndNamePairs = new Dictionary<string, string>();
            Dictionary<string, string> dictionaryOfIdAndNamePairsWithUnreadMessages = new Dictionary<string, string>();

            if (listOfAllMessageWhoAreSentToReceiver.Count > 0)
            {
                foreach (var chatMessage in listOfAllMessageWhoAreSentToReceiver)
                {
                    if (!dictionaryOfIdAndNamePairs.ContainsKey(chatMessage.Who_IsSending))
                    {
                        dictionaryOfIdAndNamePairs.Add(chatMessage.Who_IsSending, (await userManager.FindByIdAsync(chatMessage.Who_IsSending)).UserName);

                    }

                }

                foreach (var keyValuePair in dictionaryOfIdAndNamePairs)
                {
                    var count = listOfAllMessageWhoAreSentToReceiver.Where(x => x.Who_IsSending == keyValuePair.Key && x.Status == false).Count();
                    if (count > 0)
                    {

                        dictionaryOfIdAndNamePairsWithUnreadMessages.Add(keyValuePair.Key, (await userManager.FindByIdAsync(keyValuePair.Key)).UserName + ":" + count.ToString());
                    }
                    else
                    {
                        dictionaryOfIdAndNamePairsWithUnreadMessages.Add(keyValuePair.Key, (await userManager.FindByIdAsync(keyValuePair.Key)).UserName + ":");
                    }
                    
                }
            }

            if (dictionaryOfIdAndNamePairsWithUnreadMessages.Count > 0)
            {
                return Json(dictionaryOfIdAndNamePairsWithUnreadMessages);
            }
            return Json("There's no contacts!");
        }


        [HttpPost]
        public IActionResult GetListOfUnreadMessagesBySenderNameAndReceiverName(string senderName, string receiverName)
        {
            var listOfReadedMessage = new List<ChatMessage>();

            var SvePorukeIzmedjuOdredjenaDvaUcesnika = chatMessageRepository.GetListOfAllMessages().Where(x => (x.Who_IsSending == senderName && x.ToWhom__IsSending == receiverName) || (x.Who_IsSending == receiverName && x.ToWhom__IsSending == senderName)).OrderByDescending(x => x.DateOfSending).ToList();

            var svePorukeSaStatusomNulaIUpuceneReceiveru = SvePorukeIzmedjuOdredjenaDvaUcesnika.Where(x => x.ToWhom__IsSending == receiverName && x.Status == false).OrderByDescending(x=>x.DateOfSending).ToList();

            var sveProcitanePorukePosleNeporcitanih = new List<ChatMessage>();
            if (svePorukeSaStatusomNulaIUpuceneReceiveru.Count != 0)
            {

                DateTime dateForReadingOlderReadedMessages= svePorukeSaStatusomNulaIUpuceneReceiveru.Last().DateOfSending;

                foreach (var unreadMessage in svePorukeSaStatusomNulaIUpuceneReceiveru)
                {
                    unreadMessage.Status = true;
                    chatMessageRepository.UpdateMessageByStatus(unreadMessage);
                }

                sveProcitanePorukePosleNeporcitanih = SvePorukeIzmedjuOdredjenaDvaUcesnika.Where(x => x.DateOfSending < dateForReadingOlderReadedMessages).OrderByDescending(x=>x.DateOfSending).ToList();

                if (sveProcitanePorukePosleNeporcitanih.Count() < 5)
                {
                    foreach (var procitanaPoruke in sveProcitanePorukePosleNeporcitanih)
                    {
                        svePorukeSaStatusomNulaIUpuceneReceiveru.Add(procitanaPoruke);
                    }
                }
                else
                {
                    foreach (var procitanaPoruke in sveProcitanePorukePosleNeporcitanih.Take(5))
                    {
                        svePorukeSaStatusomNulaIUpuceneReceiveru.Add(procitanaPoruke);
                    }
                }

            }

            else
            {
                if (SvePorukeIzmedjuOdredjenaDvaUcesnika.Count() < 10)
                {
                    svePorukeSaStatusomNulaIUpuceneReceiveru = SvePorukeIzmedjuOdredjenaDvaUcesnika.ToList();
                }
                else
                {
                    svePorukeSaStatusomNulaIUpuceneReceiveru = SvePorukeIzmedjuOdredjenaDvaUcesnika.Take(10).ToList();
                }
            }

            svePorukeSaStatusomNulaIUpuceneReceiveru = svePorukeSaStatusomNulaIUpuceneReceiveru.OrderBy(x => x.DateOfSending).ToList();

            return Json(svePorukeSaStatusomNulaIUpuceneReceiveru);
        }

        [HttpPost]
        public async Task<IActionResult> GetSenderUserNameById(string senderId)
        {
            var senderUserName = (await userManager.FindByIdAsync(senderId)).UserName;
            return Json(senderUserName);
        }

        [HttpPost]
        public async Task<IActionResult> GetTopTenMessages(string senderId, string receiverName)
        {
            var receieverId = (await userManager.FindByNameAsync(receiverName)).Id;

            List<ChatMessage> top10Messages = null;

            if (chatMessageRepository.GetListOfAllMessages().Where(x => (x.Who_IsSending == senderId && x.ToWhom__IsSending == receieverId) || (x.Who_IsSending == receieverId && x.ToWhom__IsSending == senderId)).Count() < 10)
            {
              top10Messages= chatMessageRepository.GetListOfAllMessages().Where(x => (x.Who_IsSending == senderId && x.ToWhom__IsSending == receieverId) || (x.Who_IsSending == receieverId && x.ToWhom__IsSending == senderId)).OrderByDescending(x => x.DateOfSending).ToList();
            }
            else
            {
              top10Messages= chatMessageRepository.GetListOfAllMessages().Where(x => (x.Who_IsSending == senderId && x.ToWhom__IsSending == receieverId) || (x.Who_IsSending == receieverId && x.ToWhom__IsSending == senderId)).OrderByDescending(x => x.DateOfSending).Take(10).ToList();
            }

            top10Messages = top10Messages.OrderBy(x=>x.DateOfSending).ToList();

            return Json(top10Messages);
        }


        [HttpPost]
        public async Task<IActionResult> ChangeStatusWhenSeeMessage(string senderName, string receiverId)
        {

            var senderId = (await userManager.FindByNameAsync(senderName)).Id;

           var messagesForChangeStatus= chatMessageRepository.GetListOfChatMessageByUnreadMessage(receiverId, false).Where(x => x.Who_IsSending == senderId).ToList();
            foreach(var chatMessage in messagesForChangeStatus)
            {
                chatMessage.Status = true;
                chatMessageRepository.UpdateMessageByStatus(chatMessage);
            }

            return Json("true");
        }

    }
}
