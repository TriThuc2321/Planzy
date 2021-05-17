using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planzy.Models.ChuyenBayModel;
namespace Planzy.Models.ChuyenBayModel
{
    class ChuyenBayServices
    {
        private static List<ChuyenBay> ChuyenBaysList;

        public ChuyenBayServices()
        {
            ChuyenBaysList = new List<ChuyenBay>();
        }
        public List<ChuyenBay> GetAll()
        {
            return ChuyenBaysList;
        }
        public bool Add(ChuyenBay newChuyenBay)
        {
            if (ChuyenBaysList.Count == 0)
            {
                ChuyenBaysList.Add(newChuyenBay);
                return true;
            }
            else
            {
               if ( ChuyenBaysList.Exists(e => e.MaChuyenBay == newChuyenBay.MaChuyenBay))
                {
                    return false;
                }    
               else
                {
                    ChuyenBaysList.Add(newChuyenBay);
                    return true;
                }    
            }

        }
        public bool Update(ChuyenBay chuyenBayUpdate)
        {
            bool isUpdate = false;
            for (int index = 0; index < ChuyenBaysList.Count; index++)
            {
                if (ChuyenBaysList[index].MaChuyenBay == chuyenBayUpdate.MaChuyenBay)
                {
                    ChuyenBaysList[index].MaChuyenBay = chuyenBayUpdate.MaChuyenBay;
                    isUpdate = true;
                }
            }
            return isUpdate;
        }
        public bool Delete(string Id)
        {
            bool isDeleted = false;
            for (int index = 0; index < ChuyenBaysList.Count; index++)
            {
                if (ChuyenBaysList[index].MaChuyenBay == Id)
                {
                    ChuyenBaysList.RemoveAt(index);
                    isDeleted = true;
                    break;
                }
            }
            return isDeleted;
        }
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
