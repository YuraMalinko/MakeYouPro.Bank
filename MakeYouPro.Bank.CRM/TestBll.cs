using MakeYouPro.Bank.CRM.ExceptionMiddleware;

namespace MakeYouPro.Bank.CRM
{
    public class TestBll
    {
        public void GetTest()
        {
            try
            {
                var repo = new TestDal();
                repo.GetTest();
            }
            catch(MyCustomException ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }

    public class TestDal
    { 
        public void GetTest()
        {
            throw new MyCustomException("Not Implemented Exception");
        }
    }
}
