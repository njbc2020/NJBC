using NJBC.DataLayer.Models;

namespace NJBC.App.Crawler
{
    internal class ExecutionEngine
    {
        private NJBC_DBContext context;

        public ExecutionEngine(NJBC_DBContext context)
        {
            this.context = context;
        }
    }
}