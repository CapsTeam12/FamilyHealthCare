using Business.IServices;
using Data;
using Data.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class DbClient : IDbClient
    {
        private readonly IMongoCollection<Appointment> _appointments;
        public DbClient(IOptions<MongoDbConfig> mongoDbConfig)
        {
            var client = new MongoClient(mongoDbConfig.Value.Connection_String);
            var database = client.GetDatabase(mongoDbConfig.Value.Database_Name);
            _appointments = database.GetCollection<Appointment>(mongoDbConfig.Value.Appointments_Collection_Name);
        }
        public IMongoCollection<Appointment> GetAppointmentsCollection()
        {            
            return _appointments;
        }
    }
}
