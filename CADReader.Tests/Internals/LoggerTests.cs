using BojkoSoft.Transformations;
using CAD.Geometry;
using CAD.Internals;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAD.Tests.Internals
{
    [TestClass]
    public class LoggerTests
    {
        [TestMethod]
        public void WritesException()
        {
            Assert.IsNotNull(Logger.Instance);
            Assert.IsNotNull(Logger.Instance.Messages);

            int count = Logger.Instance.Messages.Count;

            this.CalculateSomething();

            Assert.IsTrue(Logger.Instance.Messages.Count == count + 1);
        }

        private void CalculateSomething()
        {
            try
            {
                try
                {
                    try
                    {
                        throw new NullReferenceException("'id'");
                    }
                    catch (Exception ex1)
                    {
                        throw new ArgumentOutOfRangeException("'id' not found in 'points'", ex1);
                    }
                }
                catch (Exception ex2)
                {
                    throw new ArgumentNullException("pointId", ex2);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogError(exception);
            }
        }
    }
}
