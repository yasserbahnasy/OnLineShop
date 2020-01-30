using OnLineShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnLineShop.Controllers
{
    [Authorize]
    public class ControlPanalController : Controller
    {
        OnlineShopDBEntities db = new OnlineShopDBEntities();
        
        // GET: ControlPanal
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Categories()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {
                return View(db.tb_Categories.ToList());
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {
                return View();
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult AddCategory(tb_Categories model)
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {
                if (ModelState.IsValid)
                {
                    var Category = new tb_Categories { cat_name = model.cat_name, cat_icon = model.cat_icon, cat_desc = model.cat_desc };
                    db.tb_Categories.Add(Category);
                    db.SaveChanges();
                    return RedirectToAction("Categories", "ControlPanal");
                }
                return View(model);
            }
            else
            {
                return HttpNotFound();
            } 
        }

        public ActionResult Products()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {
                return View(db.tb_products.ToList());
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult MyProducts()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();

            var products = from pro in db.tb_products
                           where pro.user_id == DetailsUser.id
                           select pro;
            return View(products.ToList());
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {
                ViewBag.category_id = new SelectList(db.tb_Categories, "cat_id", "cat_name");
                ViewBag.brand_id = new SelectList(db.tb_brands, "id", "name");
                return View();
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(tb_products product, HttpPostedFileBase Uploadimage)
        {
            if (ModelState.IsValid)
            {
                var UserName = User.Identity.Name;
                var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();

                string filename = "Product-"+DateTime.Now.ToString("ddMMyyhhmmss") + Path.GetExtension(Uploadimage.FileName);
                string path = Path.Combine(Server.MapPath("~/uploads"), filename);
                Uploadimage.SaveAs(path);
                product.image = filename;
                product.date = DateTime.Now;
                product.user_id = DetailsUser.id;
                db.tb_products.Add(product);
                db.SaveChanges();
                return RedirectToAction("MyProducts");
            }

            ViewBag.brand_id = new SelectList(db.tb_brands, "id", "name");
            ViewBag.category_id = new SelectList(db.tb_Categories, "cat_id", "cat_name", product.category_id);
            return View(product);
        }

        [HttpGet]
        public ActionResult EditProduct(int? id)
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
            ViewBag.brand_id = new SelectList(db.tb_brands, "id", "name");
            ViewBag.category_id = new SelectList(db.tb_Categories, "cat_id", "cat_name", Product.category_id);
            return View(Product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(tb_products Product, HttpPostedFileBase Uploadimage)
        {
            if (ModelState.IsValid)
            {
                string oldPath = Path.Combine(Server.MapPath("~/uploads"), Product.image);
                if (Uploadimage != null)
                {
                    string filename = "Product-" + DateTime.Now.ToString("ddMMyyhhmmss") + Path.GetExtension(Uploadimage.FileName);    
                    System.IO.File.Delete(oldPath);
                    string path = Path.Combine(Server.MapPath("~/uploads"), filename);
                    
                    Uploadimage.SaveAs(path);
                    Product.image = filename;
                }
                db.Entry(Product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyProducts");
            }
            ViewBag.brand_id = new SelectList(db.tb_brands, "id", "name");
            ViewBag.category_id = new SelectList(db.tb_Categories, "cat_id", "cat_name", Product.category_id);
            return View(Product);
        }

        [HttpGet]
        public ActionResult DeleteProduct(int? id)
        {

            int user_id = (int)Session["UserId"];
            tb_Users UserDetails = db.tb_Users.Find(user_id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_products Product = db.tb_products.Find(id);
            if (Product == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (Product.user_id == user_id || UserDetails.type == "مشرف")
                {
                    db.tb_products.Remove(Product);
                    db.SaveChanges();
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
        }


        [HttpGet]
        public ActionResult EditCategory(int? id)
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tb_Categories Category = db.tb_Categories.Find(id);
                if (Category == null)
                {
                    return HttpNotFound();
                }
                return View(Category);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult EditCategory(tb_Categories model)
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {

                if (ModelState.IsValid)
                {
                    //tb_Categories Category = db.tb_Categories.Find(model.cat_id);
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Categories");
                }
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }


        [HttpGet]
        public ActionResult DeleteCategory(int? id)
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tb_Categories Category = db.tb_Categories.Find(id);
                if (Category == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    db.tb_Categories.Remove(Category);
                    db.SaveChanges();
                    return RedirectToAction("Categories");
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult EditInformation()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            return View(DetailsUser);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditInformation(tb_Users Profile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Profile).State = EntityState.Modified;
                db.SaveChanges();
                FormsAuthentication.SetAuthCookie(Profile.username, false);
                return RedirectToAction("EditInformation", "ControlPanal");
            }
            return View(Profile);
        }

        [HttpGet]
        [Authorize]
        public ActionResult NoQProduct()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "بائع")
            {
                var data = db.tb_products.Where(a => a.user_id == DetailsUser.id && a.quantity == 0);
                return View(data.ToList());
            }
            else
            {
                var data = db.tb_products.Where(a => a.quantity == 0);
                return View(data.ToList());
            }

        }

        [HttpGet]
        [Authorize]
        public ActionResult MostParch()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {
                var data = db.MostParch().ToList();
                return View(data);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult addbrand()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {
                return View();
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddBrand(tb_brands model, HttpPostedFileBase Uploadimage)
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {
                var brand = db.tb_brands.Where(a => a.name == model.name || a.englishname == model.englishname).SingleOrDefault();
                if (brand == null)
                {
                    if (ModelState.IsValid)
                    {
                        string filename = "brand-" + DateTime.Now.ToString("ddMMyyhhmmss") + Path.GetExtension(Uploadimage.FileName);
                        string path = Path.Combine(Server.MapPath("~/uploads"), filename);
                        Uploadimage.SaveAs(path);
                        model.logo = filename;
                        db.tb_brands.Add(model);
                        db.SaveChanges();
                        return RedirectToAction("Brands");
                    }
                }
                else
                {
                    ViewBag.msg = "العلامة التجارية موجودة مسبقا";
                }
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Brands()
        {
            return View(db.tb_brands.ToList());
        }
        [HttpGet]
        [Authorize]
        public ActionResult DeleteBrand(int? id)
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tb_brands brand = db.tb_brands.Find(id);
                if (brand == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    db.tb_brands.Remove(brand);
                    db.SaveChanges();
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult EditBrand(int? id)
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tb_brands brand = db.tb_brands.Find(id);
                if (brand == null)
                {
                    return HttpNotFound();
                }
                return View(brand);
            }
            else
            {
                return HttpNotFound();
            } 
        }


        [HttpPost]
        [Authorize]
        public ActionResult EditBrand(tb_brands model, HttpPostedFileBase Uploadimage)
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {

                if (ModelState.IsValid)
                {
                    string oldPath = Path.Combine(Server.MapPath("~/uploads"), model.logo);
                    if (Uploadimage != null)
                    {
                        string filename = "brand-" + DateTime.Now.ToString("ddMMyyhhmmss") + Path.GetExtension(Uploadimage.FileName);
                        System.IO.File.Delete(oldPath);
                        string path = Path.Combine(Server.MapPath("~/uploads"), filename);

                        Uploadimage.SaveAs(path);
                        model.logo = filename;
                    }
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("brands");
                }
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Users()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {
                return View(db.tb_Users.ToList());
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult addNewUser()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {
                ViewBag.type = new SelectList(db.tb_roles, "Name", "Name");
                return View();
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult addNewUser(tb_Users model)
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {
                var user = db.tb_Users.Where(a => a.email == model.email || a.username == model.username).SingleOrDefault();
                if (user == null)
                {
                    if (ModelState.IsValid)
                    {
                        db.tb_Users.Add(model);
                        db.SaveChanges();
                        return RedirectToAction("Users");
                    }
                }
                else
                {
                    ViewBag.msg = "البريد الإلكتروني أو إسم المستخدم مستخدم مسبقاً";
                }
                ViewBag.type = new SelectList(db.tb_roles, "Name", "Name");
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult EditUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Users user = db.tb_Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.type = new SelectList(db.tb_roles, "Name", "Name");
            return View(user);
        }


        [HttpPost]
        [Authorize]
        public ActionResult EditUser(tb_Users model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("users");
            }
            ViewBag.type = new SelectList(db.tb_roles, "Name", "Name");
            return View(model);
        }


        [HttpGet]
        [Authorize]
        public ActionResult DeleteUser(int? id)
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مشرف")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tb_Users user = db.tb_Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    db.tb_Users.Remove(user);
                    db.SaveChanges();
                    return RedirectToAction("Users");
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult ReadyShip()
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مندوب شحن")
            {
                return View(db.tb_checkout.Where(a => a.type == "لم تشحن بعد").ToList());
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult DetailsShip(int id)
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مندوب شحن")
            {
                var DetailsShip = db.tb_checkout.Where(a => a.id == id).SingleOrDefault();
                return View(DetailsShip);
            }
            else
            {
                return HttpNotFound();
            }
        }


        [HttpGet]
        [Authorize]
        public ActionResult DonShip(int id)
        {
            var UserName = User.Identity.Name;
            var DetailsUser = db.tb_Users.Where(a => a.username == UserName).SingleOrDefault();
            if (DetailsUser.type == "مندوب شحن")
            {
                var DetailsShip = db.tb_checkout.Where(a => a.id == id).SingleOrDefault();
                DetailsShip.type = "تم الشحن";
                if (ModelState.IsValid)
                {
                    db.Entry(DetailsShip).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ReadyShip");
                }
                return View(DetailsShip);
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}