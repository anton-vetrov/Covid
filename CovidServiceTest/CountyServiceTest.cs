using CovidService.Repositories;
using CovidService.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CovidServiceTest
{
    [TestClass]
    public class CountyServiceTest
    {
        private static FileRepository _fileRepository;

        private CountyService _countyService;

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            using (var memoryStream = new MemoryStream(Properties.Resources.Covid19ConfirmedUS))
            {
                _fileRepository = new FileRepository(memoryStream);
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _countyService = new CountyService(_fileRepository);
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }


        [TestMethod]
        public void GetSummary_ReturnsList()
        {
            var summary = _countyService.GetSummary("Harris", new DateTime(2023, 02, 01), new DateTime(2023, 02, 13), 0, 1);

        }

    }
}
