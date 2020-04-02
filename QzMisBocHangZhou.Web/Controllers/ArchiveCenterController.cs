using QzMisBocHangZhou.Biz;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class ArchiveCenterController : Controller
    {
        // GET: ArchiveCenter
        public ActionResult ArchiveTransferCheckView()
        {
            return View();
        }

        public ActionResult ArchiveTransferCheckEditView(string mode, string id)
        {
            return View();
            //if (string.IsNullOrWhiteSpace(id))
            //{
            //    return View(new ArchiveTransferInfo());
            //}
            //else
            //{
            //    var result = ArchiveTransferInfoBiz.Get(id);
            //    return View(result);
            //}
        }
    }
}