using Data.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IDbClient
    {
        IMongoCollection<Appointment> GetAppointmentsCollection();
    }
}
