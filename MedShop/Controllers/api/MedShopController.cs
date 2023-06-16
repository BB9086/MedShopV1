using AutoMapper;
using MedShopWebAPi.Data;
using MedShopWebAPi.Dtos;
using MedShopWebAPi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MedShop.Dtos;
using MedShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MedShopWebAPi.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class MedShopController : ControllerBase
    {
        private readonly IMedShopRepo _repository;
        public IMapper _mapper { get; }

        public MedShopController(IMedShopRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        //Get api/v1/medshop/getallorders
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetAllOrders()
        {
            var orders = _repository.GetAllOrders();
            var customers = _repository.GetAllCustomers();

            //return Ok(_mapper.Map<IEnumerable<OrderReadDto>>(orders);

            var result = orders.Join(customers, t1 => t1.CustomerId, t2 => t2.CustomerId, (t1, t2) =>
            {
                var t3 = _mapper.Map<Order, OrderReadDto>(t1);
                t3 = _mapper.Map<Customer, OrderReadDto>(t2, t3);

                return t3;
            });

            return Ok(result);

        }

        //Get api/v1/medshop/getallordersforstore/{storeId}
        [HttpGet]
        [Route("{storeId}")]
        [Authorize]
        public ActionResult<IEnumerable<Order>> GetAllOrdersForStore(int storeId)
        {
            var orders = _repository.GetAllOrdersForStore(storeId).ToList();

            List<int> customerIds = new List<int>();

            foreach (var item in orders)
            {
                customerIds.Add(item.CustomerId);
            }

            var customers = _repository.GetAllCustomers(customerIds);

            var result = orders.Join(customers, t1 => t1.CustomerId, t2 => t2.CustomerId, (t1, t2) =>
            {
                var t3 = _mapper.Map<Order, OrderReadDto>(t1);
                t3 = _mapper.Map<Customer, OrderReadDto>(t2, t3);

                return t3;
            });


            var resultTop5Orders = result.OrderByDescending(x => x.CreatedOn).Take(5);

            return Ok(resultTop5Orders);

        }


        [HttpPatch]
        [Route("{orderId}/{customerEmail}")]
        [Authorize]
        public ActionResult ChangeStatusOfOrder(string orderId, string customerEmail, [FromBody] JsonPatchDocument<OrderUpdateDto> patchDoc)
        {
            string content = "";

            var orderFromRepo = _repository.GetOrderById(Convert.ToInt32(orderId));

            if (orderFromRepo == null)
            {
                return NotFound();
            }
            var orderToPatch = _mapper.Map<OrderUpdateDto>(orderFromRepo);

            patchDoc.ApplyTo(orderToPatch, ModelState);

            if (!TryValidateModel(orderToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(orderToPatch, orderFromRepo);

            _repository.UpdateOrder(orderFromRepo);

            var orderItems = _repository.GetOrderItemsByOrderID(Convert.ToInt32(orderId));

            List<string> listOfContentIds = new List<string>();

            //Link za slanje multimedijalnih poruka!!!!!!: https://www.youtube.com/watch?v=Ip7JY-9z9ro

            MailMessage mm = new MailMessage();
            mm.From = new MailAddress("yamaica95kw@gmail.com");
            mm.To.Add(customerEmail);
            if (orderFromRepo.OrderStatus == 20)
            {
                mm.Subject = "Potvrda porudžbine";
                content = "Poštovani, vaša porudžbina je potvrđena i u daljoj je obradi.Vaš MedShop!< br />";
            }
            else if (orderFromRepo.OrderStatus == 30)
            {
                mm.Subject = "Odbijanje porudžbine";
                content = "Poštovani, vaša porudžbina je odbijena zbog trenutne nemogućnosti obrade. Molimo Vas imajte razumevanja. Vaš MedShop!";
            }


            foreach (var orderItem in orderItems)
            {

                var productItem = _repository.GetAllProducts().Where(x => x.ProductId == orderItem.ProductId).FirstOrDefault();

                content += "<div style='border-top: 2px solid orange; background-color:lightgrey; width: 300px'><h4>" + productItem.Name + "</h4><p><img src=cid:" + productItem.ProductKey + " height=80 width=80 float=left margin-left=20px margin-top=-40px>Cena proizvoda: " + productItem.CurrentPrice + " , kolicina: " + orderItem.Quantity + "</p></div>";

                if (orderItem == orderItems.Last())
                {
                    if (orderFromRepo.OrderStatus == 20)
                    {
                        content += "<div style='border: 2px solid green; width: 300px'><b>Ukupno za naplatu: " + orderFromRepo.OrderTotal + "</b></div>";
                    }
                    else if (orderFromRepo.OrderStatus == 30)
                    {
                        content += "<div style='border: 2px solid red; width: 300px'><b>Narudžbina je odbijena. Pokušajte u narednom satu.</b></div>";
                    }

                }

                AlternateView imgView = AlternateView.CreateAlternateViewFromString(content, null, "text/html");
                var x = productItem.ImageSource;
                LinkedResource lR = new LinkedResource(productItem.ImageSource.Replace("~", "wwwroot"));
                lR.ContentId = productItem.ProductKey;
                imgView.LinkedResources.Add(lR);

                mm.AlternateViews.Add(imgView);
                listOfContentIds.Add(lR.ContentId);

            }

            //mm.AlternateViews.Add(imgView);

            foreach (string contentId in listOfContentIds)
            {
                mm.Body += contentId;
            }

            //mm.Body = lR.ContentId;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            NetworkCredential nc = new NetworkCredential("yamaica95kw@gmail.com", "montana9000");
            smtp.EnableSsl = true;
            smtp.Credentials = nc;
            smtp.Send(mm);

            return NoContent();

        }

        //Get api/v1/medshop/getorderbyid/{id}
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public ActionResult<Order> GetOrderById(int id)
        {
            var order = _repository.GetOrderById(id);
            var customer = _repository.GetCustomerById(order.CustomerId);

            List<Order> orders = new List<Order>();
            orders.Add(order);

            List<Customer> customers = new List<Customer>();
            customers.Add(customer);

            if (order != null)
            {
                var result = orders.Join(customers, t1 => t1.CustomerId, t2 => t2.CustomerId, (t1, t2) =>
                {
                    var t3 = _mapper.Map<Order, OrderReadDto>(t1);
                    t3 = _mapper.Map<Customer, OrderReadDto>(t2, t3);

                    return t3;
                });

                var firstListElement = result.FirstOrDefault();

                return Ok(firstListElement);

            }
            return NotFound();
        }



        //Get api/v1/medshop/getorderitemsbyid/{orderId}
        [HttpGet]
        [Route("{orderId}")]
        [Authorize]
        public ActionResult<Order> GetOrderItemsById(int orderId)
        {
            var orderItems = _repository.GetOrderItemsByOrderID(orderId);
            var products = _repository.GetAllProducts();

            var result = orderItems.Join(products, t1 => t1.ProductId, t2 => t2.ProductId, (t1, t2) =>
            {
                var t3 = _mapper.Map<OrderItem, OrderItemReadDto>(t1);
                t3 = _mapper.Map<Product, OrderItemReadDto>(t2, t3);

                return t3;
            });

            return Ok(result);
        }


        //Get api/v1/medshop/getallstoresformanager/{userName}
        //https://localhost:44336/api/v1/medshop/getallstoresformanager/manager01@gmail.com
        [HttpGet]
        [Route("{userName}")]
        [Authorize]
        public async Task<IEnumerable<Store>> GetAllStoresForManager(string userName)
        {
            IEnumerable<Store> stores = await _repository.GetAllStoresForManager(userName);

            return stores;
        }

        //Get api/v1/medshop/ismanagerindb/{userName}
        [HttpPost]
        [Route("{userName}")]
        public IsUserInDb IsManagerInDb(string userName)
        {
            IsUserInDb isUserInDb = new IsUserInDb();
            isUserInDb.UserInDb = _repository.IsManagerInDb(userName);

            //return _mapper.Map<IEnumerable<StoreReadDto>>(stores);

            return isUserInDb;
        }



    }
}
