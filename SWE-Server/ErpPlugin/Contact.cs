﻿using ErpPlugin.Data;
using ErpPlugin.Data.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin
{
    public class Contact : BusinessObject
    {
        public Contact()
        {
        }

        public string Name { get; set; }

        public string Uid { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Prefix { get; set; }

        public string Suffix { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Email { get; set; }

        public int? AddressId { get; set; }

        public Address Address { get; set; }

        public Address InvoiceAddress { get; set; }

        public Address DeliveryAddress { get; set; }

        public Contact BelongsTo { get; set; }

        public ICollection<Contact> Search()
        {
            if (State != BusinessObjectState.SearchObject)
                throw new InvalidOperationException("Only SearchObjects can be used for searching");

            try
            {
                return CurrentDalFactory.Instance.CreateDal().Search(this);
            }
            catch (ObjectNotFoundException)
            {
                return new List<Contact>();
            }
        }

        public bool Save()
        {
            return CurrentDalFactory.Instance.CreateDal().Save(this);
        }

        public static Contact CreateSearchObject()
        {
            return CreateSearchObject<Contact>();
        }

        public static Contact LoadObject(int id)
        {
            var dal = CurrentDalFactory.Instance.CreateDal();
            var contact = Contact.CreateSearchObject();
            contact.ID = id;
            return dal.Search(contact).SingleOrDefault();
        }

    }
}
