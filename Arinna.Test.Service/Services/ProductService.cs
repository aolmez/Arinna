﻿using Arinna.Test.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arinna.Test.Model;
using System.Linq.Expressions;
using System.Security.Cryptography;
using Arinna.Test.Data;
using Arinna.Data.UnitOfWork;

namespace Arinna.Test.Service.Services
{
    public class ProductService : IProductService
    {
        public Product GetProduct(Expression<Func<Product, bool>> predicate)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                return uof.GetRepository<Product>().Get(predicate);
            }
        }

        public Product GetProduct(Expression<Func<Product, bool>> predicate, Expression<Func<Product, object>> path)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                return uof.GetRepository<Product>().Get(predicate, path);
            }
        }

        public List<Product> GetAllProducts()
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                return uof.GetRepository<Product>().GetAll().ToList();
            }
        }

        public List<Product> GetAllProducts(Expression<Func<Product, bool>> predicate)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                return uof.GetRepository<Product>().GetAll(predicate).ToList();
            }
        }

        public List<Product> GetAllProducts(Expression<Func<Product, bool>> predicate, Expression<Func<Product, object>> path)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                return uof.GetRepository<Product>().GetAll(predicate, path).ToList();
            }
        }

        public int ProductCount()
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                return uof.GetRepository<Product>().Count();
            }
        }

        public int ProductCount(Expression<Func<Product, bool>> predicate)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                return uof.GetRepository<Product>().Count(predicate);
            }
        }

        public bool ProductAny()
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                return uof.GetRepository<Product>().Any();
            }
        }

        public bool ProductAny(Expression<Func<Product, bool>> predicate)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                return uof.GetRepository<Product>().Any(predicate);
            }
        }

        public List<Product> IncludeProductRange(IQueryable<Product> products, Expression<Func<Product, object>> path)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                var data = uof.GetRepository<Product>().GetAll();
                return uof.GetRepository<Product>().Include(data, path).ToList();
            }
        }

        public void AddProduct(Product product)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                uof.GetRepository<Product>().Add(product);
                uof.Complete();
            }
        }

        public void AddProductRange(IEnumerable<Product> products)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                uof.GetRepository<Product>().AddRange(products);
                uof.Complete();
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                uof.GetRepository<Product>().Update(product);
                uof.Complete();
            }
        }

        public void UpdateProductRange(IEnumerable<Product> products)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                uof.GetRepository<Product>().UpdateRange(products);
                uof.Complete();
            }
        }

        public void RemoveProduct(Product product)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                uof.GetRepository<Product>().Remove(product);
                uof.Complete();
            }
        }

        public void RemoveProductRange(IEnumerable<Product> products)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                uof.GetRepository<Product>().RemoveRange(products);
                uof.Complete();
            }
        }

        public void RunCrudProductOperation(Product product)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                uof.GetRepository<Product>().RunCrudOperation(product);
                uof.Complete();
            }
        }

        public void RunCrudProductOperationRange(IEnumerable<Product> products)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                uof.GetRepository<Product>().RunCrudOperationRange(products);
                uof.Complete();
            }
        }

        public void ExecuteProductSqlCommand(string sql, params object[] parameters)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                uof.GetRepository<Product>().ExecuteSqlCommand(sql, parameters);
            }
        }

        public void ExecuteProductSqlCommand(IDbCommand sqlCommand)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                uof.GetRepository<Product>().ExecuteSqlCommand(sqlCommand);
            }
        }

        public List<Product> ExecuteProductSqlQuery(string sql, params object[] parameters)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                return uof.GetRepository<Product>().ExecuteSqlQuery(sql, parameters).ToList();
            }
        }

        public List<Product> ExecuteProductSqlQuery(IDbCommand sqlCommand)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                return uof.GetRepository<Product>().ExecuteSqlQuery(sqlCommand).ToList();
            }
        }

        public List<ProductDto> ExecuteProductDtoSqlQuery(string sql, params object[] parameters)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                return uof.GetRepository<Product>().ExecuteSqlQuery<ProductDto>(sql, parameters).ToList();
            }
        }

        public List<ProductDto> ExecuteProductDtoSqlQuery(IDbCommand sqlCommand)
        {
            using (var uof = new UnitOfWork(new ArinnaTestContext()))
            {
                return uof.GetRepository<Product>().ExecuteSqlQuery<ProductDto>(sqlCommand).ToList();
            }
        }
    }
}
