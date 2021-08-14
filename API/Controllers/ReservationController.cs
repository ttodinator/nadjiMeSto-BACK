using Data.UnitOfWork;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ReservationController:BaseApiController
    {
        private IUnitOfWork unitOfWork;

        public ReservationController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<int> aaa()
        {
            return await unitOfWork.RepositoryReservation.GetReservedTablesCount(1,"noon",new DateTime(2002,2,2),5,true);
        }

        [HttpGet("aaa")]
        public async Task<List<Reservation>> ccc()
        {
            return await unitOfWork.RepositoryReservation.GetAll();
        }
    }
}
