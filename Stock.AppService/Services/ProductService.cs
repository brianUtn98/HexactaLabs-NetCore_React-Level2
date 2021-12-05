using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Stock.AppService.Base;
using Stock.Model.Entities;
using Stock.Repository.LiteDb.Interface;

namespace Stock.AppService.Services
{
   /// <summary>
   /// Product Service.
   /// </summary>
   public class ProductService : BaseService<Product>
   {
       /// <summary>
       /// Initializes a new instance of the <see cref="ProductService"/> class.
       /// </summary>

       ///<param name="repository">Product repository</param>
       public ProductService(IRepository<Product> repository) : base(repository)
       {

       }

    
        /// <summary>
        /// Checks if the product name is unique or not.
        /// </summary>
        /// <param name="name">Product name to check.</param>
        /// <r></r
        private bool NombreUnico(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            return Repository.List(x => x.Name.ToUpper().Equals(name.ToUpper())).Count == 0;
        }

        /// <summary>
        /// Search products.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<Product> Search(Expression<Func<Product, bool>> filter)
        {
            return Repository.List(filter);
        }
   }
}