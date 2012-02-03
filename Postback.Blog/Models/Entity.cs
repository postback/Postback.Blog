using System;
using Norm;

namespace Postback.Blog.Models
{
    public class Entity
    {
        public Entity()
        {
            Id = ObjectId.NewObjectId();
        }

        [MongoIdentifier]
        public ObjectId Id { get; set; }
    }
}