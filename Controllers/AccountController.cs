using OnLineShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnLineShop.Controllers
{
    public class AccountController : Controller
    {
        OnlineShopDBEntities db = new OnlineShopDBEntities();

        [HttpGet]
        [Authorize]
        public ActionResult profile()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشتري")
            {
                return View(DetailsUser);
            }
            else
            {
                return RedirectToAction("Index", "ControlPanal");
            }
        }


        [HttpGet]
        public ActionResult Register()
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.type = new SelectList(db.tb_roles.Where(a => !a.name.Contains("مشرف")), "Name", "Name");
                return View();
            }
        }

        [HttpPost]
        public ActionResult Register(tb_Users model)
        {
            ViewBag.type = new SelectList(db.tb_roles.Where(a => !a.name.Contains("مشرف")), "Name", "Name");
            if (ModelState.IsValid)
            {
                var user = new tb_Users { username = model.username, email = model.email, type = model.type, password = model.password, fullname = model.fullname };
                db.tb_Users.Add(user);
                db.SaveChanges();
                FormsAuthentication.SetAuthCookie(model.username, false);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.Name != "")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.type = new SelectList(db.tb_roles.Where(a => !a.name.Contains("مشرف")), "Name", "Name");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(tb_Users model, string ReturnUrl)
        {
            var UserLogin = db.tb_Users.Where(x => x.username == model.username && x.password==model.password).FirstOrDefault();
            if (UserLogin == null)
            {
                ViewBag.Message = "البيانات غير صحيحة";
                return View(model);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(UserLogin.username,false);
                if (ReturnUrl == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                return Redirect(ReturnUrl);
            }
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpGet]
        [Authorize]
        public ActionResult wishlist()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            var wishlists = db.tb_actions.Where(a => a.user_id == DetailsUser.id && a.type_action == "WishList").ToList();


            return View(wishlists);
        }

        [HttpGet]
        [Authorize]
        public ActionResult shopping_cart()
        {
            var UserName = User.Identity.AuthenticationType;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            var shopping_cart = db.tb_actions.Where(a => a.user_id == DetailsUser.id && a.type_action == "Cart").ToList();

            return View(shopping_cart);
        }

        [HttpGet]
        [Authorize]
        public ActionResult settings()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            

            return View(DetailsUser);
        }

        [HttpPost]
        [Authorize]
        public ActionResult settings(tb_Users Profile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Profile).State = EntityState.Modified;
                db.SaveChanges();
                FormsAuthentication.SetAuthCookie(Profile.username, false);
                return RedirectToAction("settings", "Account");

            }
            return View(Profile);
        }

        
        [Authorize]
        public ActionResult BillInfo()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            var billinfo = db.tb_billinfo.Where(a => a.user_id == DetailsUser.id).ToList();

            return View(billinfo);
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddBillInfo()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();

            var ShipUser = db.tb_billinfo.Where(a => a.user_id == DetailsUser.id).SingleOrDefault();
            if (ShipUser != null)
            {
                Redirect("EditBill/" + ShipUser.id);
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddBillInfo(tb_billinfo model)
        {
            if (ModelState.IsValid)
            {
                var UserName = User.Identity.Name;
                var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();

                var BillInfo = new tb_billinfo { address = model.address, email = model.email, fullname = model.fullname, phone = model.phone, postal_code = model.postal_code, region = model.region, country = model.country, city = model.city, company_name = model.company_name, user_id = DetailsUser.id };
                db.tb_billinfo.Add(BillInfo);
                db.SaveChanges();
                return RedirectToAction("Profile", "Account");
            }
            return View(model);
        }


        [Authorize]
        public ActionResult EditBill(int? id)
        {
            tb_billinfo BillUser = db.tb_billinfo.Find(id);
            return View(BillUser);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditBill(tb_billinfo model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("BillInfo", "Account");
            }
            return View(model);
        }


        [Authorize]
        public ActionResult DeleteBill(int? id)
        {
            tb_billinfo BillUser = db.tb_billinfo.Find(id);
            db.tb_billinfo.Remove(BillUser);
            db.SaveChanges();
            return RedirectToAction("BillInfo", "Account");
        }


        [Authorize]
        public ActionResult ShipInfo()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            var ship = db.tb_ship.Where(a => a.user_id == DetailsUser.id).ToList();

            return View(ship);
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddShipInfo()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();

            var ShipUser = db.tb_ship.Where(a => a.user_id == DetailsUser.id).SingleOrDefault();
            if (ShipUser !=null)
            {
                return Redirect("EditShip/"+ ShipUser.id);

            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddShipInfo(tb_ship model)
        {
            if (ModelState.IsValid)
            {
                var UserName = User.Identity.Name;
                var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();

                var ShipInfo = new tb_ship { address = model.address, fullname = model.fullname, phone = model.phone, postal_code = model.postal_code, region = model.region, country = model.country, company_name = model.company_name, city = model.city, user_id = DetailsUser.id };
                db.tb_ship.Add(ShipInfo);
                db.SaveChanges();
                return RedirectToAction("Profile", "Account");
            }
            return View(model);
        }

        [Authorize]
        public ActionResult EditShip(int? id)
        {
            tb_ship ShipUser = db.tb_ship.Find(id);
            return View(ShipUser);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditShip(tb_ship model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ShipInfo", "Account");
            }
            return View(model);
        }

        [Authorize]
        public ActionResult DeleteShip(int? id)
        {
            tb_ship ShipUser = db.tb_ship.Find(id);
            db.tb_ship.Remove(ShipUser);
            db.SaveChanges();
            return RedirectToAction("ShipInfo", "Account");
        }

        [HttpGet]
        [Authorize]
        public ActionResult DeleteProductWish(int id)
        {
            tb_actions Action = db.tb_actions.Find(id);
            db.tb_actions.Remove(Action);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpGet]
        [Authorize]
        public ActionResult DeleteProductCat(int id)
        {
            tb_actions Action = db.tb_actions.Find(id);
            db.tb_actions.Remove(Action);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditCartQuantity(int id , int quantity)
        {
            if (ModelState.IsValid)
            {
                tb_actions Action = db.tb_actions.Find(id);
                Action.quantity = quantity;
                db.SaveChanges();
                
            }
            return RedirectToAction("shopping_cart", "Account");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Checkout(tb_checkout model)
        {
            if (ModelState.IsValid)
            {
                decimal totalPrice =0 ;
                DateTime date = DateTime.Now;
                
                var ActionDetails = db.tb_actions.Where(a => a.user_id == model.user_id && a.type_action == "Cart").ToList();
               
                string check_code = model.user_id + +model.ship_id + DateTime.Now.ToString("ddMMyyhhmmss");
                string type = "لم تشحن بعد";

                var AddCheckout = new tb_checkout { date = date, total_price = 0, bill_id = model.bill_id, ship_id = model.ship_id, user_id = model.user_id, check_code = check_code, type = type };
                db.tb_checkout.Add(AddCheckout);
                db.SaveChanges();

                foreach (var item in ActionDetails)
                {
                    var orderDetails = new tb_orderDetails { check_id = AddCheckout.id, user_id = item.user_id, product_id = item.product_id, quantity = item.quantity};
                    db.tb_orderDetails.Add(orderDetails);

                    var productQ = db.tb_products.Where(a => a.id == item.product_id).SingleOrDefault();
                    int lastQ = (int)(productQ.quantity - item.quantity);

                    tb_products product = db.tb_products.Find(item.product_id);
                    product.quantity = lastQ;
                    db.SaveChanges();

                    totalPrice += (decimal)(item.quantity * item.tb_products.price);
                    
                }

                tb_checkout check = db.tb_checkout.Where(a => a.id == AddCheckout.id).SingleOrDefault(); ;
                check.total_price = totalPrice;
                db.Entry(check).State = EntityState.Modified;
                
                db.SaveChanges();
                db.DeleteActionCart( "Cart" , model.user_id);

                db.SaveChanges();
                return RedirectToAction("shopping_cart", "Account");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult purchases()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            var checkout = db.tb_checkout.Where(a => a.user_id == DetailsUser.id).ToList();

            return View(checkout);
        }


        [HttpGet]
        [Authorize]
        public ActionResult returnProduct(int productId, int purchID, int userID)
        {
            var orderDetails = db.tb_orderDetails.Where(a => a.check_id == purchID && a.product_id == productId && a.user_id == userID).SingleOrDefault();
            return View(orderDetails);
        }

        [HttpPost]
        [Authorize]
        public ActionResult returnProduct(int productId, int purchID, string text, int quantity, int userID)
        {
            int user_id = userID;
            DateTime date = DateTime.Now;

            var orderDetails = db.tb_orderDetails.Where(a => a.check_id == purchID).ToList();

            if (orderDetails.Count == 1)
            {
                tb_checkout checkout = db.tb_checkout.Find(purchID);
                checkout.type = "مرتجع";
                db.SaveChanges();
            }
            var actionDetails = db.tb_orderDetails.Where(a => a.product_id == productId && a.check_id == purchID).SingleOrDefault();
            tb_orderDetails action = db.tb_orderDetails.Find(actionDetails.id);
            action.type = "مرتجع";

            var reterned = new tb_reterned { date = date, product_id = productId, user_id = userID, quantity = quantity, check_id = purchID, text = text };
            db.tb_reterned.Add(reterned);

            db.SaveChanges();
            return RedirectToAction("purchases");
        }

        [HttpPost]
        [Authorize]
        public ActionResult NonShip(int productId, int purchID)
        {

            var orderDetails = db.tb_orderDetails.Where(a => a.check_id == purchID).ToList();

            if (orderDetails.Count == 1)
            {
                tb_checkout checkout = db.tb_checkout.Find(purchID);
                db.tb_checkout.Remove(checkout);
            }
            var actionDetails = db.tb_orderDetails.Where(a => a.product_id == productId && a.check_id == purchID).SingleOrDefault();
            tb_orderDetails action = db.tb_orderDetails.Find(actionDetails.id);
            db.tb_orderDetails.Remove(action);


            db.SaveChanges();
            return RedirectToAction("purchases");
        }

}
}