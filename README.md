# MedShop - ASP.NET Core MVC, ASP.NET Core Web API, SignalR, JQuery, Bootstrap, SQL

**About this app:** This application is developed for demonstration purposes and enables several different producers to sell their products to customers. The user can be authorized as an administrator, manager or customer. Accordingly, if the user is an administrator, he can create store managers, while managers can create stores and add different products to them. JWT authentication is implemented. Users can log in using OAuth2.0 with credentials from external authentication providers (Facebook, Google). The application also provides the possibility of chatting (using SignalR) between the store manager and the customer. Entity Framework Code First Approach was used to interact with the SQL Server database. The Web API enables communication with the client application that displays incoming orders to the store manager. NLog library is used to store log messages. 

<!--- ## URL
###### https://medshopsite.azurewebsites.net/

## Test credentials

#### Customer:
###### Email: user01@gmail.com 
###### Password: Test1234!

#### Manager: 
###### Email: manager01@gmail.com 
###### Password: Test1234!

#### Administrator:
###### Email: admin@gmail.com 
###### Password: Test1234! -->

## Application screenshots

###### Main Page

![img1](https://user-images.githubusercontent.com/118169200/220606323-11e98704-3277-40f0-b89d-510f1b250844.png)


###### Local Account and External Login

![img6](https://user-images.githubusercontent.com/118169200/220545658-8c93924b-b7c4-4612-aee3-f0c8ccf38ee7.png)

###### Register Page

![img6](https://user-images.githubusercontent.com/118169200/220546542-83a15e7c-79bf-4792-ac32-b6b86ac84afa.png)

###### Forgot Password Page

![img6](https://user-images.githubusercontent.com/118169200/220548090-1ff1df62-f255-4b09-9639-fa037f9dcf2a.png)

###### Our Products

![products](https://user-images.githubusercontent.com/118169200/220606625-a10b8ae9-9992-4edd-a539-4e0bee16282b.png)

###### Product Details

![img6](https://user-images.githubusercontent.com/118169200/220551308-3c52d771-6fbe-4899-b110-8f4ff1bcc0cb.png)

###### Shopping Cart
![img6](https://user-images.githubusercontent.com/118169200/220552028-e13f9111-4491-4ea7-8b06-9b2003a07f30.png)

###### Shopping Cart - Details
![img6](https://user-images.githubusercontent.com/118169200/220552570-cf9ea086-994b-42c1-91d2-421dd177364a.png)

###### Reservation Page (Pickup or Delivery)
![reserv](https://github.com/BB9086/MedShop/assets/118169200/e37d35e8-2a33-45c0-8b83-e86ee89614a1)

###### Customer Data
![podkup](https://github.com/BB9086/MedShop/assets/118169200/fb6cf695-7696-44c8-9c78-eaf54d77ad47)

###### Order Details
![detalji](https://github.com/BB9086/MedShop/assets/118169200/e553f81d-9959-4d51-a4b1-e035f1ecc70d)

###### Chat 
![Chat](https://user-images.githubusercontent.com/118169200/220607502-ebf1cda9-f822-46ed-bb3b-c5d6c66d9442.png)

#### Administrator
If the user is an administrator, he has the ability to add new users, to add a certain role to them and to create actions.

###### Add new users
![admin01](https://user-images.githubusercontent.com/118169200/220608752-456a97b8-71f6-4a7b-9907-9411011eb917.png)

###### User data change
![admin02](https://user-images.githubusercontent.com/118169200/220609100-ef6adbd9-ee6f-4119-bd1e-277bb815326d.png)

###### Add new role or edit existing ones
![admin03](https://user-images.githubusercontent.com/118169200/220610069-385a3cbf-6712-4875-8fda-6ad7765d39b9.png)

###### Adding a new store or edit existing ones
![admin04](https://user-images.githubusercontent.com/118169200/220610494-65e8798d-be4f-4aa0-b9f8-4023d4744e70.png)

###### Adding a new store or edit existing ones
![admin05](https://user-images.githubusercontent.com/118169200/220610520-ec81dac7-5897-4785-9509-6db29bbac984.png)

#### Manager
If the user is manager, he has the ability to add new product categories or edit existing ones and to add a new products.

######  Adding a new product category
![manager01](https://user-images.githubusercontent.com/118169200/220613231-79ed826e-90ef-4358-9991-515efc3a25bd.png)

###### Adding a new product category
![manager02](https://user-images.githubusercontent.com/118169200/220613238-05151512-56fc-4443-8313-2e2a41fba33a.png)

###### Adding a new product
![manager04](https://user-images.githubusercontent.com/118169200/220613247-6205967c-8d40-4d46-872f-393bea3b042c.png)

## ASP.NET Core web API documentation with Swagger

<!--- ###### URL: https://medshopsite.azurewebsites.net/swagger/index.html -->

###### Web API

![swagger](https://user-images.githubusercontent.com/118169200/220547262-b679e318-3dfe-43e8-9c80-60068b17a1bc.png)








