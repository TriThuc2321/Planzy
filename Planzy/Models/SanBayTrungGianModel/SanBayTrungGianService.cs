using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planzy.Models.SanBayModel;
namespace Planzy.Models.SanBayTrungGianModel
{
    class SanBayTrungGianService
    {
        private static List<SanBayTrungGian> SanBayTrungGiansList;

        public SanBayTrungGianService()
        {
            SanBayTrungGiansList = new List<SanBayTrungGian>();
        }
        public List<SanBayTrungGian> GetAll()
        {
            return SanBayTrungGiansList;
        }
        public bool Add(SanBayTrungGian newSanbay)
        {
            if (SanBayTrungGiansList.Count == 0)
            {
                SanBayTrungGiansList.Add(newSanbay);
                return true;
            }
            else
            {
                for (int index = 0; index < SanBayTrungGiansList.Count; index++)
                {
                    if (newSanbay.MaSanBaySau == SanBayTrungGiansList[index].MaSanBay)
                    {
                        SanBayTrungGiansList.Insert(index, newSanbay);
                        return true;
                    }    
                }
                SanBayTrungGiansList.Add(newSanbay);
                return true;
            }    
               
        }
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
