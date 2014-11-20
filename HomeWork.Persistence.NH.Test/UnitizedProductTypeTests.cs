using System;
using System.Collections.Generic;
using System.Transactions;
using HomeWork.Persistance.NH;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MITD.Core;
using MITD.Data.NH;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using NHibernate.Linq;


namespace MITD.ProductManagement.Persistance.NH.Test
{
    [TestClass]
    public class UnitizedProductTypeTests
    {

        #region Fields
        private static List<System.Tuple<DBActionType, string, Exception>> expMap = new List<System.Tuple<DBActionType, string, Exception>>();
        #endregion

        #region Constructors
        public UnitizedProductTypeTests()
        {

        }
        #endregion

        #region Additional test attributes

        private TransactionScope scope;
        [TestInitialize()]
        public void MyTestInitialize()
        {
            scope = new TransactionScope();
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            scope.Dispose();
        }

        #endregion

        #region Methods

        public static UnitizedProductType SaveUnitizedProductType(string name, string code, string description,
            ProductTypeCategory productTypeCategory, List<string> featureCategories)
        {
            var session = ProductManagementSession.GetSession();
            var configurator = new AggregateRootConfigurator(new EventPublisher());
            var unitizedProductType = new UnitizedProductType(ProductTypeTestUtil.GetAbstractProductTypeNextId(), name,
                code, description, productTypeCategory, configurator);
            if (featureCategories != null)
            {
                var stringProductFeature = StringProductFeatureTests.SaveStringProductFeature("stringFeature", "desc1");
                unitizedProductType.AddFeatureCategories(featureCategories);
                unitizedProductType.AssignFeature(stringProductFeature, unitizedProductType.FeatureCategories.First(), false, new List<string> { "value1", "value2" });
            }
                
            using (session.BeginTransaction())
            {
                session.Save(unitizedProductType);
                session.Transaction.Commit();
            }
            session.Close();
            return unitizedProductType;
        }

        public static UnitizedProductType SaveUnitizedProductTypeWithFeatureAndProductCategory(string name, string code, string description)
        {
            ProductTypeTestUtil.InitAndRegisterUnitOfMeasureDomain(new Dictionary<string, List<string>>
            {
                {"Unitize", new List<string> {"each"}}
                
            });
            var productTypeCategory = ProductTypeCategoryTests.SaveProductTypeCategory("pt", "desc", null);
           
            var unitizedProductType = SaveUnitizedProductType(name, code, description, productTypeCategory, new List<string> { "featureCat1", "featurCat2" });
            
            return unitizedProductType;
        }

        #endregion


        [TestMethod]
        public void CreateShouldSaveUnitizedProductType()
        {
            #region Arrange

            var session = HomeWorkSession.GetSession();
            var uow = new NHUnitOfWork(session);
            var transaction = new NHTransactionHandler();
            var aggConfigurator = new AggregateRootConfigurator(new EventPublisher());
            //var productCategory = ProductTypeCategoryTests.SaveProductTypeCategory("pt", "desc", null);
           // var stringProductFeature = StringProductFeatureTests.SaveStringProductFeature("stringFeature", "desc1");
            //var unitizedProductType = new UnitizedProductType(ProductTypeTestUtil.GetAbstractProductTypeNextId(),
            //    "unitizedProductType", "Code", "desc1", productCategory, aggConfigurator);
            //unitizedProductType.AddFeatureCategories(new List<string> { "featureCat1", "featurCat2" });
            //unitizedProductType.AssignFeature(stringProductFeature, unitizedProductType.FeatureCategories.First(), false, new List<string> { "value1", "value2" });
            var target = new ProductTypeRepository(transaction, uow, new NHRepositoryExceptionMapper(expMap)
                , aggConfigurator);

            #endregion

            #region Action

            using (session.BeginTransaction())
            {
                //target.Create(unitizedProductType);
                session.Transaction.Commit();
            }

            #endregion

            #region Assert

            //session = ProductManagementSession.GetSession(session.Connection);
            //var actual = session.Query<UnitizedProductType>().Single(u => u.Id == unitizedProductType.Id);
           // ProductTypeTestUtil.UnitizedProductTypeAreEqual(unitizedProductType, actual);

            #endregion
        }

        [TestMethod]
        public void DeleteShouldDeleteUnitizedProductType()
        {
            #region Arrange

            var session = ProductManagementSession.GetSession();
            var uow = new NHUnitOfWork(session);
            var transaction = new NHTransactionHandler();
            var aggConfigurator = new AggregateRootConfigurator(new EventPublisher());
            var productCategory = ProductTypeCategoryTests.SaveProductTypeCategory("pt", "desc", null);
            var unitizedProductTypeArrange = SaveUnitizedProductType("ProductCategory", "code", "desc1", productCategory, null);
            var target = new ProductTypeRepository(transaction, uow, new NHRepositoryExceptionMapper(expMap)
                , aggConfigurator);
            var unitizedProductType =
               session.Query<UnitizedProductType>().Single(p => p.Id == unitizedProductTypeArrange.Id);

            #endregion

            #region Action

            using (session.BeginTransaction())
            {
                target.Delete(unitizedProductType);
                session.Transaction.Commit();
            }

            #endregion

            #region Assert

            session = ProductManagementSession.GetSession(session.Connection);
            var actual = session.Query<UnitizedProductType>().SingleOrDefault(u => u.Id == unitizedProductType.Id);
            Assert.IsNull(actual);

            #endregion
        }

        [TestMethod]
        public void ModifyShouldSaveChangesUnitizedProductType()
        {
            #region Arrange

            var session = ProductManagementSession.GetSession();
            var uow = new NHUnitOfWork(session);
            var transaction = new NHTransactionHandler();
            var aggConfigurator = new AggregateRootConfigurator(new EventPublisher());
            var productCategory = ProductTypeCategoryTests.SaveProductTypeCategory("pt", "desc", null);
            var unitizedProductTypeArrange = SaveUnitizedProductType("ProductCategory", "code", "desc1", productCategory, null);
            var target = new ProductTypeRepository(transaction, uow, new NHRepositoryExceptionMapper(expMap)
                 , aggConfigurator);
            var unitizedProductType = (UnitizedProductType)target.GetBy(unitizedProductTypeArrange.Id);
            #endregion

            #region Action

            unitizedProductType.Update("ProducCategoryChanged", "CodeChanged", "descChanged", productCategory);
            using (session.BeginTransaction())
            {
                session.Transaction.Commit();
            }


            #endregion

            #region Assert

            session = ProductManagementSession.GetSession(session.Connection);
            var actual = session.Query<UnitizedProductType>().Single(u => u.Id == unitizedProductType.Id);
            ProductTypeTestUtil.UnitizedProductTypeAreEqual(unitizedProductType, actual);

            #endregion
        }

    }
}
