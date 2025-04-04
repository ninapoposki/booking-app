﻿using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{


    public enum EntityType { NONE, TOUR, ACCOMMODATION,TOURGRADE,GUEST };

    public class Image : ISerializable
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int EntityId { get; set; } //id ture
        public EntityType EntityType { get; set; }
        public Image() { }

        public Image(int id, string path, int entityId, EntityType entityType) {
        
            Id=id;
            Path=path;
            EntityId=entityId;
            EntityType=entityType;
        }
        public void FromCSV(string[] values)
        {
            Id= Convert.ToInt32(values[0]);
            Path = values[1];
            EntityId = Convert.ToInt32(values[2]);
            //da za gostaizbacuje te slike
            if (values[3] == "ACCOMMODATION") { EntityType = EntityType.ACCOMMODATION; } //ovde dodaj GUEST I TOUR da bi mogli da se prikaze
            else if (values[3] == "TOUR") { EntityType = EntityType.TOUR; }
            else if (values[3] == "TOURGRADE") { EntityType = EntityType.TOURGRADE; }
            else if (values[3] == "GUEST") { EntityType = EntityType.GUEST; }
            else { EntityType = EntityType.NONE; }
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Path,
                EntityId.ToString(),
                EntityType.ToString()
            };
            return csvValues;
        }
    }
}
