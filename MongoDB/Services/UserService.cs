using AutoMapper.Configuration;
using MongoDB.Driver;
using MongoDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _User;
        public UserService()
        {

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("png");
            _User = database.GetCollection<User>("user");
        }

        public List<User> GetUsers()
        {
           return _User.Find(item=>true).ToList();
        }
        public User GetUser(string userName)
        {
            return _User.Find(item => item.username == userName).FirstOrDefault();
        }

        public void AddUser(User user)
        {
            _User.InsertOne(user);
        }
        public void Update(string userName ,User user)
        {
            _User.ReplaceOne(item => item.username == userName, user);
        }
        public void DeleteUser(string userName)
        {
            var user= this.GetUser(userName);
            if(user!=null)
            {
                _User.DeleteOne(item => item.username == userName);
            }    
        }
    }
}
