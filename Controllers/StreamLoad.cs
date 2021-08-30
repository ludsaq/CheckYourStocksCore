using CheckYourStocks.Models;
using Microsoft.EntityFrameworkCore;
using QuikSharp;
using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CheckYourStocks.Controllers
{
    public class StreamLoad
    {
        public DateTime curDate;
        private readonly StockContext dbStocks;
        private readonly DepositionContext dbDeposition;
        public StreamLoad(StockContext dbStocks, DepositionContext dbDeposition)
        {
            this.dbDeposition = dbDeposition;
            this.dbStocks = dbStocks;
            curDate = DateTime.Now;
            InitializationFields();
        }
        public void InitializationFields()
        {
            Quik quik = new Quik(Quik.DefaultPort, new InMemoryStorage());
            SetListDepo(quik);
            SetDictionaryHistory(quik);
            RunLoad();
        }
        private void SetListDepo(Quik quik)
        {
            List<DepoLimitEx> listDepoLimits = quik.Trading.GetDepoLimits().Result;
            List<Deposition> listDep = new List<Deposition>();
            foreach (var item in listDepoLimits)
            {
                Deposition dep = new Deposition(item.SecCode, item.AweragePositionPrice);
                listDep.Add(dep);
            }
            for (int i = 0; i < listDep.Count - 1; i++)
            {
                if ((!listDep.ElementAt(i).Equals(listDep.ElementAt(i + 1))) || (i == listDep.Count - 2))
                    listMoney.listAweragePrice.Add(listDep.ElementAt(i));
            }
        }
        private void SetDictionaryHistory(Quik quik)
        {
            foreach (var item in listMoney.listAweragePrice)
            {
                string classCode = quik.Class.GetSecurityClass("SPBFUT,TQBR,TQBS,TQNL,TQLV,TQNE,TQOB,SPBXM", item.secCode).Result;
                List<Candle> toolCandles = quik.Candles.GetAllCandles(classCode, item.secCode, CandleInterval.M15).Result;
                listMoney.listHistory.Add(item.secCode, toolCandles.ElementAt(toolCandles.Count - 1).Close);
            }
        }
        private async void RunLoad() 
        {
            await Task.Run(() => LoadDatabase());
            Thread.Sleep(3600000);
            RunLoad();
        }
        private bool IsTimeCome()
        {
            if (curDate.Date < DateTime.Now.Date)
                return true;
            else
                return false;
        }
        private void LoadDatabase(bool needDate = false)
        {
            bool isLoadDate = true;
            if (needDate)
                isLoadDate = IsTimeCome();

            if (isLoadDate)
            {
                try
                {
                    // LoadDeposition();
                    // LoadStocks();
                    throw new Exception(); 
                }
                catch (Exception)
                {
                    ErrorViewModel.ErrorGetData = "Невозможно получить данные с приложения Quik";
                }
              
            }
        }
        private async Task LoadDeposition()
        { 
            dbDeposition.Database.ExecuteSqlRaw("truncate table Depositions");
                foreach (var deposit in listMoney.listAweragePrice)
                    dbDeposition.Depositions.Add(deposit);

                await dbDeposition.SaveChangesAsync();
        }
        private async Task LoadStocks()
        {

            foreach (var stock in listMoney.listHistory)
            {
                Stock odjectStock = new Stock
                {
                    id = new Random().Next(2139999999).ToString(),
                    Name = stock.Key,
                    Date = DateTime.Now.Date,
                    Cost = (double)stock.Value
                };
                dbStocks.Stocks.Add(odjectStock);
            }

            await dbStocks.SaveChangesAsync();
        }
    }
}
