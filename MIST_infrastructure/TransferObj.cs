using System;

namespace MIST_infrastructure
{
    public class TransferObj<T>
    {
        public bool IsSucceed => Result != null;
        public string FailMessage { get; set; }
        public T Result { get; set; }

        public TransferObj(T result, string failMessage = "")
        {
            Result = result;
            FailMessage = failMessage;
        }
        public TransferObj()
        {

        }
    }
}
