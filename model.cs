NhanvienController
-----------------categorymenu------------
[ChildActionOnly]
        public PartialViewResult CategoryMenu()
        {
            var list = db.ChucVus.ToList();
            return PartialView(list);
        }
        [Route("NhanVien/NhanVienTheoPhong/{CId}")]
        public ActionResult ListNV(int CId)
        {
            var list = db.NhanViens.Where(e => e.CId ==  CId).ToList();
            return View(list);
        }

-------------------------view category----------------------------------

@model IEnumerable<WebApplication12.Models.ChucVu>
@foreach (var item in Model)
{
    var url = "/NhanVien/NhanVienTheoPhong/" + item.CId;
    <a href="@url">@item.TenChucVu</a>
    <label>&nbsp;|&nbsp;</label>
}


--------------------------create------------------------------------------------

 [HttpPost]
        public ActionResult Create(NhanVien nv)
        {
            try
            {
                db.NhanViens.Add(nv);
                db.SaveChanges();
                return Json(new { result = true, JsonRequestBehavior.AllowGet });
            }
            catch (Exception er)
            {
                return Json(new { result = false, error = er.Message });
            }
        }
---------edit----------
 [HttpPost]
        public ActionResult Edit(NhanVien nv)
        {
            try
            {
                db.Entry(nv).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { result = true, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
        }
----------delete-------------
[HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                NhanVien nhanVien = db.NhanViens.Find(id);
                db.NhanViens.Remove(nhanVien);
                db.SaveChanges();
                return Json(new { result = true, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
        }
--------------------------view creat-----------------------------------------------------
 <input type="button" value="Create" id="btnSave" name="btnSave" class="btn btn-primary" />
<br />
<div id="msg" style="color:blue"></div>

@section Scripts {
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnSave").click(function () {
                var nv = {};
                nv.Ten = $("#Ten").val();
                nv.Tuoi = $("#Tuoi").val();
                nv.DiaChi = $("#DiaChi").val();
                nv.Luong = $("#Luong").val();
                nv.CId = $("#CId").val();
                $.ajax({
                    type: "POST",
                    url: '@Url.Action("/Create")',
                    data: '{nv:' + JSON.stringify(nv) + '}',
                    dataType: "json",
                    contentType: "application/json;charset=utf-8",
                    success: function (response) {
                        if (response.result == true) {
                            $("#msg").html("Add data successfully.");

                        } else {
                            $("#msg").html(response.error);
                        }
                    },
                    error: function (response) {
                        alert("Error in ajax block !");
                    }
                });
                return false;
            });
        });
    </script>
    @Scripts.Render("~/bundles/jqueryval")
}
----------------------view edit------------------------------
<input type="button" value="Save" id="btnUpdate" name="btnUpdate" class="btn btn-default" />

<div id="msg" style="color:blue"></div>

@section Scripts {
    @*@Scripts.Render("~/bundles/jqueryval")*@
    <script type="text/javascript">
        $(function () {
            $("#btnUpdate").click(function () {
                var nv = {};
                nv.Ten = $("#Ten").val();
                nv.Tuoi = $("#Tuoi").val();
                nv.DiaChi = $("#DiaChi").val();
                nv.Luong = $("#Luong").val();
                nv.CId = $("#CId").val();
                nv.NId = $("#NId").val();
                $.ajax({
                    type: "POST",
                    url: '@Url.Action("/Edit/")' + nv.NId,
                    data: JSON.stringify(nv),
                    dataType: "json",
                    contentType: "application/json;charset=utf-8",
                    success: function (response) {
                        if (response.result == true) {
                            $("#msg").html("Sửa thành công!");
                        }
                        else {
                            $("#msg").html(response.error);
                        }
                    },
                    error: function (response) {
                        alert("Có lỗi");
                    }
                });
                return false;
            });
        });
    </script>
}
-------------------------------------view delete--------------------------
 @using (Html.BeginForm())
    {
        @Html.AntiForgeryToken()
        @Html.HiddenFor(model => model.NId)
        <div class="form-actions no-color">
            <input type="button" value="Delete" id="btnDelete" name="submitButton" class="btn btn-default" />
            @Html.ActionLink("Back to List", "Index")
        </div>
    }
    <div id="msg" style="color:blue"></div>

@section Scripts {
    <script type="text/javascript">
        $(function () {
            $("#btnDelete").click(function () {
                $.ajax({
                    type: "POST",
                    url: '@Url.Action("/Delete/")' + $("#NId").val(),
                    success: function (response) {
                        if (response.result == true) {
                            $("#msg").html('Xóa thành công.');
                        }
                        else {
                            $("#msg").html(response.error);
                        }
                    },
                    error: function (response) {
                        alert("Có lỗi");
                    }
                });
                return false;
            });
        });
    </script>
}

/////////////////////////////////////////////////////////////////////////////////////
TaikhoansController

 [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Username, string Pass)
        {
            var user = db.TaiKhoans.Where(u => u.Usename == Username && u.Pass == Pass).FirstOrDefault();
            if(user == null)
            {
                ViewBag.errMsg = "Thông tin tài khoản/mật khẩu không chính xác";
                return View("Login");
            }
            else
            {
                Session["Username"] = Username;
                return RedirectToAction("Index", "Home");
            }
            
        }

        public ActionResult Logout()
        {
            Session["Username"] = null;
            return RedirectToAction("Index", "Home");
        }
---------------------view login------------------
@using(Html.BeginForm())
            {
                <p>
                    Username
                    <input type="text" name="Username" />
                </p>
                <p>
                    Password
                    <input type="password" name="Pass" />
                </p>
                <p>
                    <input type="submit" value="Login" />
                </p>
            }
@ViewBag.errMsg

----------------------------layout--------------------
<center>
        @Html.ActionLink("Index", "index", "NhanViens") &nbsp;|&nbsp;
        @{Html.RenderAction("CategoryMenu", "NhanViens");}
        @if (Session["Username"] == null)
        {
            <a href="/TaiKhoans/Login">Login</a>
        }
        else
        {
            <label>Chào mừng bạn &nbsp;<b>@Session["Username"]</b>&nbsp;| &nbsp;</label>
            <a href="/TaiKhoans/Logout">Logout</a>
        }
    </center>