using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ModelManagement.Models
{
    public class ModelBase
    {
        public ModelBase() { }
        public ModelBase(ModelBase model)
        {
            ModelId = model.ModelId;
            FirstName = model.FirstName;
            LastName = model.LastName;
            Email = model.Email;
            PhoneNo = model.PhoneNo;
            AddressLine1 = model.AddressLine1;
            AddresLine2 = model.AddresLine2;
            Zip = model.Zip;
            City = model.City;
            BirthDay = model.BirthDay;
            Height = model.Height;
            ShoeSize = model.ShoeSize;
            HairColor = model.HairColor;
            Comments = model.Comments;
        }
        public long ModelId { get; set; }
        [MaxLength(64)]
        public string? FirstName { get; set; }
        [MaxLength(32)]
        public string? LastName { get; set; }
        [MaxLength(254)]
        public string? Email { get; set; }
        [MaxLength(12)]
        public string? PhoneNo { get; set; }
        [MaxLength(64)]
        public string? AddressLine1 { get; set; }
        [MaxLength(64)]
        public string? AddresLine2 { get; set; }
        [MaxLength(9)]
        public string? Zip { get; set; }
        [MaxLength(64)]
        public string? City { get; set; }
        [Column(TypeName = "date")]
        public DateTime BirthDay { get; set; }
        public double Height { get; set; }
        public int ShoeSize { get; set; }
        [MaxLength(32)]
        public string? HairColor { get; set; }
        [MaxLength(1000)]
        public string? Comments { get; set; }
    }

    public class Model : ModelBase
    {
        public ICollection<Job>? Jobs { get; set; } = new List<Job>();
        public ICollection<Expense>? Expenses { get; set; } = new List<Expense>();
    }

    public class ModelWithBase : ModelBase
    {
        public ModelWithBase(Model m) : base(m) 
        {
            foreach(Job J in m.Jobs)
            {
                Jobs.Add((JobBase) J);
            }
            Expenses = m.Expenses;
        }
        public ICollection<JobBase> Jobs { get; set; } = new List<JobBase>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}

