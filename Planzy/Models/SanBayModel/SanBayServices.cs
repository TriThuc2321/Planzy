using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.SanBayModel
{
    class SanBayServices
    {
        private static List<SanBay> SanBaysList;

        public SanBayServices()
        {
            SanBaysList = new List<SanBay>()
            {
                new SanBay{Id = "TSN", TenSanBay = "Tân Sơn Nhất" ,ThoiGianDungToiDa = 1, ThoiGianDungToiThieu = 1},
                new SanBay{Id = "TSN2", TenSanBay = "Tân Sơn Nhì" ,ThoiGianDungToiDa = 1, ThoiGianDungToiThieu = 1}
            };
        }
        public List<SanBay> GetAll()
        {
            return SanBaysList;
        }
        public bool Add(SanBay newSanbay)
        {
            if (SanBaysList.Contains(newSanbay))
            {
                return false;
            }    
            else
            {
                SanBaysList.Add(newSanbay);
                return true;
            }    
        }
        public bool Update(SanBay sanBayUpdate)
        {
            bool isUpdate = false;
            for (int index = 0; index < SanBaysList.Count; index++)
            {
                if (SanBaysList[index].Id == sanBayUpdate.Id)
                {
                    SanBaysList[index].TenSanBay = sanBayUpdate.TenSanBay;
                    isUpdate = true;
                }
            }
            return isUpdate;
        }
        public bool Delete(string Id)
        {
            bool isDeleted = false;
            for (int index = 0; index < SanBaysList.Count; index++)
            {
                if (SanBaysList[index].Id == Id)
                {
                    SanBaysList.RemoveAt(index);
                    isDeleted = true;
                    break;
                }
            }
            return isDeleted;
        }
        public SanBay SearchID(string Id)
        {
            return SanBaysList.FirstOrDefault(e => e.Id == Id);
        }
        public SanBay SearchTen(string tenSanBay)
        {
            return SanBaysList.FirstOrDefault(e => e.TenSanBay == tenSanBay);
        }
    }
}
