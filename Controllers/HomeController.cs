using OnLineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OnLineShop.Controllers
{
    public class HomeController : Controller
    {
        OnlineShopDBEntities db = new OnlineShopDBEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View(db.tb_Categories.ToList());
        }

        [HttpGet]
        public ActionResult Product(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_products Product = db.tb_products.Find(id);
            if (Product == null)
            {
                return HttpNotFound();
            }

            return View(Product);
        }

        [HttpGet]
        public ActionResult Category(string name)
        {
            if (name == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string CatName = name.Replace("-", " ");
            var Category = db.tb_Categories.Where(a => a.cat_name == CatName).ToList();
            if (Category == null)
            {
                return HttpNotFound();
            }

            return View(Category);
        }

        [HttpGet]
        public ActionResult brand(string name)
        {
            if (name == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string brandname = name.Replace("-", " ");
            var Brand = db.tb_brands.Where(a => a.englishname == brandname).ToList();
            if (Brand == null)
            {
                return HttpNotFound();
            }

            return View(Brand);
        }


        [HttpGet]
        [Authorize]
        public ActionResult AddWishList(int? product_id)
        {
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddWishList(int product_id)
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            string msg = string.Empty;
            var DetailsAction = db.tb_actions.Where(a => a.user_id == DetailsUser.id && a.product_id == product_id && a.type_action == "WishList").SingleOrDefault();
            if (DetailsAction == null)
            {
                if (ModelState.IsValid)
                {
                    DateTime dateToday = DateTime.Now;
                    var action = new tb_actions { product_id = product_id, user_id = DetailsUser.id, type_action = "WishList", date = dateToday, quantity = 0 };
                    db.tb_actions.Add(action);
                    db.SaveChanges();
                    msg = "تم إضافة المنتج إلي المفضلة بنجاح";
                    return Json(msg ,JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                msg = "المنتج مضاف مسبقا إلي المفضلة";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddCart(int? product_id)
        {
            return Redirect("~/product/" + product_id);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddCart(int quantity, int product_id)
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();

            var DetailsAction = db.tb_actions.Where(a => a.user_id == DetailsUser.id && a.product_id == product_id && a.type_action == "Cart").SingleOrDefault();
            if (DetailsAction == null)
            {
                if (ModelState.IsValid)
                {
                    var action = new tb_actions();

                    var DetailsProCart = db.tb_actions.Where(a => a.product_id == product_id && a.user_id == DetailsUser.id && a.type_action == "WishList").SingleOrDefault();
                    if (DetailsProCart != null)
                    {
                        db.tb_actions.Remove(DetailsProCart);
                    }
                    action.date = DateTime.Now;
                    action.user_id = DetailsUser.id;
                    action.product_id = product_id;
                    action.quantity = quantity;
                    action.type_action = "Cart";
                    db.tb_actions.Add(action);
                    db.SaveChanges();
                }
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        

        [HttpGet]
        public ActionResult Search(string searchName, int? CatID)
        {
            string keyword = searchName.Replace("-", " ");


            if (CatID == 0)
            {
                var result = db.tb_products.Where(a => a.title.Contains(keyword)
                            || a.description.Contains(keyword)).ToList();
                return View(result);
            }
            else
            {
                var result = db.tb_products.Where(a =>  (a.title.Contains(keyword) && a.category_id == CatID)
                             || (a.description.Contains(keyword) && a.category_id == CatID)).ToList();
                return View(result);
            }
        }

    }
}