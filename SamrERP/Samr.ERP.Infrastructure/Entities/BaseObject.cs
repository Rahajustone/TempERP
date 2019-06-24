using System;
using System.ComponentModel.DataAnnotations;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class BaseObject:IBaseObject
    {
        [Key]
        public Guid Id { get; set; }

        public BaseObject()
        {
            Id = Guid.NewGuid();

            //if (this is IChangeable)
            //{
            //    (this as IChangeable).Created = DateTime.Now;
            //    (this as IChangeable).Updated = DateTime.Now;
            //}
        }
    }

    public interface IBaseObject
    {
        [Key]
        Guid Id { get; set; }
    }
}
