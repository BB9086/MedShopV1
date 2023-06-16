using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MedShop.Models;
using MedShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AdministrationController> logger;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager,
                                        ILogger<AdministrationController> logger)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            ApplicationUser user = userManager.Users.FirstOrDefault(x => x.Id == id);
            //or: userManager.FindByIdAsync(id);
            if (user == null)
            {
                return View("NotFound");
            }
            var userClaims = await userManager.GetClaimsAsync(user);
            //var userRoles = await userManager.GetRolesAsync(user);
            var model = new EditUserViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id,
                Claims = userClaims.Select(x => x.Value).ToList()
                //Roles=userRoles
            };

            foreach (var role in roleManager.Roles.ToList())
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Roles.Add(role.Name);
                }
            }


            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);

                if (user != null)
                {
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListUsers");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    return View("NotFound");
                }

            }
            return View(model);

        }


        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = model.Name
                };
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }


            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public IActionResult ListClaims()
        {
            var claims = ClaimsStore.AllClaims;
            return View(claims);
        }

        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return RedirectToAction("ListRoles");
            }
            var model = new EditRoleViewModel()
            {
                Id = role.Id,
                Name = role.Name

            };
            foreach (var user in userManager.Users.ToList())
            {
                var result = await userManager.IsInRoleAsync(user, role.Name);
                if (result)
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy ="EditRolePolicy")]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(model.Id);

                if (role != null)
                {
                    role.Name = model.Name;
                    var result = await roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    return View("NotFound");
                }

            }
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users.ToList())
            {
                UserRoleViewModel userRoleViewModel = new UserRoleViewModel()
                {

                    UserId = user.Id,
                    UserName = user.UserName


                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(string roleId, List<UserRoleViewModel> model)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return View("NotFound");
            }

            IdentityResult result = null;
            foreach (var userRoleViewModel in model)
            {
                ApplicationUser user = await userManager.FindByIdAsync(userRoleViewModel.UserId);
                if (userRoleViewModel.IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!userRoleViewModel.IsSelected && (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else if (!userRoleViewModel.IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    continue;
                }

                else if (userRoleViewModel.IsSelected && (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    continue;
                }
                else
                {
                    ModelState.AddModelError("", "Can not add or remove this specific user to this role");
                    return View();
                }
                
            }
            return RedirectToAction("EditRole", new { id = roleId });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("ListUsers");

                }
            }
            else
            {
                return View("NotFound");
            }
        }

        [HttpPost]
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<ActionResult> DeleteRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role != null)
            {
                try
                {
                    var result = await roleManager.DeleteAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View("ListRoles");

                    }
                }
                catch (DbUpdateException ex)
                {
                    logger.LogError($"Error deleting role {ex}");

                    ViewBag.ErrorTitle = $"{role.Name} role is in use";
                    ViewBag.ErrorMessage = $"{role.Name} role can not be deleted as there are users in this role." +
                        $" If you want to delete this role, please remove the users from" +
                        $" this role and then try to delete";

                    return View("Error");
                }
            }
            else
            {
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<ActionResult> ManageUserRoles(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return View("NotFound");
            }
            else
            {

                var roles = roleManager.Roles.ToList();

                List<ManageUserRolesViewModel> model = new List<ManageUserRolesViewModel>();

                foreach(var role in roles)
                {
                    ManageUserRolesViewModel mod = new ManageUserRolesViewModel();
                    mod.RoleId = role.Id;
                    mod.RoleName = role.Name;
                    if(await userManager.IsInRoleAsync(user, role.Name))
                    {
                        mod.IsSelected = true;
                    }
                    else
                    {
                        mod.IsSelected = false;
                    }

                    model.Add(mod);
                }

                ViewBag.UserId = user.Id;


                return View(model);
            }

         }
        [HttpPost]
        public async Task<ActionResult> ManageUserRoles(string id, List<ManageUserRolesViewModel> model)
        {
           
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return View("NotFound");
            }
            else
            {
                IdentityResult result = null;

                foreach(var manageUserRolesmodel in model)
                {
                    if(manageUserRolesmodel.IsSelected && !(await userManager.IsInRoleAsync(user, manageUserRolesmodel.RoleName)))
                    {
                      result=  await userManager.AddToRoleAsync(user, manageUserRolesmodel.RoleName);
                    }
                    else if(!manageUserRolesmodel.IsSelected && (await userManager.IsInRoleAsync(user, manageUserRolesmodel.RoleName)))
                    {
                        
                        result = await userManager.RemoveFromRoleAsync(user, manageUserRolesmodel.RoleName);
                    }
                    else if (!manageUserRolesmodel.IsSelected && !(await userManager.IsInRoleAsync(user, manageUserRolesmodel.RoleName)))
                    {
                        continue;
                    }
                    else if (manageUserRolesmodel.IsSelected && (await userManager.IsInRoleAsync(user, manageUserRolesmodel.RoleName)))
                    {
                        continue;
                    }

                    if (result.Succeeded)
                    {
                        continue;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Can not add or remove this specific role to this user");
                        return View();
                    }
                }
               
                return RedirectToAction("EditUser",new { id=user.Id});
            }
            
        }

        [HttpGet]
        public async Task<ActionResult> ManageUserClaims(string id)
        {
            ViewBag.UserId = id;
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return View("NotFound");
            }
           
                List<UserClaimsViewModel> modelList = new List<UserClaimsViewModel>();
                var existingUserClaims =await userManager.GetClaimsAsync(user);

                foreach(var claim in ClaimsStore.AllClaims)
                {
                    var model = new UserClaimsViewModel
                    {
                        ClaimType = claim.Type
                    };

                    if (existingUserClaims.Any(x=>x.Type==claim.Type))
                    {
                        model.IsSelected = true;
                    }
                    else
                    {
                        model.IsSelected = false;
                    }

                    modelList.Add(model);
                }
            


            return View(modelList);

        }

        [HttpPost]
        public async Task<ActionResult> ManageUserClaims(string id, List<UserClaimsViewModel> model)
        {
            
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return View("NotFound");
            }

            
            var existingUserClaims = await userManager.GetClaimsAsync(user);
            var result= await userManager.RemoveClaimsAsync(user, existingUserClaims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "can not remove user existing claims");
                return View(model);
            }

           result= await userManager.AddClaimsAsync(user, model.Where(x => x.IsSelected == true).Select(x=>new Claim(x.ClaimType,x.ClaimType)));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "can not add selected claims to specified user");
                return View(model);
            }


            return RedirectToAction("EditUser", new { id=id});

        }
    }


}
