﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arinna.Data.Model;
using Arinna.Test.Model;
using Arinna.Test.Service.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arinna.Test.Tests
{
    [TestClass]
    public class DataAccessAsynchronousTest
    {
        [ClassInitialize]
        public static void TestInit(TestContext testContext)
        {
            const string xmlSchemaPath = @"..\..\DummyData\DummyDataSchema.xsd";
            const string xmlSourcePath = @"..\..\DummyData\DummyDataSource.xml";
            var connectionString = ConfigurationManager.ConnectionStrings["ArinnaTestContext"].ToString();

            NDbUnit.Core.INDbUnitTest mySqlDatabase = new NDbUnit.Core.SqlClient.SqlDbUnitTest(connectionString);

            mySqlDatabase.ReadXmlSchema(xmlSchemaPath);
            mySqlDatabase.ReadXml(xmlSourcePath);

            mySqlDatabase.PerformDbOperation(NDbUnit.Core.DbOperationFlag.CleanInsertIdentity);
        }

        [TestMethod]
        public void TestGetProductAsyncWithPredicate()
        {
            const string obj = "Product1";
            const string expected = "Product1";

            var productService = new ProductService();
            var data = productService.GetProductAsync(x => x.ProductName == obj);

            Assert.IsNotNull(data);
            Assert.IsTrue(data.ProductName == expected);
            Assert.IsNull(data.Category);
        }

        [TestMethod]
        public void TestGetProductAsyncWithPredicateAndPath()
        {
            const string obj = "Product1";
            const string expected = "Product1";

            var productService = new ProductService();
            var data = productService.GetProductAsync(x => x.ProductName == obj, x => x.Category);

            Assert.IsNotNull(data);
            Assert.IsTrue(data.ProductName == expected);
            Assert.IsNotNull(data.Category);
        }

        [TestMethod]
        public void TestGetAllProductsAsync()
        {
            var productService = new ProductService();
            var datas = productService.GetAllProductsAsync();

            Assert.IsNotNull(datas);
            Assert.IsTrue(datas.Any());
            Assert.IsNull(datas.First().Category);
        }

        [TestMethod]
        public void TestGetAllProductsAsyncWithPredicate()
        {
            var productService = new ProductService();
            var datas = productService.GetAllProductsAsync(x => x.IsActive);

            Assert.IsNotNull(datas);
            Assert.IsTrue(datas.Any());
            Assert.IsFalse(datas.Any(x => !x.IsActive));
            Assert.IsNull(datas.First().Category);
        }

        [TestMethod]
        public void TestGetAllProductsAsyncWithPredicateAndPath()
        {
            var productService = new ProductService();
            var datas = productService.GetAllProductsAsync(x => x.IsActive, x => x.Category);

            Assert.IsNotNull(datas);
            Assert.IsTrue(datas.Any());
            Assert.IsFalse(datas.Any(x => !x.IsActive));
            Assert.IsNotNull(datas.First().Category);
        }

        [TestMethod]
        public void TestProductCountAsync()
        {
            const int expected = 10;

            var productService = new ProductService();
            var data = productService.ProductCountAsync();

            Assert.AreEqual(data, expected);
        }

        [TestMethod]
        public void TestProductCountAsyncWithPredicate()
        {
            const int expected = 8;

            var productService = new ProductService();
            var data = productService.ProductCountAsync(x => x.IsActive);

            Assert.AreEqual(data, expected);
        }

        [TestMethod]
        public void TestProductAnyAsync()
        {
            var productService = new ProductService();
            var data = productService.ProductAnyAsync();

            Assert.IsTrue(data);
        }

        [TestMethod]
        public void TestProductAnyAsyncWithPredicate()
        {
            var productService = new ProductService();
            var data = productService.ProductAnyAsync(x => x.IsActive);

            Assert.IsTrue(data);
        }

        [TestMethod]
        public void TestAddProductAsync()
        {
            const string obj = "AddedProduct";
            const int expected = 1;

            var productService = new ProductService();
            var dataCountBefore = productService.ProductCount(x => x.ProductName == obj);
            productService.AddProductAsync(new Product
            {
                ProductName = obj,
                IsActive = true,
                CategoryId = 1,
                ObjectState = ObjectState.Added
            });
            var dataCountAfter = productService.ProductCount(x => x.ProductName == obj);

            Assert.IsTrue(dataCountAfter - dataCountBefore == expected);
        }

        [TestMethod]
        public void TestAddProductRangeAsync()
        {
            const string obj = "AddedProductMulti";
            const int expected = 2;

            var productService = new ProductService();
            var dataCountBefore = productService.ProductCount(x => x.ProductName == obj);
            productService.AddProductRangeAsync(new List<Product> {
                new Product
                {
                    ProductName = obj,
                    IsActive = false,
                    CategoryId = 1,
                    ObjectState = ObjectState.Added
                },
                new Product
                {
                    ProductName = obj,
                    IsActive = false,
                    CategoryId = 1,
                    ObjectState = ObjectState.Added
                }
            });
            var dataCountAfter = productService.ProductCount(x => x.ProductName == obj);

            Assert.IsTrue(dataCountAfter - dataCountBefore == expected);
        }

        [TestMethod]
        public void TestUpdateProductAsync()
        {
            const string objSource = "Product1";
            const string objTarget = "UpdatedProduct";
            const int expected = 1;

            var productService = new ProductService();
            var dataCountBeforeSource = productService.ProductCount(x => x.ProductName == objSource);
            var dataCountBeforeTarget = productService.ProductCount(x => x.ProductName == objTarget);
            var data = productService.GetProduct(x => x.ProductName == objSource);
            data.ProductName = objTarget;
            data.ObjectState = ObjectState.Modified;
            productService.UpdateProductAsync(data);
            var dataCountAfterSource = productService.ProductCount(x => x.ProductName == objSource);
            var dataCountAfterTarget = productService.ProductCount(x => x.ProductName == objTarget);

            Assert.IsTrue(dataCountBeforeSource - dataCountAfterSource == expected && dataCountAfterTarget - dataCountBeforeTarget == expected);
        }

        [TestMethod]
        public void TestUpdateProductRangeAsync()
        {
            const string objSource1 = "Product2";
            const string objSource2 = "Product3";
            const string objTarget = "UpdatedProductMulti";
            const int expected = 2;

            var productService = new ProductService();
            var dataCountBeforeSource = productService.ProductCount(x => x.ProductName == objSource1 || x.ProductName == objSource2);
            var dataCountBeforeTarget = productService.ProductCount(x => x.ProductName == objTarget);
            var datas = productService.GetAllProducts(x => x.ProductName == objSource1 || x.ProductName == objSource2);
            datas.ForEach(x =>
            {
                x.ProductName = objTarget;
                x.ObjectState = ObjectState.Modified;
            });
            productService.UpdateProductRangeAsync(datas);
            var dataCountAfterSource = productService.ProductCount(x => x.ProductName == objSource1 || x.ProductName == objSource2);
            var dataCountAfterTarget = productService.ProductCount(x => x.ProductName == objTarget);

            Assert.IsTrue(dataCountBeforeSource - dataCountAfterSource == expected && dataCountAfterTarget - dataCountBeforeTarget == expected);
        }

        [TestMethod]
        public void TestRemoveProductAsync()
        {
            const string obj = "Product4";
            const int expected = 1;

            var productService = new ProductService();
            var dataBefore = productService.ProductCount(x => x.ProductName == obj);
            var data = productService.GetProduct(x => x.ProductName == obj);
            data.ObjectState = ObjectState.Deleted;
            productService.RemoveProductAsync(data);
            var dataAfter = productService.ProductCount(x => x.ProductName == obj);

            Assert.IsTrue(dataBefore - dataAfter == expected);
        }

        [TestMethod]
        public void TestRemoveProductRangeAsync()
        {
            const string obj1 = "Product5";
            const string obj2 = "Product6";
            const int expected = 2;

            var productService = new ProductService();
            var dataCountBefore = productService.ProductCount(x => x.ProductName == obj1 || x.ProductName == obj2);
            var datas = productService.GetAllProducts(x => x.ProductName == obj1 || x.ProductName == obj2);
            datas.ForEach(x => x.ObjectState = ObjectState.Deleted);
            productService.RemoveProductRangeAsync(datas);
            var dataCountAfter = productService.ProductCount(x => x.ProductName == obj1 || x.ProductName == obj2);

            Assert.IsTrue(dataCountBefore - dataCountAfter == expected);
        }

        [TestMethod]
        public void TestRunCrudProductOperationAsyncForInsert()
        {
            const string obj = "AddedProductWithRunCrud";
            const int expected = 1;

            var productService = new ProductService();
            var dataCountBefore = productService.ProductCount(x => x.ProductName == obj);
            productService.RunCrudProductOperationAsync(new Product
            {
                ProductName = obj,
                IsActive = true,
                CategoryId = 1,
                ObjectState = ObjectState.Added
            });
            var dataCountAfter = productService.ProductCount(x => x.ProductName == obj);

            Assert.IsTrue(dataCountAfter - dataCountBefore == expected);
        }

        [TestMethod]
        public void TestRunCrudProductOperationAsyncForUpdate()
        {
            const string objSource = "Product7";
            const string objTarget = "UpdatedProductWithRunCrud";
            const int expected = 1;

            var productService = new ProductService();
            var dataCountBeforeSource = productService.ProductCount(x => x.ProductName == objSource);
            var dataCountBeforeTarget = productService.ProductCount(x => x.ProductName == objTarget);
            var data = productService.GetProduct(x => x.ProductName == objSource);
            data.ProductName = objTarget;
            data.ObjectState = ObjectState.Modified;
            productService.RunCrudProductOperationAsync(data);
            var dataCountAfterSource = productService.ProductCount(x => x.ProductName == objSource);
            var dataCountAfterTarget = productService.ProductCount(x => x.ProductName == objTarget);

            Assert.IsTrue(dataCountBeforeSource - dataCountAfterSource == expected && dataCountAfterTarget - dataCountBeforeTarget == expected);
        }

        [TestMethod]
        public void TestRunCrudProductOperationAsyncForDelete()
        {
            const string obj = "Product8";
            const int expected = 1;

            var productService = new ProductService();
            var dataCountBefore = productService.ProductCount(x => x.ProductName == obj);
            var data = productService.GetProduct(x => x.ProductName == obj);
            data.ObjectState = ObjectState.Deleted;
            productService.RunCrudProductOperationAsync(data);
            var dataCountAfter = productService.ProductCount(x => x.ProductName == obj);

            Assert.IsTrue(dataCountBefore - dataCountAfter == expected);
        }

        [TestMethod]
        public void TestRunCrudProductOperationRangeAsync()
        {
            const string objSource2 = "Product9";
            const string objSource3 = "Product10";
            const string objTarget1 = "AddedProductWithRunCrudRange";
            const string objTarget2 = "UpdatedProductWithRunCrudRange";
            const int expected1 = 1;
            const int expected2 = 1;
            const int expected3 = 1;

            var productService = new ProductService();
            var dataList = new List<Product>();

            var dataCount1Before = productService.ProductCount(x => x.ProductName == objTarget1);
            dataList.Add(new Product
            {
                ProductName = objTarget1,
                IsActive = true,
                CategoryId = 1,
                ObjectState = ObjectState.Added
            });

            var dataCount2BeforeSource = productService.ProductCount(x => x.ProductName == objSource2);
            var dataCount2BeforeTarget = productService.ProductCount(x => x.ProductName == objTarget2);
            var data2 = productService.GetProduct(x => x.ProductName == objSource2);
            data2.ProductName = objTarget2;
            data2.ObjectState = ObjectState.Modified;
            dataList.Add(data2);

            var dataCount3Before = productService.ProductCount(x => x.ProductName == objSource3);
            var data3 = productService.GetProduct(x => x.ProductName == objSource3);
            data3.ObjectState = ObjectState.Deleted;
            dataList.Add(data3);

            productService.RunCrudProductOperationRangeAsync(dataList);

            var dataCount1After = productService.ProductCount(x => x.ProductName == objTarget1);

            var dataCount2AfterSource = productService.ProductCount(x => x.ProductName == objSource2);
            var dataCount2AfterTarget = productService.ProductCount(x => x.ProductName == objTarget2);

            var dataCount3After = productService.ProductCount(x => x.ProductName == objSource3);

            Assert.IsTrue(dataCount1After - dataCount1Before == expected1);
            Assert.IsTrue(dataCount2BeforeSource - dataCount2AfterSource == expected2 && dataCount2AfterTarget - dataCount2BeforeTarget == expected2);
            Assert.IsTrue(dataCount3Before - dataCount3After == expected3);
        }
    }
}