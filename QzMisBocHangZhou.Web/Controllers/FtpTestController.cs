using QzMisBocHangZhou.Biz;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class FtpTestController : Controller
    {
        // GET: FtpTest
        public ActionResult Index()
        {
            var ip = ConfigurationManager.AppSettings["FtpServerIP"];
            var ftpPath = ConfigurationManager.AppSettings["FtpRemotePath"];
            var user = ConfigurationManager.AppSettings["FtpUserID"];
            var pwd = ConfigurationManager.AppSettings["FtpPassword"];

            FTPHelper ftp = new FTPHelper(ip, ftpPath, user, pwd);

            var fileList = ftp.ListFiles();
            foreach(var item in fileList)
            {
                if (item.IsDirectory) continue;

                var name = Path.GetFileName(item.Path);
                var path = Path.Combine(Server.MapPath("../FtpFile"), name);
                ftp.Download(path, item.Path);

                LoadImportFile(path);
            }

            return View();
        }

        private void LoadImportFile(string filePath)
        {
            var content = System.IO.File.ReadAllLines(filePath);

            //foreach (var item in content)
            //{
            //    if (string.IsNullOrWhiteSpace(item)) continue;

            //    var data = item.Split(new string[] { " | " }, StringSplitOptions.None);
            //    if (data.Length < 23) continue;

            //    TPArchiveInfo info = new TPArchiveInfo();
            //    info.StorageLocation = data[0];
            //    info.LoanAccount = data[2];
            //    info.QuotaNo = data[3];
            //    info.Borrower = data[4];
            //    if (!string.IsNullOrWhiteSpace(data[5]))
            //    {
            //        decimal.TryParse(data[5], out decimal amount);
            //        info.LoanAmount = amount;
            //    }
            //    info.GuaranteeType = data[6];
            //    info.ArchiveStatus = data[7];
            //    info.GuaranteeCredentialNo = data[8];
            //    info.OrgName = data[9];

            //    if (!string.IsNullOrWhiteSpace(data[10]))
            //    {
            //        DateTime.TryParse(data[10], out DateTime editDate);
            //        info.EditDate = editDate;
            //    }

            //    info.Transactor = data[11];
            //    //info.PreReturnDate = data[12];
            //    //info.ReturnDate = data[13];
            //    info.GuaranteeNo = data[14];
            //    info.OrgCode = data[15];
            //    info.ArchiveNo = data[16];
            //    info.ProductCode = data[17];
            //    //info.LoanReleaseDate = data[18];
            //    //info.MortgageValue = data[19];
            //    info.ContractNo = data[20];
            //    info.HouseLocation = data[21];
            //    info.AccountManager = data[22];
            //    info.CustomerNo = data[23];
            //    TPArchiveInfoBiz.Edit(info);
            //}
        }
    }
}