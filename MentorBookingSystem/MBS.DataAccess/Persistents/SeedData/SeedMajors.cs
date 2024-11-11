using MBS.BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Persistents.SeedData
{
    public class SeedMajors
    {
        private readonly DbInitializer _dbInitializer;
        public SeedMajors(DbInitializer dbInitialize)
        {
            _dbInitializer = dbInitialize;
        }

        public void SeedingMajors()
        {
            var defaultMajors = new List<Major>
            {
                new Major { Id = Guid.Parse("903b6085-4cc3-47f3-bbdd-0f8319e5aabb"), Name = "SE", Status = BusinessObject.Enums.StatusEnum.Activated },
                new Major { Id = Guid.Parse("71577eaf-ebf1-4b23-a48d-cf8561b1c7db"), Name = "SS", Status = BusinessObject.Enums.StatusEnum.Activated },
                new Major { Id = Guid.Parse("dfdb83a4-18e0-447e-9ec8-7c8b39ee6f3a"), Name = "SA", Status = BusinessObject.Enums.StatusEnum.Activated }
            };
            _dbInitializer.Initialize(defaultMajors);
        }




    }
}
