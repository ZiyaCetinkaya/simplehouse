﻿using simplehouse.DataAccess;
using simplehouse.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace simplehouse.Controllers
{
    public class AdminController : Controller
    {
        CategoryDataAccess categoryDA = new CategoryDataAccess();
        StateDataAccess stateDA = new StateDataAccess();
        FoodDataAccess foodDA = new FoodDataAccess();
        MemberDataAccess memberDA = new MemberDataAccess();

        [Route("admin")]
        public ActionResult Index()
        {
            return View();
        }

        #region Kategori
        [Route("admin/kategoriler")]
        public ActionResult Categories()
        {
            List<CATEGORY> categories = categoryDA.GetAll(null);
            return View(categories);
        }

        [Route("admin/kategori-ekle")]
        public ActionResult CategoryInsert()
        {
            PopulateStateDropdownList();
            return View();
        }

        [HttpPost]
        public ActionResult InsertCategoryForm(CATEGORY category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    categoryDA.Insert(category);
                    return RedirectToAction("Categories", "Admin");
                }
                else
                {
                    ViewBag.Error = "Try Again.";
                    PopulateStateDropdownList(category.STATE_ID);
                    return View("CategoryInsert", category);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                PopulateStateDropdownList(category.STATE_ID);
                return View("CategoryInsert", category);
            }
        }

        [Route("admin/kategori-duzenle/{id}")]
        public ActionResult CategoryUpdate(int id)
        {
            CATEGORY category = categoryDA.GetById(id);
            PopulateStateDropdownList(category.STATE_ID);
            return View(category);
        }

        [HttpPost]
        public ActionResult UpdateCategoryForm(CATEGORY category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    categoryDA.Update(category);
                    return RedirectToAction("Categories", "Admin");
                }
                else
                {
                    ViewBag.Error = "Try Again.";
                    PopulateStateDropdownList(category.STATE_ID);
                    return View("CategoryUpdate", category);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                PopulateStateDropdownList(category.STATE_ID);
                return View("CategoryUpdate", category);
            }
        }

        [Route("admin/kategori-sil/{id}")]
        public ActionResult CategoryDelete(int id)
        {
            CATEGORY category = categoryDA.GetById(id);
            return View(category);
        }

        [HttpPost]
        public ActionResult DeleteCategoryForm(int id)
        {
            CATEGORY category = categoryDA.GetById(id);
            try
            {
                if (ModelState.IsValid)
                {
                    categoryDA.Delete(id);
                    return RedirectToAction("Categories", "Admin");
                }
                else
                {
                    ViewBag.Error = "Try Again.";
                    return View("CategoryDelete", category);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                return View("CategoryDelete", category);
            }
        }
        #endregion

        #region Yiyecekler

        [Route("admin/yiyecekler")]
        public ActionResult Foods()
        {
            List<FOOD> foods = foodDA.GetAll(null);
            return View(foods);
        }

        [Route("admin/yiyecek/{id}")]
        public ActionResult Food(int id)
        {
            FOOD food = foodDA.GetById(id);
            return View(food);
        }

        [Route("admin/yiyecek-ekle")]
        public ActionResult FoodInsert()
        {
            PopulateStateDropdownList();
            PopulateCategoryDropdownList();
            return View();
        }

        [HttpPost]
        public ActionResult InsertFoodForm(FOOD food)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Request.Files.Count > 0)
                    {
                        string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                        string uzanti = Path.GetExtension(Request.Files[0].FileName);
                        string tamYolYeri = "~/Content/images/foods/" + dosyaAdi + uzanti;
                        Request.Files[0].SaveAs(Server.MapPath(tamYolYeri));
                        food.IMAGE = dosyaAdi + uzanti;
                    }
                    else
                    {
                        ViewBag.Error = "Resim eklemelisiniz.";
                        PopulateStateDropdownList(food.STATE_ID);
                        PopulateCategoryDropdownList(food.CATEGORY_ID);
                        return View("FoodInsert", food);
                    }

                    foodDA.Insert(food);
                    return RedirectToAction("Foods", "Admin");
                }
                else
                {
                    ViewBag.Error = "Try Again.";
                    PopulateStateDropdownList(food.STATE_ID);
                    PopulateCategoryDropdownList(food.CATEGORY_ID);
                    return View("FoodInsert", food);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                PopulateStateDropdownList(food.STATE_ID);
                PopulateCategoryDropdownList(food.CATEGORY_ID);
                return View("FoodInsert", food);
            }
        }

        [Route("admin/yiyecek-duzenle/{id}")]
        public ActionResult FoodUpdate(int id)
        {
            FOOD food = foodDA.GetById(id);
            PopulateStateDropdownList(food.STATE_ID);
            PopulateCategoryDropdownList(food.CATEGORY_ID);
            return View(food);
        }

        [HttpPost]
        public ActionResult UpdateFoodForm(FOOD food)
        {
            FOOD _food = foodDA.GetById(food.ID);
            try
            {
                if (ModelState.IsValid)
                {
                    if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/Content/images/foods/" + _food.IMAGE)))
                            System.IO.File.Delete(Server.MapPath("~/Content/images/foods/" + _food.IMAGE));

                        string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                        string uzanti = Path.GetExtension(Request.Files[0].FileName);
                        string tamYolYeri = "~/Content/images/foods/" + dosyaAdi + uzanti;
                        Request.Files[0].SaveAs(Server.MapPath(tamYolYeri));
                        food.IMAGE = dosyaAdi + uzanti;
                    }
                    else
                        food.IMAGE = _food.IMAGE;

                    foodDA.Update(food);
                    return RedirectToAction("Foods", "Admin");
                }
                else
                {
                    ViewBag.Error = "Try Again.";
                    PopulateStateDropdownList(_food.STATE_ID);
                    PopulateCategoryDropdownList(_food.CATEGORY_ID);
                    return View("FoodUpdate", _food);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                PopulateStateDropdownList(_food.STATE_ID);
                PopulateCategoryDropdownList(_food.CATEGORY_ID);
                return View("FoodUpdate", _food);
            }
        }

        [Route("admin/yiyecek-sil/{id}")]
        public ActionResult FoodDelete(int id)
        {
            FOOD food = foodDA.GetById(id);
            return View(food);
        }

        [HttpPost]
        public ActionResult DeleteFoodForm(int id)
        {
            FOOD food = foodDA.GetById(id);
            try
            {
                if (ModelState.IsValid)
                {
                    foodDA.Delete(id);

                    if (System.IO.File.Exists(Server.MapPath("~/Content/images/foods/" + food.IMAGE)))
                        System.IO.File.Delete(Server.MapPath("~/Content/images/foods/" + food.IMAGE));

                    return RedirectToAction("Foods", "Admin");
                }
                else
                {
                    ViewBag.Error = "Try Again.";
                    return View("FoodDelete", food);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                return View("FoodDelete", food);
            }
        }

        #endregion
        [Route("admin/takim-uyeleri")]
        public ActionResult Members()
        {
            List<MEMBER> members = memberDA.GetAll(null);
            return View(members);
        }

        [Route("admin/takim-uyesi/{id}")]
        public ActionResult Member(int id)
        {
            MEMBER member = memberDA.GetById(id);
            return View(member);
        }

        [Route("admin/takim-uyesi-ekle")]
        public ActionResult MemberInsert()
        {
            PopulateStateDropdownList();
            return View();
        }

        [HttpPost]
        public ActionResult InsertMemberForm(MEMBER member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Request.Files.Count > 0)
                    {
                        string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                        string uzanti = Path.GetExtension(Request.Files[0].FileName);
                        string tamYolYeri = "~/Content/images/members/" + dosyaAdi + uzanti;
                        Request.Files[0].SaveAs(Server.MapPath(tamYolYeri));
                        member.IMAGE = dosyaAdi + uzanti;
                    }
                    else
                    {
                        ViewBag.Error = "Resim eklemelisiniz.";
                        PopulateStateDropdownList(member.STATE_ID);
                        return View("MemberInsert", member);
                    }

                    memberDA.Insert(member);
                    return RedirectToAction("Members", "Admin");
                }
                else
                {
                    ViewBag.Error = "Try Again.";
                    PopulateStateDropdownList(member.STATE_ID);
                    return View("MemberInsert", member);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                PopulateStateDropdownList(member.STATE_ID);
                return View("MemberInsert", member);
            }
        }

        [Route("admin/takim-uyesi-duzenle/{id}")]
        public ActionResult MemberUpdate(int id)
        {
            MEMBER member = memberDA.GetById(id);
            PopulateStateDropdownList(member.STATE_ID);
            return View(member);
        }

        [HttpPost]
        public ActionResult UpdateMemberForm(MEMBER member)
        {
            MEMBER _member = memberDA.GetById(member.ID);
            try
            {
                if (ModelState.IsValid)
                {
                    if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/Content/images/members/" + _member.IMAGE)))
                            System.IO.File.Delete(Server.MapPath("~/Content/images/members/" + _member.IMAGE));

                        string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                        string uzanti = Path.GetExtension(Request.Files[0].FileName);
                        string tamYolYeri = "~/Content/images/members/" + dosyaAdi + uzanti;
                        Request.Files[0].SaveAs(Server.MapPath(tamYolYeri));
                        member.IMAGE = dosyaAdi + uzanti;
                    }
                    else
                        member.IMAGE = _member.IMAGE;

                    memberDA.Update(member);
                    return RedirectToAction("Members", "Admin");
                }
                else
                {
                    ViewBag.Error = "Try Again.";
                    PopulateStateDropdownList(_member.STATE_ID);
                    return View("MemberUpdate", _member);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                PopulateStateDropdownList(_member.STATE_ID);
                return View("MemberUpdate", _member);
            }
        }

        [Route("admin/takim-uyesi-sil/{id}")]
        public ActionResult MemberDelete(int id)
        {
            MEMBER member = memberDA.GetById(id);
            return View(member);
        }

        [HttpPost]
        public ActionResult DeleteMemberForm(int id)
        {
            MEMBER member = memberDA.GetById(id);
            try
            {
                if (ModelState.IsValid)
                {
                    memberDA.Delete(id);

                    if (System.IO.File.Exists(Server.MapPath("~/Content/images/members/" + member.IMAGE)))
                        System.IO.File.Delete(Server.MapPath("~/Content/images/members/" + member.IMAGE));

                    return RedirectToAction("Members", "Admin");
                }
                else
                {
                    ViewBag.Error = "Try Again.";
                    return View("MemberDelete", member);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Try Again.";
                return View("MemberDelete", member);
            }
        }


        [Route("admin/iletisim-bilgileri")]
        public ActionResult ContactInfo()
        {
            return View();
        }

        [Route("admin/iletisim-formlari")]
        public ActionResult ContactForms()
        {
            return View();
        }

        [Route("admin/sifre-degistir")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        private void PopulateStateDropdownList(object selectedState = null)
        {
            var statesQuery = stateDA.GetAll();
            ViewBag.STATE_ID = new SelectList(statesQuery, "ID", "NAME", selectedState);
        }

        private void PopulateCategoryDropdownList(object selectedCategory = null)
        {
            var categoriesQuery = categoryDA.GetAll(true);
            ViewBag.CATEGORY_ID = new SelectList(categoriesQuery, "ID", "NAME", selectedCategory);
        }
    }
}