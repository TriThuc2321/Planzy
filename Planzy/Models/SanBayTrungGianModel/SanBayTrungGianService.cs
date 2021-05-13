using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.SanBayTrungGianModel
{
    class SanBayTrungGianService
    {
        private static List<SanBayTrungGian> SanBayTrungGiansList;

        public SanBayTrungGianService()
        {
            SanBayTrungGiansList = new List<SanBayTrungGian>()
            {
                new SanBayTrungGian{MaSanBay = "HN", MaChuyenBay= "MH30", MaSanBayTruoc = "", MaSanBaySau = "DN", ThoiGianDung = 10},
                new SanBayTrungGian{MaSanBay = "DN", MaChuyenBay= "MH30", MaSanBayTruoc = "HN", MaSanBaySau = "", ThoiGianDung = 10},
                new SanBayTrungGian{MaSanBay = "DN", MaChuyenBay= "MH30", MaSanBayTruoc = "HN", MaSanBaySau = "", ThoiGianDung = 10},
            };
        }
        public List<SanBayTrungGian> GetAll()
        {
            return SanBayTrungGiansList;
        }
        //public bool Add(SanBayTrungGian newSanbay)
        //{
        //    if (SanBayTrungGianList.Contains(newSanbay))
        //    {
        //        return false;
        //    }    
        //    else
        //    {
        //        SanBaysList.Add(newSanbay);
        //        return true;
        //    }    
        //}
        //public bool Update(SanBay sanBayUpdate)
        //{
        //    bool isUpdate = false;
        //    for (int index = 0; index < SanBaysList.Count; index++)
        //    {
        //        if (SanBaysList[index].Id == sanBayUpdate.Id)
        //        {
        //            SanBaysList[index].TenSanBay = sanBayUpdate.TenSanBay;
        //            isUpdate = true;
        //        }
        //    }
        //    return isUpdate;
        //}
        //public bool Delete(string Id)
        //{
        //    bool isDeleted = false;
        //    for (int index = 0; index < SanBaysList.Count; index++)
        //    {
        //        if (SanBaysList[index].Id == Id)
        //        {
        //            SanBaysList.RemoveAt(index);
        //            isDeleted = true;
        //            break;
        //        }
        //    }
        //    return isDeleted;
        //}
        //public SanBay SearchID(string Id)
        //{
        //    return SanBaysList.FirstOrDefault(e => e.Id == Id);
        //}
        //public SanBay SearchTen(string tenSanBay)
        //{
        //    return SanBaysList.FirstOrDefault(e => e.TenSanBay == tenSanBay);
        //}
    }
}
